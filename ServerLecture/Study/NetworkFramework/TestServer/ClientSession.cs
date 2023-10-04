using H00N.Network;
using System.Net;

namespace TestServer
{
    public class ClientSession : Session
    {
        public ChatRoom Room;
        public EndPoint EndPoint;

        public override void OnConnected(EndPoint endPoint)
        {
            EndPoint = endPoint;
            Program.Room.Push(() => Program.Room.Enter(this, endPoint));

            Console.WriteLine($"[Session] Client Connected : {endPoint}");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] Client Disconnected : {endPoint}");
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
