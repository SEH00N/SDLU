using H00N.Network;
using System;
using System.Net;

namespace TestClient
{
    public class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint) // 연결되었을 때
        {
            //Console.WriteLine($"[Session] Connected with Server"); // 접속 로그
            Program.Chatting(); // 채팅 시작
        }

        public override void OnDisconnected(EndPoint endPoint) // 연결 해제되었을 때
        {
            //Console.WriteLine($"[Session] Disconnected with Server"); // 퇴장 로그
        }

        public override void OnPacketReceived(ArraySegment<byte> buffer) // 패킷이 수신되었을 때
        {
            //Console.WriteLine($"[Session] {buffer.Count} of Data Received"); // 패킷 수신 로그
            PacketManager.Instance.HandlePacket(this, buffer); // 패킷 핸들링
        }

        public override void OnSent(int length) // 데이터가 전송되었을 때
        {
            //Console.WriteLine($"[Session] {length} of Data Sent"); // 패킷 송신 로그
        }
    }
}
