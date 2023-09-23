using H00N.Network;
using Packets;

namespace TestClient
{
    public class PacketHandler
    {
        public static void S_ChatPacket(Session session, Packet packet)
        {
            S_ChatPacket chatPacket = packet as S_ChatPacket;
            Console.WriteLine($"{chatPacket.sender} : {chatPacket.message}");
        }

        public static void S_EnterPacket(Session session, Packet packet)
        {
            S_EnterPacket enterPacket = packet as S_EnterPacket;
            Console.WriteLine($"{enterPacket.sender} Connected");
        }

        public static void S_LeavePacket(Session session, Packet packet)
        {
            S_LeavePacket leavePacket = packet as S_LeavePacket;
            Console.WriteLine($"{leavePacket.sender} Disconnected");
        }
    }
}
