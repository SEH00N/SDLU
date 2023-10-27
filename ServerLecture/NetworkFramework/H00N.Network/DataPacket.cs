using System;

namespace H00N.Network
{
    /// <summary>
    /// 이 패킷은 실제 통신에 사용되진 않는다.<br/>
    /// 실제 통신이 이루어지는 패킷은 Packet 뿐이고<br/>
    /// 이 패킷은 직렬화 가능한 구조체를 만들기 위한 패킷이다.<br/>
    /// <br/>
    /// ex) class VectorData : DataPacket { int x; int y; }<br/>
    /// 이처럼 단지 x와 y값을 저장하기 위한 패킷<br/>
    /// 하지만 DataPacket의 추상 메소드를 구현함으로써 직렬화 및 역직렬화를 가능케 한다.
    /// </summary>
    public abstract class DataPacket
    {
        public abstract ushort Deserialize(ArraySegment<byte> buffer, int offset); // 역직렬화 (바이트 배열 -> 데이터)
        public abstract ushort Serialize(ArraySegment<byte> buffer, int offset); // 직렬화 (데이터 -> 바이트 배열)
    }
}
