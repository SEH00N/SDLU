using H00N.Network;
using System;
using System.Collections.Generic;

namespace Packets
{
    public class S_PlayerListPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_PlayerListPacket;

        public List<ushort> playerList;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out ushort listSize);
            playerList = new List<ushort>(listSize);
            for (ushort i = 0; i < listSize; i++)
            {
                process += PacketUtility.ReadUShortData(buffer, process, out ushort playerID);
                playerList[i] = playerID;
            }
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);

            ushort process = 0;

            process += sizeof(ushort);

            process += PacketUtility.AppendUShortData(ID, buffer, process);

            process += PacketUtility.AppendUShortData((ushort)playerList.Count, buffer, process);
            playerList.ForEach(p => process += PacketUtility.AppendUShortData(p, buffer, process));

            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
