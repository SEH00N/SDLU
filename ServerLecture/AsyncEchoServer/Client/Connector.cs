using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class Connector
    {
        private Socket serverSocket; // 서버 소켓과 통신할 소켓
        private Action<string> onMessageReceivedEvent; // 메세지가 받아졌을 때 실행시킬 이벤트

        private SocketAsyncEventArgs sendArgs; // 메세지를 전송할 때 사용될 인자

        public Connector(IPEndPoint endPoint, Action<string> onMessageReceived)
        {
            serverSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // 소켓 생성

            this.onMessageReceivedEvent = onMessageReceived; // onMessageReceived 구독
            sendArgs = new SocketAsyncEventArgs(); // 비동기 인자 생성
        }

        public void Connect(IPEndPoint endPoint)
        {
            serverSocket.Connect(endPoint); // 서버에 연결

            Console.WriteLine("서버와 연결되었습니다.");
            StartReceive(); // 수신 시작
        }

        public void Disconnect()
        {
            // 연결 끊기
            serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Close();

            Console.WriteLine("서버와 연결이 끊겼습니다.");
        }

        public void Send(string message)
        {
            byte[] sendBytes = Encoding.UTF8.GetBytes(message); // 메세지 변환
            sendArgs.SetBuffer(sendBytes); // 보낼 메세지 버퍼 세팅

            serverSocket.SendAsync(sendArgs); // 메세지 전송
        }

        private void StartReceive()
        {
            SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs(); // 메세지 수신할 때 사용할 인자 생성
            receiveArgs.Completed += OnReceiveCompleted; // Receive가 끝났을 때 실행되는 Completed 콜백에 OnReceiveCompleted 구독

            Receive(receiveArgs); // 실질적인 메세지 수신
        }

        private void Receive(SocketAsyncEventArgs args)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024], 0, 1024); // 메세지를 담을 버퍼 생성
            args.SetBuffer(buffer.Array, buffer.Offset, buffer.Count); // 메세지를 담을 버퍼 세팅

            bool pending = serverSocket.ReceiveAsync(args); // 비동기로 메세지를 받는 함수. 즉시 처리되었다면 false, 작업이 보류중이면 true 반환
            if (pending == false) // 반약 동기적으로 처리되었다면
                OnReceiveCompleted(null, args); // 즉시 처리가 되었다면 Completed 콜벡이 실행되지 않기 때문에 직접 핸들링
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success && args.BytesTransferred > 0) // 만약 받은 메세지의 길이가 0보다 크고 성공적으로 통신이 되었다면
            {
                string receivedMessage = Encoding.UTF8.GetString(args.Buffer, 0, args.BytesTransferred); // 받은 메세지 변환
                onMessageReceivedEvent?.Invoke(receivedMessage); // onMessageReceivedEvent 발행

                Receive(args); // 다시 메세지 수신 시작
            }
            else
                Disconnect(); // 통신에 실패했다면 서버와의 연결 끊기
        }
    }
}
