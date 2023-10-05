using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H00N.Network
{
    public class SharedBuffer
    {
        private ArraySegment<byte> buffer;
        private int readCursor;
        private int writeCursor;

        public int Size => writeCursor - readCursor;
        public int Capacity => buffer.Count - writeCursor;

        public ArraySegment<byte> ReadBuffer => new ArraySegment<byte>(buffer.Array, buffer.Offset + readCursor, Size);
        public ArraySegment<byte> WriteBuffer => new ArraySegment<byte>(buffer.Array, buffer.Offset + writeCursor, Capacity);

        public SharedBuffer(int bufferSize)
        {
            buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
        }

        public void PurifyBuffer()
        {
            int dataLength = Size;
            if (dataLength == 0)
                readCursor = writeCursor = 0;
            else
            {
                Buffer.BlockCopy(buffer.Array, buffer.Offset + readCursor, buffer.Array, buffer.Offset, dataLength);
                readCursor = 0;
                writeCursor = dataLength;
            }
        }

        public bool ShiftReadCursor(int readSize)
        {
            if (readSize > Size)
                return false;

            readCursor += readSize;
            return true;
        }

        public bool ShiftWriteCursor(int writtenSize)
        {
            if (writtenSize > Capacity)
                return false;

            writeCursor += writtenSize;
            return true;
        }
    }
}
