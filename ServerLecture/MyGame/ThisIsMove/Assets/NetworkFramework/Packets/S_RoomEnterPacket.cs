using System;
using System.Collections.Generic;
using H00N.Network;

namespace Packets
{
    public class S_RoomEnterPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_RoomEnterPacket;

        public List<PlayerPacket> playerList;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);
            process += PacketUtility.ReadListData<PlayerPacket>(buffer, process, out playerList);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendListData<PlayerPacket>(this.playerList, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
