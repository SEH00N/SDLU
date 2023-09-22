using H00N.Network;
using System.Net;

namespace TestClient
{
    public class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] Connected with Server");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] Disconnected with Server");
        }

        public override void OnPacketReceived(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.HandlePacket(buffer);
        }

        public override void OnSent(int length)
        {
            Console.WriteLine($"[Session] {length} of Data Sent");
        }
    }
}
