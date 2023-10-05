using H00N.Network;
using System;
using System.Net;

namespace Server
{
    public class ClientSession : Session
    {
        public GameRoom Room;
        public ushort ID;

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"{endPoint} 들어옴");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"{endPoint} 나감");
        }

        public override void OnPacketReceive(ArraySegment<byte> buffer)
        {
            Console.WriteLine($"{buffer.Count}만큼 받음");
            PacketManager.Instance.HandlePacket(this, buffer);
        }

        public override void OnSent(int length)
        {
            Console.WriteLine($"{length}만큼 보냄");
        }
    }
}
