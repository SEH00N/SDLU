using System;
using System.Text;

namespace H00N.Network
{
    public class PacketUtility
    {
        public static Packet CreatePacket<T>(ArraySegment<byte> buffer) where T : Packet, new()
        {
            T packet = new T();
            packet.Deserialize(buffer);

            return packet;
        }

        public static ushort ReadPacketID(ArraySegment<byte> buffer)
        {
            // [패킷 사이즈] [패킷 아이디] [데이터...]
            ushort packetID = BitConverter.ToUInt16(buffer.Array, buffer.Offset + sizeof(ushort));
            return packetID;
        }

        public static ushort TranslateString(ArraySegment<byte> buffer, int offset, out string result)
        {
            ushort process = 0;

            ushort stringLength = BitConverter.ToUInt16(buffer.Array, buffer.Offset + offset + process);
            process += sizeof(ushort);

            result = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + offset + process, stringLength);
            process += stringLength;

            return process;
        }

        public static ushort AppendIntData(int data, byte[] buffer, int offset)
        {
            ushort length = sizeof(int);
            Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer, offset, length);

            return length;
        }

        public static ushort AppendUShortData(ushort data, byte[] buffer, int offset)
        {
            ushort length = sizeof(ushort);
            Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer, offset, length);

            return length;
        }

        public static ushort AppendStringData(string data, byte[] buffer, int offset)
        {
            ushort length = sizeof(ushort); // 길이를 알려줄 공간 확보
            length += (ushort)Encoding.Unicode.GetBytes(data, 0, data.Length, buffer, offset + length);
            Buffer.BlockCopy(BitConverter.GetBytes(length), 0, buffer, offset, sizeof(ushort));

            return length;
        }
    }
}
