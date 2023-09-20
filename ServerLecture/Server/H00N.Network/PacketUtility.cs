
namespace H00N.Network
{
    public class PacketUtility
    {
        public static Packet CreatePacket<T>(ArraySegment<byte> buffer) where T : Packet, new()
        {
            T packet = new T();
            packet.Deserialize(buffer);

            return packet;
        }
    }
}
