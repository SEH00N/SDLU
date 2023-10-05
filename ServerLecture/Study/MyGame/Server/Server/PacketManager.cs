
using H00N.Network;
using Packets;

namespace Server
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
            packetFactories.Add((ushort)PacketID.C_RoomEnterRequestPacket, PacketUtility.CreatePacket<C_RoomEnterRequestPacket>);
            packetHandlers.Add((ushort)PacketID.C_RoomEnterRequestPacket, PacketHandler.C_RoomEnterRequestPacket);

            packetFactories.Add((ushort)PacketID.C_EnterRoomPacket, PacketUtility.CreatePacket<C_EnterRoomPacket>);
            packetHandlers.Add((ushort)PacketID.C_EnterRoomPacket, PacketHandler.C_EnterRoomPacket);

            packetFactories.Add((ushort)PacketID.C_MovePacket, PacketUtility.CreatePacket<C_MovePacket>);
            packetHandlers.Add((ushort)PacketID.C_MovePacket, PacketHandler.C_MovePacket);
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
