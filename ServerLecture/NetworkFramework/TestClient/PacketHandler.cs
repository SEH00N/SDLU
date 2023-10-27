using H00N.Network;
using Packets;
using System;

namespace TestClient
{
    public class PacketHandler
    {
        public static void S_ChatPacket(Session session, Packet packet) // Server로부터 ChatPacket을 받았을 때
        {
            S_ChatPacket chatPacket = packet as S_ChatPacket; // packet 캐스팅
            Console.WriteLine($"{chatPacket.sender} : {chatPacket.message}"); // 출력
        }

        public static void S_EnterPacket(Session session, Packet packet) // Server로부터 EnterPacket을 받았을 때
        {
            S_EnterPacket enterPacket = packet as S_EnterPacket; // packet 캐스팅
            Console.WriteLine($"{enterPacket.sender} Connected"); // 출력
        }

        public static void S_LeavePacket(Session session, Packet packet) // Server로부터 LeavePacket을 받았을 때
        {
            S_LeavePacket leavePacket = packet as S_LeavePacket; // packet 캐스팅
            Console.WriteLine($"{leavePacket.sender} Disconnected"); // 출력
        }
    }
}
