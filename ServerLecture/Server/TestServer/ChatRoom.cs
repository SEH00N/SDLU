using H00N.Network;
using Microsoft.VisualBasic;
using Packets;
using System;
using System.Net;

namespace TestServer
{
    public class ChatRoom
    {
        private List<ClientSession> sessions = new List<ClientSession>();

        public void Broadcast(Packet packet)
        {
            ArraySegment<byte> buffer = packet.Serialize();
            foreach (ClientSession session in sessions)
                session.Send(buffer);
        }

        public void Enter(ClientSession session, EndPoint endPoint)
        {
            sessions.Add(session);
            session.Room = this;

            S_EnterPacket enterPacket = new S_EnterPacket();
            enterPacket.sender = endPoint.ToString();

            //Broadcast(enterPacket);
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
