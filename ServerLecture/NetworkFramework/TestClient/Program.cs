using H00N.Network;
using Packets;
using System;
using System.Net;

namespace TestClient
{
    internal class Program
    {
        public static Connector connector; // 커넥터
        public static ServerSession serverSession; // 서버와 통신하는 세션

        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("172.30.1.52"); // 접속할 IP
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081); // 연결할 소켓의 종단점

            serverSession = new ServerSession(); // 서버와 통신할 세션 생성

            connector = new Connector(endPoint, serverSession); // 커넥터 생성
            connector.StartConnect(endPoint); // 접속 시작

            while(true) // 메인 스레드가 꺼지지 않도록
            { }
        }

        public static async void Chatting() // 채팅
        {
            while(true)
            {
                if (serverSession.Active == 0) // serverSession이 종료되면
                    break; // 루프 끝내기

                string message = Console.ReadLine(); // 메세지 작성
                C_ChatPacket chatPacket = new C_ChatPacket(); // 패킷 생성
                chatPacket.message = message; // 메세지 할당

                ArraySegment<byte> buffer = chatPacket.Serialize(); // 패킷 직렬화

                serverSession.Send(buffer); // 패킷 전송
            }
        }
    }
}