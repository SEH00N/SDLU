using System;
using System.Net.Sockets;
using System.Net;

namespace H00N.Network
{
    /// <summary>
    /// 클라와 연결하는 서버 측 연결용 클래스
    /// </summary>
    public class Listener
    {
        private Socket listenSocket = null; // 리슨소켓
        private Action<Socket> onAccepted; // 다른 소켓과 연결되었을 때 실행될 콜백

        private bool active = false; // 연결 여부

        public Listener(IPEndPoint endPoint) // 생성자
        {
            listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // 리슨소켓 생성
            listenSocket.Bind(endPoint); // 리슨소켓 바인딩

            // 전역변수 초기화
            onAccepted = null;
            active = false;
        }

        public bool Listen(int backlog = 10) // 리스닝시키는 함수
        {
            if (active) // 예외처리
            {
                Console.WriteLine("Listen Socket is Already Listening");
                return false;
            }

            listenSocket.Listen(backlog); // 리스닝
            return true; // 성공 반환
        }

        public void StartAccept(Action<Socket> onAccepted) // 연결 루프를 시작하는 함수
        {
            SocketAsyncEventArgs acceptArgs = new SocketAsyncEventArgs(); // 인자 생성
            acceptArgs.Completed += OnAcceptCompleted; // 연결 후 실행될 콜백 구독
            this.onAccepted = onAccepted; // 연결 후 실행될 콜백 할당

            Accept(acceptArgs); // 연결 시작
        }

        private void Accept(SocketAsyncEventArgs args) // 실질적인 연결이 이루어지는 함수
        {
            args.AcceptSocket = null; // 연결 소켓 밀어주기 (args를 재활용하기 때문)

            bool pending = listenSocket.AcceptAsync(args); // 비동기 연결
            if (pending == false) // 만약 동기적으로 처리되었다면
                OnAcceptCompleted(null, args); // 직업 콜백 호출
        }

        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs args) // 연결 후 실행될 콜백
        {
            if (args.SocketError == SocketError.Success) // 성공적으로 작업이 처리되었다면
            {
                Socket clientSocket = args.AcceptSocket;
                onAccepted?.Invoke(clientSocket); // 콜백 호출
            }

            Accept(args); // 다시 연결 시작
        }
    }
}
