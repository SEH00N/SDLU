using System;

namespace H00N.Network
{
    public abstract class Packet
    {
        public abstract ushort ID { get; }

        public abstract void Deserialize(ArraySegment<byte> buffer);
        public abstract ArraySegment<byte> Serialize();
    }
}
