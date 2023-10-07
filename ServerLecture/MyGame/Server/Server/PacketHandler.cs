using H00N.Network;
using Packets;

namespace Server
{
    public class PacketHandler
    {
        public static void C_LogInPacket(Session session, Packet packet) // Client로부터 ChatPacket을 받았을 때
        {
            C_LogInPacket loginPacket = packet as C_LogInPacket;
            ClientSession clientSession = session as ClientSession;

            Player player = new Player(clientSession, Program.playerCount, loginPacket.nickname, 0, 0, 0);
            Program.players.Add(player.playerID, player);

            Program.playerCount++;

            S_LogInPacket sendPacket = new S_LogInPacket();
            sendPacket.playerID = player.playerID;

            clientSession.Send(sendPacket.Serialize());
        }

        public static void C_RoomEnterPacket(Session session, Packet packet)
        {
            C_RoomEnterPacket enterPacket = packet as C_RoomEnterPacket;

            GameRoom room = Program.room;
            room.AddJob(() => room.AddPlayer(enterPacket.playerID));

            S_RoomEnterPacket resPacket = new S_RoomEnterPacket();
            resPacket.playerList = new List<PlayerPacket>();
            room.players.ForEach(p => {
                if (p == enterPacket.playerID)
                    return;

                Console.WriteLine($"현재 플레이어 아이디 : {p}");

                Player player = Program.players[p];
                PlayerPacket playerPacket = new PlayerPacket(player.playerID, player.x, player.y, player.z);
                resPacket.playerList.Add(playerPacket);
            });
            session.Send(resPacket.Serialize());

            Player player = Program.players[enterPacket.playerID];
            S_PlayerJoinPacket broadcastPacket = new S_PlayerJoinPacket();
            broadcastPacket.playerData = new PlayerPacket(player.playerID, player.x, player.y, player.z);

            room.AddJob(() => room.Broadcast(broadcastPacket, player.playerID));
        }

        public static void C_MovePacket(Session session, Packet packet)
        {
            C_MovePacket movePacket = packet as C_MovePacket;
            GameRoom room = Program.room;

            Player player = room.GetPlayer(movePacket.playerData.playerID);
            if (player == null)
                return;

            player.x = movePacket.playerData.x;
            player.y = movePacket.playerData.y;
            player.z = movePacket.playerData.z;

            S_MovePacket resPacket = new S_MovePacket();
            resPacket.playerData = new PlayerPacket(player.playerID, player.x, player.y, player.z);

            room.Broadcast(resPacket, movePacket.playerData.playerID);
        }
    }
}
