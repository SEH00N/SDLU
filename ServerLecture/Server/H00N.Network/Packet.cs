using System;

namespace H00N.Network
{
    public abstract class Packet
    {
        public abstract ushort ID { get; } // 패킷 아이디

        public abstract void Deserialize(ArraySegment<byte> buffer); // 역직렬화 (바이트 배열 -> 데이터)
        public abstract ArraySegment<byte> Serialize(); // 직렬화 (데이터 -> 바이트 배열)
    }
}
