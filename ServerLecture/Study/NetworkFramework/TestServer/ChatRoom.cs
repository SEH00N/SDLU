using H00N.Network;
using Packets;
using System.Net;

namespace TestServer
{
    public class ChatRoom
    {
        private List<ClientSession> sessions = new List<ClientSession>();

        private JobQueue jobQueue = new JobQueue();

        public void Push(Action job) => jobQueue.Push(job);

        public void Broadcast(Packet packet)
        {
            ArraySegment<byte> buffer = packet.Serialize();
            sessions.ForEach(session => session.Send(buffer));
        }

        public void Enter(ClientSession session, EndPoint endPoint)
        {
            sessions.Add(session);
            session.Room = this;

            S_EnterPacket enterPacket = new S_EnterPacket();
            enterPacket.sender = endPoint.ToString();

            Broadcast(enterPacket);
        }
    }
}
