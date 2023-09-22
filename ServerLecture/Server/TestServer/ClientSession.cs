using H00N.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace TestServer
{
    public class ClientSession : Session
    {


        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] Client Connected : {endPoint}");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] Client Disconnected : {endPoint}");
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
