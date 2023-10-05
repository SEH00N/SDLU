using H00N.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packets
{
    public class C_EnterRoomPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_EnterRoomPacket;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);
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
