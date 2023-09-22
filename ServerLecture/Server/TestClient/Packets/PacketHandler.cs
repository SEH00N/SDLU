using H00N.Network;
using Packets;

namespace TestClient
{
    public class PacketHandler
    {
        public static void S_ChatPacketHandler(Packet packet)
        {
            S_ChatPacket chatPacket = packet as S_ChatPacket;
            Console.WriteLine($"{chatPacket.sender} : {chatPacket.message}");
        }
    }
}
