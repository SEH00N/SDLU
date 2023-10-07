using H00N.Network;
using System;
using System.Net;

namespace Server
{
    public class ClientSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"{endPoint} : 클라이언트 접속!");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"{endPoint} : 클라이언트 접속해제!");
        }

        public override void OnPacketReceived(ArraySegment<byte> buffer)
        {
            Console.WriteLine($"{buffer.Count} : 데이터 받음!");
            PacketManager.Instance.HandlePacket(this, PacketManager.Instance.CreatePacket(buffer));
        }

        public override void OnSent(int length)
        {
            Console.WriteLine($"{length} : 데이터 보냄!");
        }
    }
}
