using System;

namespace H00N.Network
{
    public abstract class DataPacket
    {
        public abstract ushort Deserialize(ArraySegment<byte> buffer, int offset); // 역직렬화 (바이트 배열 -> 데이터)
        public abstract ushort Serialize(ArraySegment<byte> buffer, int offset); // 직렬화 (데이터 -> 바이트 배열)
    }
}
