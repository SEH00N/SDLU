using System;
using System.Collections.Generic;
using System.Text;

namespace H00N.Network
{
    public class PacketUtility
    {
        /// <summary>
        /// 패킷 팩토리<br/>
        /// 직렬화 된 버퍼 형태의 패킷을 사용하여 제네릭 형태의 패킷을 생성함
        /// </summary>
        /// <param name="buffer">직렬화 된 패킷(버퍼)</param>
        public static Packet CreatePacket<T>(ArraySegment<byte> buffer) where T : Packet, new()
        {
            T packet = new T(); // 패킷 컨테이너 생성
            packet.Deserialize(buffer); // 패킷 역직렬화

            return packet; // 패킷 반환
        }

        /// <summary>
        /// 패킷의 아이디를 읽는 함수
        /// </summary>
        /// <param name="buffer">직렬화 된 패킷(버퍼)</param>
        public static ushort ReadPacketID(ArraySegment<byte> buffer)
        {
            ushort packetID = BitConverter.ToUInt16(buffer.Array, buffer.Offset + sizeof(ushort));
            return packetID;
        }

        /// <summary>
        /// buffer의 offset부터 리스트 데이터를 읽어들이는 함수
        /// </summary>
        /// <param name="buffer">직렬화 된 패킷(버퍼)</param>
        /// <param name="offset">읽어들이기 시작할 버퍼의 오프셋</param>
        /// <param name="result">읽어들인 리스트 데이터</param>
        /// <returns>처리한 바이트 개수</returns>
        public static ushort ReadListData<T>(ArraySegment<byte> buffer, int offset, out List<T> result) where T : DataPacket, new()
        {
            ushort process = 0;

            ushort listLength = BitConverter.ToUInt16(buffer.Array, offset);
            process += sizeof(ushort);

            result = new List<T>();

            for(int i = 0; i < listLength; i++)
            {
                process += ReadDataPacket<T>(buffer, offset + process, out T data);
                result.Add(data);
            }

            return process;
        }

        /// <summary>
        /// buffer의 offset부터 데이터 패킷을 읽어들이는 함수
        /// </summary>
        /// <param name="buffer">직렬화 된 패킷(버퍼)</param>
        /// <param name="offset">읽어들이기 시작할 버퍼의 오프셋</param>
        /// <param name="result">읽어들인 데이터 패킷</param>
        /// <returns>처리한 바이트 개수</returns>
        public static ushort ReadDataPacket<T>(ArraySegment<byte> buffer, int offset, out T result) where T : DataPacket, new()
        {
            ushort process = 0;

            result = new T();
            process += result.Deserialize(buffer, offset);

            return process;
        }

        /// <summary>
        /// buffer의 offset부터 int 데이터를 읽어들이는 함수
        /// </summary>
        /// <param name="buffer">직렬화 된 패킷(버퍼)</param>
        /// <param name="offset">읽어들이기 시작할 버퍼의 오프셋</param>
        /// <param name="result">읽어들인 int</param>
        /// <returns>처리한 바이트 개수</returns>
        public static ushort ReadIntData(ArraySegment<byte> buffer, int offset, out int result)
        {
            result = BitConverter.ToInt32(buffer.Array, buffer.Offset + offset);
            return sizeof(int);
        }

        /// <summary>
        /// buffer의 offset부터 ushort 데이터를 읽어들이는 함수
        /// </summary>
        /// <param name="buffer">직렬화 된 패킷(버퍼)</param>
        /// <param name="offset">읽어들이기 시작할 버퍼의 오프셋</param>
        /// <param name="result">읽어들인 ushort</param>
        /// <returns>처리한 바이트 개수</returns>
        public static ushort ReadUShortData(ArraySegment<byte> buffer, int offset, out ushort result)
        {
            result = BitConverter.ToUInt16(buffer.Array, buffer.Offset + offset);
            return sizeof(ushort);
        }

        /// <summary>
        /// buffer의 offset부터 short 데이터를 읽어들이는 함수
        /// </summary>
        /// <param name="buffer">직렬화 된 패킷(버퍼)</param>
        /// <param name="offset">읽어들이기 시작할 버퍼의 오프셋</param>
        /// <param name="result">읽어들인 short</param>
        /// <returns>처리한 바이트 개수</returns>
        public static ushort ReadShortData(ArraySegment<byte> buffer, int offset, out short result)
        {
            result = BitConverter.ToInt16(buffer.Array, buffer.Offset + offset);
            return sizeof(short);
        }

        /// <summary>
        /// buffer의 offset부터 float 데이터를 읽어들이는 함수
        /// </summary>
        /// <param name="buffer">직렬화 된 패킷(버퍼)</param>
        /// <param name="offset">읽어들이기 시작할 버퍼의 오프셋</param>
        /// <param name="result">읽어들인 float</param>
        /// <returns>처리한 바이트 개수</returns>
        public static ushort ReadFloatData(ArraySegment<byte> buffer, int offset, out float result)
        {
            result = BitConverter.ToSingle(buffer.Array, buffer.Offset + offset);
            return sizeof(float);
        }

        /// <summary>
        /// buffer의 offset부터 string 데이터를 읽어들이는 함수
        /// [문자열 길이][문자열 데이터...]
        /// </summary>
        /// <param name="buffer">직렬화 된 패킷(버퍼)</param>
        /// <param name="offset">읽어들이기 시작할 버퍼의 오프셋</param>
        /// <param name="result">읽어들인 string</param>
        /// <returns>처리한 바이트 개수</returns>
        public static ushort ReadStringData(ArraySegment<byte> buffer, int offset, out string result)
        {
            ushort process = 0;

            ushort stringLength = BitConverter.ToUInt16(buffer.Array, buffer.Offset + offset);
            stringLength -= 2;

            process += sizeof(ushort);

            result = Encoding.Unicode.GetString(buffer.Array, buffer.Offset + offset + process, stringLength);
            process += stringLength;

            return process;
        }

        /// <summary>
        /// buffer의 offset부터 리스트 데이터를 작성하는 함수
        /// </summary>
        /// <param name="data">적을 데이터</param>
        /// <param name="buffer">적을 버퍼(공간)</param>
        /// <param name="offset">적기 시작할 버퍼의 offset</param>
        /// <returns>작성한 바이트 개수</returns>
        public static ushort AppendListData<T>(List<T> data, ArraySegment<byte> buffer, int offset) where T : DataPacket, new()
        {
            ushort process = 0;
            process += AppendUShortData((ushort)data.Count, buffer, offset + process);

            for(int i = 0; i < data.Count; i++)
                process += data[i].Serialize(buffer, offset + process);

            return process;
        }

        /// <summary>
        /// buffer의 offset부터 데이터 패킷을 작성하는 함수
        /// </summary>
        /// <param name="data">적을 데이터</param>
        /// <param name="buffer">적을 버퍼(공간)</param>
        /// <param name="offset">적기 시작할 버퍼의 offset</param>
        /// <returns>작성한 바이트 개수</returns>
        public static ushort AppendDataPacket<T>(T data, ArraySegment<byte> buffer, int offset) where T : DataPacket, new()
        {
            ushort process = 0;

            process += data.Serialize(buffer, offset);
            return process;
        }

        /// <summary>
        /// buffer의 offset부터 int 데이터를 작성하는 함수
        /// </summary>
        /// <param name="data">적을 데이터</param>
        /// <param name="buffer">적을 버퍼(공간)</param>
        /// <param name="offset">적기 시작할 버퍼의 offset</param>
        /// <returns>작성한 바이트 개수</returns>
        public static ushort AppendIntData(int data, ArraySegment<byte> buffer, int offset)
        {
            ushort length = sizeof(int);
            Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer.Array, buffer.Offset + offset, length);

            return length;
        }

        /// <summary>
        /// buffer의 offset부터 ushort 데이터를 작성하는 함수
        /// </summary>
        /// <param name="data">적을 데이터</param>
        /// <param name="buffer">적을 버퍼(공간)</param>
        /// <param name="offset">적기 시작할 버퍼의 offset</param>
        /// <returns>작성한 바이트 개수</returns>
        public static ushort AppendUShortData(ushort data, ArraySegment<byte> buffer, int offset)
        {
            ushort length = sizeof(ushort);
            Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer.Array, buffer.Offset + offset, length);

            return length;
        }

        /// <summary>
        /// buffer의 offset부터 short 데이터를 작성하는 함수
        /// </summary>
        /// <param name="data">적을 데이터</param>
        /// <param name="buffer">적을 버퍼(공간)</param>
        /// <param name="offset">적기 시작할 버퍼의 offset</param>
        /// <returns>작성한 바이트 개수</returns>
        public static ushort AppendShortData(short data, ArraySegment<byte> buffer, int offset)
        {
            ushort length = sizeof(ushort);
            Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer.Array, buffer.Offset + offset, length);

            return length;
        }

        /// <summary>
        /// buffer의 offset부터 float 데이터를 작성하는 함수
        /// </summary>
        /// <param name="data">적을 데이터</param>
        /// <param name="buffer">적을 버퍼(공간)</param>
        /// <param name="offset">적기 시작할 버퍼의 offset</param>
        /// <returns>작성한 바이트 개수</returns>
        public static ushort AppendFloatData(float data, ArraySegment<byte> buffer, int offset)
        {
            ushort length = sizeof(float);
            Buffer.BlockCopy(BitConverter.GetBytes(data), 0, buffer.Array, buffer.Offset + offset, length);
            
            return length;
        }

        /// <summary>
        /// buffer의 offset부터 string 데이터를 작성하는 함수
        /// </summary>
        /// <param name="data">적을 데이터</param>
        /// <param name="buffer">적을 버퍼(공간)</param>
        /// <param name="offset">적기 시작할 버퍼의 offset</param>
        /// <returns>작성한 바이트 개수</returns>
        public static ushort AppendStringData(string data, ArraySegment<byte> buffer, int offset)
        {
            ushort length = sizeof(ushort);
            length += (ushort)Encoding.Unicode.GetBytes(data, 0, data.Length, buffer.Array, buffer.Offset + offset + length);
            Buffer.BlockCopy(BitConverter.GetBytes(length), 0, buffer.Array, buffer.Offset + offset, sizeof(ushort));

            return length;
        }
    }
}
