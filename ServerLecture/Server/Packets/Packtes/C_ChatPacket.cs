using H00N.Network;

namespace Packets
{
    public class C_ChatPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_ChatPacket;

        public string message;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort); // 패킷 사이즈
            process += sizeof(ushort); // 패킷 아이디

            process += PacketUtility.TranslateString(buffer, process, out this.message);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);

            ushort process = 0;
            process += sizeof(ushort); // 패킷의 사이즈를 넣을 공간 미리 확보

            process += PacketUtility.AppendUShortData(this.ID, buffer, process); // ID 할당
            process += PacketUtility.AppendStringData(this.message, buffer, process); // message 할당
            PacketUtility.AppendUShortData(process, buffer, 0); // 아까 확보해둔 그 공간에 사이즈 할당

            return UniqueBuffer.Close(process);
        }
    }
}
