using H00N.Network;
using Packets;

namespace TestServer
{
    public class PacketManager
    {
        private static PacketManager instance;
        public static PacketManager Instance {
            get {
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
            packetFactories.Add((ushort)PacketID.C_ChatPacket, PacketUtility.CreatePacket<C_ChatPacket>);
            packetHandlers.Add((ushort)PacketID.C_ChatPacket, PacketHandler.C_ChatPacket);
        }

        public void HandlePacket(Session session, ArraySegment<byte> buffer)
        {
            ushort packetID = PacketUtility.ReadPacketID(buffer);

            if(packetFactories.ContainsKey(packetID))
            {
                Packet packet = packetFactories[packetID]?.Invoke(buffer);
                if (packetHandlers.ContainsKey(packetID))
                    packetHandlers[packetID]?.Invoke(session, packet);
            }
        }
    }
}
