using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Listener
    {
        private Socket listenSocket = null; // 리슨소켓
        private List<Socket> clientSockets = new List<Socket>(); // 리슨소켓과 연결 되어있는 소켓 리스트

        private Action<Socket, string> onMessageReceivedEvent; // 메세지가 받아졌을 때 실행시킬 이벤트
        private SocketAsyncEventArgs sendArgs; // 데이터를 보낼 때 사용하는 인자

        private object clientSocketsLocker = new object(); // 소켓 리스트 자물쇠
        private object handlerLocker = new object(); // 핸들러 자물쇠

        public Listener(IPEndPoint endPoint, int backlog, Action<Socket, string> onMessageReceived)
        {
            // 소켓 생성 & 바인딩
            listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(endPoint);
            listenSocket.Listen(backlog);

            onMessageReceivedEvent += onMessageReceived; // 콜백 구독
            sendArgs = new SocketAsyncEventArgs(); // 인자 객체 생성
        }

        public void Broadcast(string message)
        {
            lock (clientSocketsLocker) // clientSockets 라킹
            {
                byte[] sendBytes = Encoding.UTF8.GetBytes(message); // 메세지 변환
                sendArgs.SetBuffer(sendBytes); // 보낼 메세지 버퍼 세팅

                clientSockets.ForEach(socket => socket.SendAsync(sendArgs)); // 모든 클라이언트들에게 메세지 전송
            }
        }

        #region Accept

        public void StartAccept()
        {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs(); // 소켓 비동기 작업에 사용되는 이벤트 인자
            args.Completed += OnAcceptCompleted; // Completed 콜백에 OnAcceptCompleted 구독

            Accept(args); // 실제로 받는 소켓 작업은 Accept 메소드에서 처리
        }

        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if(args.SocketError == SocketError.Success) // 소켓이 성공적으로 받아졌다면
            {
                lock(clientSocketsLocker) // clientSockets 라킹
                {
                    Socket clientSocket = args.AcceptSocket;
                    StartReceive(clientSocket); // 메세지 수신 시작
                    clientSockets.Add(clientSocket); // 소켓 리스트에 추가

                    // 디버깅
                    IPEndPoint clientSocketEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;
                    Console.WriteLine($"클라이언트가 접속하였습니다. [{clientSocketEndPoint.Address}]");
                }
            }
            else // 소켓 받아들이기에 실패했다면
                Console.WriteLine(args.SocketError); // 소켓에러 출력

            Accept(args); // 다시 받기 시작
        }

        private void Accept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null; // 이벤트 매개변수를 재사용하기 때문에 소켓을 받는 데에 사용되는 AcceptSocket 필드 초기화

            bool pending = listenSocket.AcceptAsync(args); // 비동기로 소켓을 받는 함수. 즉시 처리되었다면 false, 작업이 보류중이면 true 반환
            if (pending == false) // 만약 즉시 처리가 되었다면
                OnAcceptCompleted(null, args); // 즉시 처리가 되었다면 Completed 콜백이 실행되지 않기 때문에 직접 핸들링
        }

        public void Kick(Socket socket)
        {
            try
            {
                lock (clientSocketsLocker) // clientSockets 라킹
                {
                    // 디버깅
                    IPEndPoint clientSocketEndPoint = socket.RemoteEndPoint as IPEndPoint;

                    // 소켓 내보내기
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();

                    clientSockets.Remove(socket); // clientSockets에서 내보낸 소켓 지우기

                    Console.WriteLine($"클라이언트가 접속 해제하였습니다. [{clientSocketEndPoint.Address}]");
                }
            }
            catch { }
        }

        #endregion

        #region Receive

        private void StartReceive(Socket socket)
        {
            SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs(); // 메세지를 받을 때 사용하는 인자
            receiveArgs.Completed += OnReceiveCompleted; // Receive가 끝났을 때 실행되는 Completed 콜백에 OnReceiveCompleted 구독
            receiveArgs.UserToken = socket; // 토큰으로 클라이언트 소켓 넘기기

            Receive(socket, receiveArgs); // 실질적인 메세지 수신
        }

        private void Receive(Socket socket, SocketAsyncEventArgs args)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024], 0, 1024); // 메세지를 받을 버퍼
            args.SetBuffer(buffer.Array, buffer.Offset, buffer.Count); // 인자에 버퍼 세팅

            bool pending = socket.ReceiveAsync(args); // 비동기로 메세지를 받는 함수. 즉시 처리되었다면 false, 작업이 보류중이면 true 반환
            if (pending == false) // 반약 동기적으로 처리되었다면
                OnReceiveCompleted(null, args); // 즉시 처리가 되었다면 Completed 콜벡이 실행되지 않기 때문에 직접 핸들링
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs args)
        {
            Socket socket = args.UserToken as Socket; // 토큰으로 넘어온 소켓 받기
            if (args.SocketError == SocketError.Success && args.BytesTransferred > 0) // 만약 받은 메세지의 길이가 0보다 크고 성공적으로 통신이 되었다면
            {
                string receivedMessage = Encoding.UTF8.GetString(args.Buffer, 0, args.BytesTransferred); // 받은 메세지 변환
                lock(handlerLocker) // onMessageReceivedEvent 라킹
                    onMessageReceivedEvent?.Invoke(socket, receivedMessage); // onMessageReceivedEvent 발행

                Receive(socket, args); // 다시 메세지 수신 시작
            }
            else
                Kick(socket); // 통신에 실패했다면 소켓 내보내기
        }

        #endregion
    }
}
