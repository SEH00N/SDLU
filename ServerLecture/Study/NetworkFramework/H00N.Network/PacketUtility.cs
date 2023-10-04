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
            ushort packetID = BitConverter.ToUInt16(buffer.Array, buffer.Offset + sizeof(ushort));
            return packetID;
        }

        public static ushort ReadUShortData(ArraySegment<byte> buffer, int offset, out ushort result)
        {
            result = BitConverter.ToUInt16(buffer.Array, buffer.Offset + offset);
            return sizeof(ushort);
        }

        public static ushort TranslateString(ArraySegment<byte> buffer, int offset, out string result)
        {
            ushort process = 0;

            ushort stringLength = BitConverter.ToUInt16(buffer.Array, buffer.Offset + offset + process);
            stringLength -= 2;

            process += sizeof(ushort);

            result = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + offset + process, stringLength);
            process += stringLength;

            return process;
        }

        public static ushort AppendUShortData(ushort data, ArraySegment<byte> buffer, int offset)
        {
            ushort length = sizeof(ushort);
            Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer.Array, buffer.Offset + offset, length);

            return length;
        }

        public static ushort AppendStringData(string data, ArraySegment<byte> buffer, int offset)
        {
            ushort length = sizeof(ushort);
            length += (ushort)Encoding.Unicode.GetBytes(data, 0, data.Length, buffer.Array, buffer.Offset + offset + length);
            Buffer.BlockCopy(BitConverter.GetBytes(length), 0, buffer.Array, buffer.Offset + offset, sizeof(ushort));

            return length;
        }
    }
}
