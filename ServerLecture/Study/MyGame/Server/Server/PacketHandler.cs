using H00N.Network;
using Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class PacketHandler
    {
        public static void C_RoomEnterRequestPacket(Session session, Packet packet)
        {
            ClientSession clientSession = session as ClientSession;

            ushort playerID = Program.PlayerCount++;
            clientSession.ID = playerID;

            S_RoomEnterResponsePacket resPacket = new S_RoomEnterResponsePacket();
            resPacket.playerID = playerID;

            session.Send(resPacket.Serialize());
        }

        public static void C_MovePacket(Session session, Packet packet)
        {
            C_MovePacket movePacket = packet as C_MovePacket;

            S_MovePacket s_movePacket = new S_MovePacket();
            s_movePacket.playerID = movePacket.playerID;
            s_movePacket.x = movePacket.x;
            s_movePacket.y = movePacket.y;
            s_movePacket.z = movePacket.z;

            Program.Room.Push(() => Program.Room.Broadcast(s_movePacket, session as ClientSession));
        }

        public static void C_EnterRoomPacket(Session session, Packet packet)
        {
            ClientSession clientSession = session as ClientSession;

            S_PlayerListPacket playerListPacket = new S_PlayerListPacket();
            playerListPacket.playerList = Program.Room.Players.Select(x => x.ID).ToList();

            S_PlayerJoinPacket joinPacket = new S_PlayerJoinPacket();
            joinPacket.playerID = clientSession.ID;

            Program.Room.Push(() => Program.Room.Enter(clientSession));

            Program.Room.Push(() => Program.Room.Broadcast(joinPacket, clientSession));
            session.Send(playerListPacket.Serialize());
        }
    }
}
