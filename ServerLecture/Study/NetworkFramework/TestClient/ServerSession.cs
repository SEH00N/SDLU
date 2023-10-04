using H00N.Network;
using System.Net;
using System.Security.Principal;

namespace TestClient
{
    public class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] Connected with Server");
            Program.Chatting();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] Disconnected with Server");
        }

        public override void OnPacketReceive(ArraySegment<byte> buffer)
        {
            Console.WriteLine($"[Session] {buffer.Count} of Data Received");
            PacketManager.Instance.HandlePacket(this, buffer);
        }

        public override void OnSent(int length)
        {
            Console.WriteLine($"[Session] {length} of Data Sent");
        }
    }
}
