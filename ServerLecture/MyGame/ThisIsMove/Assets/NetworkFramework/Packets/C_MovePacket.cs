using H00N.Network;
using System;

namespace Packets
{
    public class C_MovePacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_MovePacket;

        public PlayerPacket playerData;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadDataPacket<PlayerPacket>(buffer, process, out playerData);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendDataPacket<PlayerPacket>(this.playerData, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
