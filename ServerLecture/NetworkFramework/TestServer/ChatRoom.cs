using H00N.Network;
using Packets;
using System;
using System.Collections.Generic;
using System.Net;

namespace TestServer
{
    public class ChatRoom
    {
        private List<ClientSession> sessions = new List<ClientSession>();
        private Queue<ArraySegment<byte>> packetQueue = new Queue<ArraySegment<byte>>();

        private JobQueue jobQueue = new JobQueue();

        public void Push(Action item) => jobQueue.Push(item);

        public void Flush()
        {
            while(packetQueue.Count > 0)
            {
                ArraySegment<byte> buffer = packetQueue.Dequeue();
                foreach (ClientSession session in sessions)
                    session.Send(buffer);
            }
        }

        public void Broadcast(Packet packet)
        {
            packetQueue.Enqueue(packet.Serialize());
        }

        public void Enter(ClientSession session, EndPoint endPoint)
        {
            sessions.Add(session);
            session.Room = this;

            S_EnterPacket enterPacket = new S_EnterPacket();
            enterPacket.sender = endPoint.ToString();

            Broadcast(enterPacket);
        }

        public void Leave(ClientSession session)
        {
            sessions.Remove(session);
            session.Room = null;

            S_LeavePacket leavePacket = new S_LeavePacket();

            Broadcast(leavePacket);
        }
    }
}
