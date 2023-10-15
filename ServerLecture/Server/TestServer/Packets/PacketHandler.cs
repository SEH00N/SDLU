using H00N.Network;
using Packets;

namespace TestServer
{
    public class PacketHandler
    {
        public static void C_ChatPacket(Session session, Packet packet) // Client로부터 ChatPacket을 받았을 때
        {
            C_ChatPacket chatPacket = packet as C_ChatPacket; // packet 캐스팅
            ClientSession clientSession = session as ClientSession; // session 캐스팅
            //Console.WriteLine($"{clientSession.EndPoint} : {chatPacket.message}"); // message 출력

            S_ChatPacket sendPacket = new S_ChatPacket(); // Client에게 보낼 패킷 생성
            sendPacket.sender = clientSession.EndPoint.ToString(); // 보낸사람 할당
            sendPacket.message = chatPacket.message; // 메세지 할당

            ChatRoom room = clientSession.Room;
            room.Push(() => room.Broadcast(sendPacket)); // 브로드 캐스팅
        }
    }
}
