using H00N.Network;
using System;
using System.Net;
using System.Net.Sockets;

namespace TestServer
{
    internal class Program
    {
        public static Listener Listener; // 리스너
        public static ChatRoom Room; // 채팅방

        public static JobQueue JobQueue; // JobQueue


        static void Main(string[] args)
        {
            Room = new ChatRoom(); // 채팅방 생성
            JobQueue = new JobQueue(); // 잡큐 생성

            // 종단점 생성
            //string host = Dns.GetHostName();
            //IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddress = IPAddress.Parse("172.30.1.52");
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            Listener = new Listener(endPoint); // 리스너 생성
            if (Listener.Listen(10)) // 리스닝이 성공하면
                Listener.StartAccept(OnAccepted); // 클라이언트 연결 시작

            FlushLoop(1000 / 20); // 플러시 루프
        }

        private static void FlushLoop(int delay) // 채팅방 패킷을 플러시하는 함수
        {
            int last = Environment.TickCount; // 마지막 플러시 시간

            while (true)
            {
                int now = Environment.TickCount; // 현재 시간
                if (now - last > delay) // 현재 시간이 마지막 플러시 시간과 delay 이상 차이나면
                {
                    JobQueue.Push(() => Room.Flush()); // JobQueue에 Flush 예약
                    last = now; // 마지막 플러시 시간 업데이트
                }
            }
        }

        private static void OnAccepted(Socket socket) // 클라이언트와 연결되었을 때
        {
            ClientSession session = new ClientSession(); // 세션 생성
            session.Open(socket); // 세션 오픈
            session.OnConnected(socket.RemoteEndPoint); // 연결 알림
        }
    }
}