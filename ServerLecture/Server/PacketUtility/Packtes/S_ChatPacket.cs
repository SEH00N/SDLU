using H00N.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketUtility
{
    public class S_ChatPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_ChatPacket;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            throw new NotImplementedException();
        }

        public override ArraySegment<byte> Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
