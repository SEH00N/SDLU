using System;
using H00N.Network;

namespace Packets
{
    public class S_LeavePacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_LeavePacket; // 패킷 아이디

        public string sender; // 나간 사람

        public override void Deserialize(ArraySegment<byte> buffer) // 역직렬화
        {
            ushort process = 0; // 처리한 길이

            process += sizeof(ushort); // 패킷 사이즈
            process += sizeof(ushort); // 패킷 아이디

            process += PacketUtility.ReadStringData(buffer, process, out this.sender); // sender 삽입
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024); // 1024만큼 버퍼 발급

            ushort process = 0; // 처리한 길이
            process += sizeof(ushort); // 패킷 사이즈 넣을 공간 확보

            process += PacketUtility.AppendUShortData(this.ID, buffer, process); // ID 삽입
            process += PacketUtility.AppendStringData(this.sender, buffer, process); // sender 삽입
            PacketUtility.AppendUShortData(process, buffer, 0); // 아까 확보해둔 공간에 전체 패킷의 길이 할당

            return UniqueBuffer.Close(process); // 버퍼 반환 (사용한 만큼만 재발급)
        }
    }
}
