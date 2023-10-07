using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace H00N.Network
{
    public abstract class Session
    {
        public const int HeaderSize = sizeof(ushort); // 패킷의 길이가 들어갈 헤더 사이즈

        private Socket socket; // 통신할 소켓과 연결된 소켓

        private int active = 0; // 연결 여부
        public int Active => active; // 연결 여부

        private SharedBuffer receiveBuffer = new SharedBuffer(65536); // 수신용 버퍼
        private Queue<ArraySegment<byte>> sendQueue = new Queue<ArraySegment<byte>>(); // 보낼 패킷을 저장할 Queue
        private List<ArraySegment<byte>> pendingList = new List<ArraySegment<byte>>(); // 세팅할 버퍼 컨테이너

        private SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs(); // 비동기 송신용 인자

        private object locker = new object(); // 수신 데이터 락커

        public abstract void OnConnected(EndPoint endPoint); // 연결되었을 때 호출되는 함수
        public abstract void OnDisconnected(EndPoint endPoint); // 연결 해제되었을 때 호출되는 함수
        public abstract void OnPacketReceived(ArraySegment<byte> buffer); // 패킷이 읽어들여졌을 때 호출되는 함수
        public abstract void OnSent(int length); // 데이터를 전송헀을 때 호출되는 함수

        public void Open(Socket socket) // 세션을 여는 함수
        {
            if (Interlocked.Exchange(ref active, 1) == 1) // active를 1로 변환, 이미 active가 1이라면 return
                return;

            this.socket = socket; // 소켓 할당

            SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs(); // 비동기 수신용 인자
            receiveArgs.Completed += OnReceiveCompleted; // 비동기 수신 콜백 구독
            sendArgs.Completed += OnSendCompleted; // 비동기 송신 콜백 구독

            Receive(receiveArgs); // 데이터 수신 루프 시작
        }

        public void Close() // 연결 해제
        {
            if (Interlocked.Exchange(ref active, 0) == 0) // active를 0로 변환, 이미 active가 0이라면 return
                return;

            EndPoint endPoint = socket.RemoteEndPoint; // 소켓의 종단점 캐싱

            socket.Shutdown(SocketShutdown.Both); // 소켓 송수신 차단
            socket.Close(); // 소켓 닫기

            Release(); // 메모리 비우기

            OnDisconnected(endPoint); // 연결 해제 알림
        }

        public void Send(ArraySegment<byte> sendBuffer) // 패킷 송신 요청 함수
        {
            lock(locker)
            {
                sendQueue.Enqueue(sendBuffer); // sendQueue에 패킷 예약
                if (pendingList.Count == 0) // pendingList가 0이라면 (현재 전송중인 데이터가 없다면)
                    FlushSendQueue(); // sendQueue Flush
            }
        }

        private void Release() // 메모리 비우는 함수
        {
            lock (locker)
            {
                sendQueue.Clear(); // sendQueue 비우기
                pendingList.Clear(); // pendingList 비우기
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
