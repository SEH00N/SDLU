using H00N.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameRoom
    {
        private List<ClientSession> players = new List<ClientSession>();
        public List<ClientSession> Players => players;

        public void Enter(ClientSession session, ushort id)
        {
            session.ID = id;
            players.Add(session);
            session.Room = this;
        }

        public void Broadcast(Packet packet, Session except)
        {
            players.ForEach(player => {
                if (player == except)
                    return;

                player.Send(packet.Serialize());
            });
        }
    }
}
