using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace H00N.Network
{
    public abstract class Session
    {
        public const int HeaderSize = sizeof(ushort);

        private Socket socket; 

        private int active = 0;
        public int Active => active;

        private SharedBuffer receiveBuffer = new SharedBuffer(65536); 
        private Queue<ArraySegment<byte>> sendQueue = new Queue<ArraySegment<byte>>();
        private List<ArraySegment<byte>> pendingList = new List<ArraySegment<byte>>(); 

        private SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();

        private object locker = new object();

        public abstract void OnConnected(EndPoint endPoint);
        public abstract void OnDisconnected(EndPoint endPoint);
        public abstract void OnPacketReceived(ArraySegment<byte> buffer);
        public abstract void OnSent(int length);

        public void Open(Socket socket)
        {
            if (Interlocked.Exchange(ref active, 1) == 1) 
                return;

            this.socket = socket;

            SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.Completed += OnReceiveCompleted;
            sendArgs.Completed += OnSendCompleted;

            Receive(receiveArgs);
        }

        public void Close()
        {
            if (Interlocked.Exchange(ref active, 0) == 0) 
                return;

            EndPoint endPoint = socket.RemoteEndPoint;

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            Release();

            OnDisconnected(endPoint);
        }

        public void Send(ArraySegment<byte> sendBuffer)
        {
            lock(locker)
            {
                sendQueue.Enqueue(sendBuffer); 
                if (pendingList.Count == 0) 
                    FlushSendQueue(); 
            }
        }

        private void Release()
        {
            lock (locker)
            {
                sendQueue.Clear();
                pendingList.Clear(); 
            }
        }

        #region Send
        private void FlushSendQueue() // sendQueue를 비우고 데이터를 전송하는 함수
        {
            if (active == 0) // 연결이 끊겼다면
                return; // return

            while (sendQueue.Count > 0) // sendQueue가 빌 때까지
            {
                ArraySegment<byte> buffer = sendQueue.Dequeue(); // 패킷 pop
                pendingList.Add(buffer); // pendingList(예약 리스트)에 추가
            }

            sendArgs.BufferList = pendingList; // 송신 인자에 버퍼들 세팅

            bool pending = socket.SendAsync(sendArgs); // 비동기 전송
            if (pending == false) // 만약 동기적으로 처리되었다면
                OnSendCompleted(null, sendArgs); // 직접 콜백 실행
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs args) // 송신 완료 콜백
        {
            lock (locker)
            {
                if (args.SocketError == SocketError.Success && args.BytesTransferred > 0) // 에러 없이 데이터가 잘 전송되었다면
                {
                    sendArgs.BufferList = null; // 송신 인자 버퍼 비우기
                    pendingList.Clear(); // 예약 리스트 비우기

                    OnSent(args.BytesTransferred); // 데이터가 전송되었음을 알림

                    if (sendQueue.Count > 0) // 모든 작업이 끝났을 때 예약된 패킷이 존재한다면
                        FlushSendQueue(); // 다시 sendQueue Flsuh
                }
                else
                    Close(); // 문제가 생겼다면 접속 해제
            }
        }
        #endregion

        #region Receive
        private void Receive(SocketAsyncEventArgs args) // 데이터 수신 함수
        {
            receiveBuffer.PurifyBuffer();

            ArraySegment<byte> buffer = receiveBuffer.WriteBuffer; // 데이터를 받을 버퍼 발급
            args.SetBuffer(buffer.Array, buffer.Offset, buffer.Count); // 수신 인자에 버퍼 세팅

            bool pending = socket.ReceiveAsync(args); // 비동기 수신
            if (pending == false) // 만약 동기적으로 처리되었다면
                OnReceiveCompleted(null, args); // 직접 콜백 실행
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs args) // 수신 완료 콜백
        {
            if(args.SocketError == SocketError.Success && args.BytesTransferred > 0) // 에러 없이 데이터가 수신되었다면
            {
                bool result = receiveBuffer.ShiftWriteCursor(args.BytesTransferred); // 데이터를 받은 만큼 WriteCursor 업데이트

                if(result == false) // WriteCursor 업데이트 중 문제가 생겼다면
                {
                    Close(); // 연결 해제
                    return; // return
                }

                ArraySegment<byte> buffer = receiveBuffer.ReadBuffer; // 데이터를 읽을 버퍼 발급

                int processedLength = HandleBuffer(buffer); // 버퍼 핸들링
                if(processedLength < 0 || receiveBuffer.Size < processedLength) // 핸들링 도중 문제가 생겼다면
                {
                    Close(); // 연결 해제
                    return; // return
                }

                result = receiveBuffer.ShiftReadCursor(processedLength); // 핸들링 된 만큼만 ReadCursor 업데이트
                if(result == false) // ReadCursor 업데이트 중 문제가 생겼다면
                {
                    Close(); // 연결 해제
                    return; // return
                }

                Receive(args); // 다시 수신 요청
            }
            else
            {
                Close(); // 문제가 생겼다면 연결 해제
            }
        }

        private int HandleBuffer(ArraySegment<byte> buffer) // 수신한 데이터를 패킷으로 해석하는 함수
        {
            int processedLength = 0; // 처리한 길이

            while(true)
            {
                if (buffer.Count < HeaderSize) // 읽을 데이터가 header의 사이즈보다 작다면
                    break; // 루프 끝내기

                // [길이] [타입] [데이터...]
                ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset); // 데이터 사이즈 추출
                if (buffer.Count < dataSize) // 데이터의 사이즈보다 버퍼의 크기가 작다면 (데이터가 모두 수신되지 않았다는 뜻)
                    break; // 루프 끝내기

                // 여기까지 왔다면 정상적인 패킷이 존재한다는 뜻

                OnPacketReceived(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize)); // Offset부터 패킷의 길이만큼 잘라서 패킷 넘기기

                processedLength += dataSize; // 처리된 길이에 추가
                buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize); // 버퍼에서 처리된 만큼을 제외
            }

            return processedLength; // 처리한 길이 반환
        }
        #endregion
    }
}

namespace H00N.Network
{
    public class SharedBuffer
    {
        private ArraySegment<byte> buffer; // 실제 버퍼
        private int readCursor; // ReadCursor (읽어야 할 커서, 읽은부분)
        private int writeCursor; // WriteCursor (적어야 할 커서, 적은부분)

        public int Size => writeCursor - readCursor; // 읽어야 할 데이터의 사이즈 (적은부분 - 읽은부분)
        public int Capacity => buffer.Count - writeCursor; // 남은 데이터의 사이즈 (전체의 길이 - 적은부분)

        public ArraySegment<byte> ReadBuffer => new ArraySegment<byte>(buffer.Array, buffer.Offset + readCursor, Size); // 읽을 할 부분 (읽은부분 ~ Size)
        public ArraySegment<byte> WriteBuffer => new ArraySegment<byte>(buffer.Array, buffer.Offset + writeCursor, Capacity); // 적을 부분 (적은 부분 ~ Capacity)

        public SharedBuffer(int bufferSize) // 생성자
        {
            buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize); // 버퍼 생성
        }

        public void PurifyBuffer() // 버퍼를 정리하는 함수
        {
            int dataLength = Size; // 현재 데이터의 길이
            if(dataLength == 0) // 만약 남은 데이터가 없다면
                readCursor = writeCursor = 0; // 현재 버퍼의 값들을 모두 무시하고 ReadCursor 와 WriteCursor를 가장 앞으로 이동
            else // 남아있는 데이터가 있다면
            {
                Array.Copy(buffer.Array, buffer.Offset + readCursor, buffer.Array, buffer.Offset, dataLength); // 남아있는 데이터를 가장 앞으로 이동
                readCursor = 0; // ReadCursor 가장 앞으로 이동
                writeCursor = dataLength; // 남아있는 데이터 뒤에서부터 적을 수 있도록 WriteCursor를 남은 데이터의 끝부분으로 이동
            }
        }

        public bool ShiftReadCursor(int readSize) // ReadCursor를 업데이트하는 함수
        {
            if (readSize > Size) // 이동하고자 하는 길이가 남아있던 데이터의 사이즈보다 크면 (문제 발생)
                return false; // 실패 반환

            // 그게 아니라면

            readCursor += readSize; // ReadCursor 이동
            return true; // 성공 반환
        }

        public bool ShiftWriteCursor(int writtenSize) // WriteCursor를 업데이트하는 함수
        {
            if (writtenSize > Capacity) // 이동하고자 하는 길이가 남은 버퍼의 사이즈보다 크면 (문제 발생)
                return false; // 실패 반환

            // 그게 아니라면

            writeCursor += writtenSize; // WriteCursor 이동
            return true; // 성공 반환
        }
    }
}
