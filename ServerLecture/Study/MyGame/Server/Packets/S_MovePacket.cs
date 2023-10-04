using H00N.Network;

namespace Packets
{
    public class S_MovePacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_MovePacket;

        public ushort playerID;
        public ushort x;
        public ushort y;
        public ushort z;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out this.playerID);
            process += PacketUtility.ReadUShortData(buffer, process, out this.x);
            process += PacketUtility.ReadUShortData(buffer, process, out this.y);
            process += PacketUtility.ReadUShortData(buffer, process, out this.z);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);

            ushort process = 0;

            process += sizeof(ushort);

            process += PacketUtility.AppendUShortData(ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.playerID, buffer, process);
            process += PacketUtility.AppendUShortData(this.x, buffer, process);
            process += PacketUtility.AppendUShortData(this.y, buffer, process);
            process += PacketUtility.AppendUShortData(this.z, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
