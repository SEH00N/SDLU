using H00N.Network;
using Packets;
using System;

namespace TestClient
{
    public class PacketManager
    {
        private static PacketManager instance;
        public static PacketManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new PacketManager();
                return instance;
            }
        }

        private Dictionary<ushort, Func<ArraySegment<byte>, Packet>> packetFactories = new Dictionary<ushort, Func<ArraySegment<byte>, Packet>>();
        private Dictionary<ushort, Action<Session, Packet>> packetHandlers = new Dictionary<ushort, Action<Session, Packet>>();

        public PacketManager()
        {
            packetFactories.Clear();
            packetHandlers.Clear();

            RegisterHandler();
        }

        private void RegisterHandler()
        {
            packetFactories.Add((ushort)PacketID.S_ChatPacket, PacketUtility.CreatePacket<S_ChatPacket>);
            packetHandlers.Add((ushort)PacketID.S_ChatPacket, PacketHandler.S_ChatPacket);

            packetFactories.Add((ushort)PacketID.S_EnterPacket, PacketUtility.CreatePacket<S_EnterPacket>);
            packetHandlers.Add((ushort)PacketID.S_EnterPacket, PacketHandler.S_EnterPacket);

            packetFactories.Add((ushort)PacketID.S_LeavePacket, PacketUtility.CreatePacket<S_LeavePacket>);
            packetHandlers.Add((ushort)PacketID.S_LeavePacket, PacketHandler.S_LeavePacket);
        }

        public void HandlePacket(Session session, ArraySegment<byte> buffer)
        {
            ushort packetID = PacketUtility.ReadPacketID(buffer);

            if (packetFactories.ContainsKey(packetID))
            {
                Packet packet = packetFactories[packetID]?.Invoke(buffer);
                if (packetHandlers.ContainsKey(packetID))
                    packetHandlers[packetID]?.Invoke(session, packet);
            }
        }
    }
}
