using H00N.Network;
using PacketUtility;
using System;

namespace TestClient
{
    public class PacketManager
    {
        private static PacketManager instance = null;
        public static PacketManager Instance {
            get {
                if (instance == null)
                    instance = new PacketManager();

                return instance;
            }
        }

        private Dictionary<PacketID, Func<ArraySegment<byte>, Packet>> packetFactory = new Dictionary<PacketID, Func<ArraySegment<byte>, Packet>>();
        private Dictionary<PacketID, Action<Packet>> packetHandler = new Dictionary<PacketID, Action<Packet>>();

        private PacketManager()
        {
            packetFactory.Clear();
            packetHandler.Clear();

            RegisterHandler();
        }

        private void RegisterHandler()
        {
            packetFactory.Add(PacketID.S_ChatPacket, PacketUtility.CreatePacket<C_MessagePacket>);
            packetHandler.Add(PacketID.S_ChatPacket, PacketHandler.C_MovePacketHandler);
        }
    }
}
