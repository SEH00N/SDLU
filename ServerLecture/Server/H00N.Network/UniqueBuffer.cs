namespace H00N.Network
{
    public class UniqueBuffer
    {
        public static ThreadLocal<SharedBuffer> LocalBuffer = new ThreadLocal<SharedBuffer>(() => null);
        public static SharedBuffer Buffer {
            get => LocalBuffer.Value;
            set {
                LocalBuffer.Value = value;
            }
        }

        public static int ChunkSize { get; set; } = 4096;

        public static ArraySegment<byte> Open(int reserveSize)
        {
            if (reserveSize > ChunkSize)
                return null;

            if (Buffer == null)
                Buffer = new SharedBuffer(ChunkSize);

            if (reserveSize > Buffer.Capacity)
                Buffer = new SharedBuffer(ChunkSize);

            return Buffer.WriteBuffer;
        }

        public static ArraySegment<byte> Close(int usedSize)
        {
            Buffer.ShiftWriteCursor(usedSize);
            Buffer.ShiftReadCursor(usedSize);

            return Buffer.ReadBuffer;
        }
    }
}
