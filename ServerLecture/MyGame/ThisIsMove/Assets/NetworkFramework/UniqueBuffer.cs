using System;
using System.Threading;

namespace H00N.Network
{
    public class UniqueBuffer
    {
        public static ThreadLocal<SharedBuffer> LocalBuffer = new ThreadLocal<SharedBuffer>(() => null); // 로컬로 가질 버퍼
        public static SharedBuffer Buffer { // 편하게 접근하기 위한 프로퍼티
            get => LocalBuffer.Value;
            set { LocalBuffer.Value = value; }
        }

        public static int ChunkSize { get; set; } = 65536; // 버퍼의 사이즈

        public static ArraySegment<byte> Open(int reserveSize) // 버퍼 발급
        {
            if (reserveSize > ChunkSize) // 제공받고자 하는 사이즈가 전체 버퍼의 사이즈보다 크면 (문제 발생)
            {
                Buffer = null;
                return null; // null 반환
            }

            if (Buffer == null) // 버퍼가 비어있다면
                Buffer = new SharedBuffer(ChunkSize); // 버퍼 새로 생성

            if (reserveSize > Buffer.Capacity) // 제공받고자 하는 사이즈만큼의 여유 공간이 없다면
                Buffer.PurifyBuffer(); // 버퍼 정리

            return Buffer.WriteBuffer; // 버퍼 발급
        }

        public static ArraySegment<byte> Close(int usedSize) // 버퍼 반환
        {
            Buffer.ShiftWriteCursor(usedSize); // 사용한 만큼 WriteCursor 이동

            ArraySegment<byte> buffer = Buffer.ReadBuffer; // 읽을 부분 캐싱
            Buffer.ShiftReadCursor(usedSize); // 읽었다 치고 ReadCursor 이동

            // 실질적인 처리는 buffer를 받아서 처리할 예정

            return buffer; // buffer 반환
        }
    }
}
