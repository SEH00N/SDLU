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

            S_PlayerListPacket playerListPacket = new S_PlayerListPacket();
            playerListPacket.playerList = Program.Room.Players.Select(x => x.ID).ToList();

            Program.Room.Enter(clientSession, Program.PlayerCount++);

            session.Send(playerListPacket.Serialize());
        }

        public static void C_MovePacket(Session session, Packet packet)
        {
            C_MovePacket movePacket = packet as C_MovePacket;

            S_MovePacket s_movePacket = new S_MovePacket();
            s_movePacket.playerID = movePacket.playerID;
            s_movePacket.x = movePacket.x;
            s_movePacket.y = movePacket.y;
            s_movePacket.z = movePacket.z;

            Program.Room.Broadcast(packet, session);
        }
    }
}
