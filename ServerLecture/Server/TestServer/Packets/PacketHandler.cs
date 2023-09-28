using H00N.Network;
using Packets;

namespace TestServer
{
    public class PacketHandler
    {
        public static void C_ChatPacket(Session session, Packet packet)
        {
            C_ChatPacket chatPacket = packet as C_ChatPacket;
            ClientSession clientSession = session as ClientSession;
            Console.WriteLine($"{clientSession.EndPoint} : {chatPacket.message}");

            S_ChatPacket sendPacket = new S_ChatPacket();
            sendPacket.sender = clientSession.EndPoint.ToString();
            sendPacket.message = chatPacket.message;

            ChatRoom room = clientSession.Room;
            room.Push(() => room.Broadcast(sendPacket));
        }
    }
}
