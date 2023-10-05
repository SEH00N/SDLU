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

        private JobQueue jobQueue = new JobQueue();

        public void Enter(ClientSession session)
        {
            players.Add(session);
            session.Room = this;
        }

        public void Push(Action action)
        {
            jobQueue.Push(action);
        }

        public void Broadcast(Packet packet, ClientSession except)
        {
            players.ForEach(player => {
                if (player.ID == except.ID)
                    return;

                player.Send(packet.Serialize());
            });
        }
    }
}
