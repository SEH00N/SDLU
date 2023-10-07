using System;
using H00N.Network;

namespace Packets
{
    public class S_LogInPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_LogInPacket;

        public ushort playerID;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);
            process += PacketUtility.ReadUShortData(buffer, process, out playerID);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.playerID, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
