using System;
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

        public static ushort TranslateString(ArraySegment<byte> buffer, int offset, out string result) // 직렬화면 문자열을 읽는 함수
        {
            ushort process = 0; // 처리한 길이

            ushort stringLength = BitConverter.ToUInt16(buffer.Array, buffer.Offset + offset + process); // 문자열 길이 추출
            stringLength -= 2; // 헤더 제거

            process += sizeof(ushort); // 문자열 길이정보 처리(ushort)

            result = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + offset + process, stringLength); // 패킷에서 문자열 길이만큼 문자열 추출
            process += stringLength; // 문자열 처리 (읽어들인 문자열 길이)

            return process; // 처리한 길이 반환
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

        public static ushort AppendStringData(string data, ArraySegment<byte> buffer, int offset) // 문자열 데이터를 버퍼에 삽입하는 함수
        {
            ushort length = sizeof(ushort); // 문자열 길이를 삽입할 공간 확보
            length += (ushort)Encoding.Unicode.GetBytes(data, 0, data.Length, buffer.Array, buffer.Offset + offset + length); // 처리한 문자열 길이
            Buffer.BlockCopy(BitConverter.GetBytes(length), 0, buffer.Array, buffer.Offset + offset, sizeof(ushort)); // 확보해둔 공간에 문자열 길이 삽입

            return length; // 처리한 길이 반환
        }
    }
}
