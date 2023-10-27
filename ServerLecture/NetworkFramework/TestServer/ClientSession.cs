using H00N.Network;
using System;
using System.Net;

namespace TestServer
{
    public class ClientSession : Session
    {
        public ChatRoom Room; // 채팅이 이루어지는 방
        public EndPoint EndPoint; // 클라이언트의 종단점

        public override void OnConnected(EndPoint endPoint) // 연결되었을 때
        {
            EndPoint = endPoint; // 클라이언트의 종단점
            Program.Room.Push(() => Program.Room.Enter(this, endPoint)); // Room에 클라이언트가 접속했음을 알림

            Console.WriteLine($"[Session] Client Connected : {endPoint}"); // 접속 로그
        }

        public override void OnDisconnected(EndPoint endPoint) // 연결 해제되었을 때
        {
            Console.WriteLine($"[Session] Client Disconnected : {endPoint}"); // 퇴장 로그
        }

        public override void OnPacketReceived(ArraySegment<byte> buffer) // 패킷이 수신되었을 때
        {
            Console.WriteLine($"[Session] {buffer.Count} of Data Received"); // 패킷 수신 로그
            PacketManager.Instance.HandlePacket(this, buffer); // 패킷 핸들링
        }

        public override void OnSent(int length) // 데이터가 전송되었을 때
        {
            Console.WriteLine($"[Session] {length} of Data Sent"); // 패킷 송신 로그
        }
    }
}
