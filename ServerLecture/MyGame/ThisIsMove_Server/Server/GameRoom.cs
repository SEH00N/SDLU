using H00N.Network;

namespace Server
{
    public class GameRoom
    {
        public List<ushort> players = new List<ushort>();

        private JobQueue jobQueue = new JobQueue();

        private Queue<Tuple<Packet, ushort>> packetQueue = new Queue<Tuple<Packet, ushort>>();

        public void AddJob(Action action) => jobQueue.Push(action);

        public void FlushPacketQueue()
        {
            while(packetQueue.Count > 0)
            {
                Tuple<Packet, ushort> tuple = packetQueue.Dequeue();
                Packet packet = tuple.Item1;
                ushort except = tuple.Item2;

                ArraySegment<byte> buffer = packet.Serialize();
                for (ushort i = 0; i < players.Count; i++)
                {
                    ushort playerID = players[i];
                    if (playerID == except)
                        continue;

                    Session session = Program.players[playerID].session;
                    session.Send(buffer);
                }
            }
        }


        public void AddPlayer(ushort playerID)
        {
            if (players.Contains(playerID))
                return;

            players.Add(playerID);
        }

        public Player GetPlayer(ushort id)
        {
            if (players.Contains(id) == false)
                return null;

            if (Program.players.ContainsKey(id) == false)
                return null;

            return Program.players[id];
        }

        public void Broadcast(Packet packet, ushort except)
        {
            packetQueue.Enqueue(new Tuple<Packet, ushort>(packet, except));
        }
    }
}
