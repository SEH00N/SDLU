using System;
using System.Net;
using System.Net.Sockets;

namespace H00N.Network
{
    /// <summary>
    /// 서버와 연결하는 클라 측 연결용 클래스
    /// </summary>
    public class Connector
    {
        private Socket socket; // 서버 소켓과 통신할 소켓
        private Session session; // 현재 커넥터가 연결중인 세션
        private bool onConnecting = false; // 연결 여부

        public Connector(IPEndPoint endPoint, Session session) // 생성자
        {
            this.session = session; // 연결 후 오픈할 세션 할당
            socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // 소켓 생성

            onConnecting = false; // 연결 상태 초기화
        }

        public void StartConnect(IPEndPoint endPoint) // 연결 시작 함수
        {
            if (onConnecting) // 예외처리
                return;

            onConnecting = true; // 예외처리

            SocketAsyncEventArgs args = new SocketAsyncEventArgs(); // 비동기 인자 생성
            args.RemoteEndPoint = endPoint; // 종단점 할당
            args.Completed += OnConnectCompleted; // Connect callback 할당

            Connect(args); // 연결 시도
        }

        private void Connect(SocketAsyncEventArgs args) // 실질적인 연결이 이루어지는 함수
        {
            bool pending = socket.ConnectAsync(args); // 비동기 연결
            if (pending == false) // 동기적으로 처리되었다면
                OnConnectCompleted(null, args); // 직접 callback 실행
        }

        private void OnConnectCompleted(object sender, SocketAsyncEventArgs args) // 연결된 후 실행되는 콜백 함수
        {
            if(args.SocketError == SocketError.Success) // 성공적으로 작업이 처리되었다면
            {
                Socket socket = args.ConnectSocket; // 연결된 소켓 (this.socket이랑 같음)
                session.Open(socket); // 세션 시작
                session.OnConnected(socket.RemoteEndPoint); // 연결되었음을 알림
            }
        }
    }
}
