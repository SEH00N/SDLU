using H00N.Network;
using System;

namespace Packets
{
    public class PlayerPacket : DataPacket
    {
        // 이 패킷은 단지 데이터를 담고있는 패킷
        // 이걸 직접적으로 전송하긴 않을 거임
        // 따라서 얘를 핸들링 할 필요는 없음

        public ushort playerID;
        public float x;
        public float y;
        public float z;

        public PlayerPacket()
        {

        }

        public PlayerPacket(ushort playerID, float x, float y, float z) 
        {
            this.playerID = playerID;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override ushort Deserialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;
            process += PacketUtility.ReadUShortData(buffer, offset + process, out this.playerID);
            process += PacketUtility.ReadFloatData(buffer, offset + process, out this.x);
            process += PacketUtility.ReadFloatData(buffer, offset + process, out this.y);
            process += PacketUtility.ReadFloatData(buffer, offset + process, out this.z);
            
            return process;
        }

        public override ushort Serialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;
            process += PacketUtility.AppendUShortData(this.playerID, buffer, offset + process);
            process += PacketUtility.AppendFloatData(this.x, buffer, offset + process);
            process += PacketUtility.AppendFloatData(this.y, buffer, offset + process);
            process += PacketUtility.AppendFloatData(this.z, buffer, offset + process);

            return process;
        }
    }
}
