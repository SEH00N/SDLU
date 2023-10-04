using H00N.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ClientSession : Session
    {
        public GameRoom Room;
        public ushort ID;

        public override void OnConnected(EndPoint endPoint)
        {
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
        }

        public override void OnPacketReceive(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.HandlePacket(this, buffer);
        }

        public override void OnSent(int length)
        {
        }
    }
}
