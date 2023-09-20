using H00N.Network;

namespace PacketUtility
{
    public class C_ChatPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_ChatPacket;

        public override void Deserialize(ArraySegment<byte> buffer)
        {

        }

        public override ArraySegment<byte> Serialize()
        {

        }
    }
}
