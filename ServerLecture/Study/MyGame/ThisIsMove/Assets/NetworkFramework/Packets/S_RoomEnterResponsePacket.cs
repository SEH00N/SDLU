using H00N.Network;
using System;

namespace Packets
{
    public class S_RoomEnterResponsePacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_RoomEnterResponsePacket;

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

            process += PacketUtility.AppendUShortData(ID, buffer, process);
            process += PacketUtility.AppendUShortData(playerID, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
