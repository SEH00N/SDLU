using H00N.Network;
using System;

namespace Packets
{
    public class C_RoomEnterRequestPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_RoomEnterRequestPacket;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);
            
            // 
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);

            ushort process = 0;

            process += sizeof(ushort);

            process += PacketUtility.AppendUShortData(ID, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
