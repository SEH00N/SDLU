using System;

namespace H00N.Network
{
    public class SharedBuffer
    {
        private ArraySegment<byte> buffer; // 실제 버퍼
        private int readCursor; // ReadCursor (읽어야 할 커서, 읽은부분)
        private int writeCursor; // WriteCursor (적어야 할 커서, 적은부분)

        public int Size => writeCursor - readCursor; // 읽어야 할 데이터의 사이즈 (적은부분 - 읽은부분)
        public int Capacity => buffer.Count - writeCursor; // 남은 데이터의 사이즈 (전체의 길이 - 적은부분)

        public ArraySegment<byte> ReadBuffer => new ArraySegment<byte>(buffer.Array, buffer.Offset + readCursor, Size); // 읽을 할 부분 (읽은부분 ~ Size)
        public ArraySegment<byte> WriteBuffer => new ArraySegment<byte>(buffer.Array, buffer.Offset + writeCursor, Capacity); // 적을 부분 (적은 부분 ~ Capacity)

        public SharedBuffer(int bufferSize) // 생성자
        {
            buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize); // 버퍼 생성
        }

        public void PurifyBuffer() // 버퍼를 정리하는 함수
        {
            int dataLength = Size; // 현재 데이터의 길이
            if(dataLength == 0) // 만약 남은 데이터가 없다면
                readCursor = writeCursor = 0; // 현재 버퍼의 값들을 모두 무시하고 ReadCursor 와 WriteCursor를 가장 앞으로 이동
            else // 남아있는 데이터가 있다면
            {
                Array.Copy(buffer.Array, buffer.Offset + readCursor, buffer.Array, buffer.Offset, dataLength); // 남아있는 데이터를 가장 앞으로 이동
                readCursor = 0; // ReadCursor 가장 앞으로 이동
                writeCursor = dataLength; // 남아있는 데이터 뒤에서부터 적을 수 있도록 WriteCursor를 남은 데이터의 끝부분으로 이동
            }
        }

        public bool ShiftReadCursor(int readSize) // ReadCursor를 업데이트하는 함수
        {
            if (readSize > Size) // 이동하고자 하는 길이가 남아있던 데이터의 사이즈보다 크면 (문제 발생)
                return false; // 실패 반환

            // 그게 아니라면

            readCursor += readSize; // ReadCursor 이동
            return true; // 성공 반환
        }

        public bool ShiftWriteCursor(int writtenSize) // WriteCursor를 업데이트하는 함수
        {
            if (writtenSize > Capacity) // 이동하고자 하는 길이가 남은 버퍼의 사이즈보다 크면 (문제 발생)
                return false; // 실패 반환

            // 그게 아니라면

            writeCursor += writtenSize; // WriteCursor 이동
            return true; // 성공 반환
        }
    }
}
