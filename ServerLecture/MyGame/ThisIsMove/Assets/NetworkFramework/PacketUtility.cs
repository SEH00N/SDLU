using System;
using System.Collections.Generic;
using System.Text;

namespace H00N.Network
{
    public class PacketUtility
    {
        public static Packet CreatePacket<T>(ArraySegment<byte> buffer) where T : Packet, new()
        {
            T packet = new T(); // 패킷 컨테이너 생성
            packet.Deserialize(buffer); // 패킷 역직렬화

            return packet; // 패킷 반환
        }

        public static ushort ReadPacketID(ArraySegment<byte> buffer) // 패킷의 ID 부분을 읽는 함수
        {
            ushort packetID = BitConverter.ToUInt16(buffer.Array, buffer.Offset + sizeof(ushort)); // 처음(Offset) + 헤더부터 ID의 사이즈만큼 읽기
            return packetID; // 패킷 아이디 반환
        }

        public static ushort ReadListData<T>(ArraySegment<byte> buffer, int offset, out List<T> result) where T : DataPacket, new()
        {
            ushort process = 0;

            ushort listLength = BitConverter.ToUInt16(buffer.Array, offset);
            process += sizeof(ushort);

            result = new List<T>();

            for (int i = 0; i < listLength; i++)
            {
                process += ReadDataPacket<T>(buffer, offset + process, out T data);
                result.Add(data);
            }

            return process;
        }
        public static ushort ReadDataPacket<T>(ArraySegment<byte> buffer, int offset, out T result) where T : DataPacket, new()
        {
            ushort process = 0;

            result = new T();
            process += result.Deserialize(buffer, offset);

            return process;
        }

        public static ushort ReadIntData(ArraySegment<byte> buffer, int offset, out int result)
        {
            result = BitConverter.ToInt32(buffer.Array, buffer.Offset + offset);
            return sizeof(int);
        }

        public static ushort ReadUShortData(ArraySegment<byte> buffer, int offset, out ushort result)
        {
            result = BitConverter.ToUInt16(buffer.Array, buffer.Offset + offset);
            return sizeof(ushort);
        }

        public static ushort ReadShortData(ArraySegment<byte> buffer, int offset, out short result)
        {
            result = BitConverter.ToInt16(buffer.Array, buffer.Offset + offset);
            return sizeof(short);
        }

        public static ushort ReadFloatData(ArraySegment<byte> buffer, int offset, out float result)
        {
            result = BitConverter.ToSingle(buffer.Array, buffer.Offset + offset);
            return sizeof(float);
        }

        public static ushort ReadStringData(ArraySegment<byte> buffer, int offset, out string result) // 직렬화면 문자열을 읽는 함수
        {
            ushort process = 0; // 처리한 길이

            ushort stringLength = BitConverter.ToUInt16(buffer.Array, buffer.Offset + offset); // 문자열 길이 추출
            stringLength -= 2; // 헤더 제거

            process += sizeof(ushort); // 문자열 길이정보 처리(ushort)

            result = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + offset + process, stringLength); // 패킷에서 문자열 길이만큼 문자열 추출
            process += stringLength; // 문자열 처리 (읽어들인 문자열 길이)

            return process; // 처리한 길이 반환
        }

        public static ushort AppendListData<T>(List<T> data, ArraySegment<byte> buffer, int offset) where T : DataPacket, new()
        {
            ushort process = 0;
            process += AppendUShortData((ushort)data.Count, buffer, offset + process);

            for (int i = 0; i < data.Count; i++)
                process += data[i].Serialize(buffer, offset + process);

            return process;
        }

        public static ushort AppendDataPacket<T>(T data, ArraySegment<byte> buffer, int offset) where T : DataPacket, new()
        {
            ushort process = 0;

            process += data.Serialize(buffer, offset);
            return process;
        }

        public static ushort AppendIntData(int data, ArraySegment<byte> buffer, int offset) // int 데이터를 버퍼에 삽입하는 함수
        {
            ushort length = sizeof(int); // 처리할 길이 (int)
            Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer.Array, buffer.Offset + offset, length); // 직렬화한 정수를 버퍼에 삽입

            return length; // 처리한 길이 반환
        }

        public static ushort AppendUShortData(ushort data, ArraySegment<byte> buffer, int offset) // ushort 데이터를 버퍼에 삽입하는 함수
        {
            ushort length = sizeof(ushort); // 처리할 길이 (ushort)
            Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer.Array, buffer.Offset + offset, length); // 직렬화한 ushort를 버퍼에 삽입

            return length; // 처리한 길이 반환
        }

        public static ushort AppendShortData(short data, ArraySegment<byte> buffer, int offset) // ushort 데이터를 버퍼에 삽입하는 함수
        {
            ushort length = sizeof(ushort); // 처리할 길이 (ushort)
            Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer.Array, buffer.Offset + offset, length); // 직렬화한 ushort를 버퍼에 삽입

            return length; // 처리한 길이 반환
        }

        public static ushort AppendFloatData(float data, ArraySegment<byte> buffer, int offset)
        {
            ushort length = sizeof(float);
            Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer.Array, buffer.Offset + offset, length);

            return length;
        }

        public static ushort AppendStringData(string data, ArraySegment<byte> buffer, int offset) // 문자열 데이터를 버퍼에 삽입하는 함수
        {
            ushort length = sizeof(ushort); // 문자열 길이를 삽입할 공간 확보
            length += (ushort)Encoding.Unicode.GetBytes(data, 0, data.Length, buffer.Array, buffer.Offset + offset + length); // 처리한 문자열 길이
            Buffer.BlockCopy(BitConverter.GetBytes(length), 0, buffer.Array, buffer.Offset + offset, sizeof(ushort)); // 확보해둔 공간에 문자열 길이 삽입

            return length; // 처리한 길이 반환
        }
    }
}
