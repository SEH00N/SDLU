using H00N.Network;

namespace Packets
{
    public class S_EnterPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_EnterPacket;

        public string sender;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.TranslateString(buffer, process, out this.sender);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);

            ushort process = 0;
            process += sizeof(ushort);

            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendStringData(this.sender, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
