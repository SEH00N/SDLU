using System.Net;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;

namespace H00N.Network
{
    public abstract class Session
    {
        public const int HeaderSize = sizeof(ushort);

        private Socket socket;
        private int active;

        private SharedBuffer receiveBuffer = new SharedBuffer(4096);
        private Queue<ArraySegment<byte>> sendQueue = new Queue<ArraySegment<byte>>();
        private List<ArraySegment<byte>> pendingList = new List<ArraySegment<byte>>();

        private SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();

        public abstract void OnConnected(EndPoint endPoint);
        public abstract void OnDisconnected(EndPoint endPoint);
        public abstract void OnPacketReceived(ArraySegment<byte> buffer);
        public abstract void OnSent(int length);

        public void Open(Socket socket)
        {
            if (active == 1)
                return;

            this.socket = socket;

            SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.Completed += OnReceiveCompleted;
            sendArgs.Completed += OnSendCompleted;

            Receive(receiveArgs);
        }

        public void Disconnect()
        {
            if (Interlocked.Exchange(ref active, 0) == 0)
                return;

            EndPoint endPoint = socket.RemoteEndPoint;

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            Release();

            OnDisconnected(endPoint);
        }

        public void Send(ArraySegment<byte> sendBuffer)
        {
            sendQueue.Enqueue(sendBuffer);

            if (pendingList.Count == 0)
                FlushSendQueue();
        }

        private void Release()
        {
            sendQueue.Clear();
            pendingList.Clear();
        }

        #region Send
        private void FlushSendQueue()
        {
            if (active == 0)
                return;

            while(sendQueue.Count > 0)
            {
                ArraySegment<byte> buffer = sendQueue.Dequeue();
                pendingList.Add(buffer);
            }

            sendArgs.BufferList = pendingList;

            bool pending = socket.SendAsync(sendArgs);
            if (pending == false)
                OnSendCompleted(null, sendArgs);
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success && args.BytesTransferred > 0)
            {
                sendArgs.BufferList = null;
                pendingList.Clear();

                OnSent(args.BytesTransferred);

                if (sendQueue.Count > 0)
                    FlushSendQueue();
            }
            else
                Disconnect();
        }
        #endregion

        #region Receive
        private void Receive(SocketAsyncEventArgs args)
        {
            ArraySegment<byte> buffer = receiveBuffer.WriteBuffer;
            args.SetBuffer(buffer.Array, buffer.Offset, buffer.Count);

            bool pending = socket.ReceiveAsync(args);
            if (pending == false)
                OnReceiveCompleted(null, args);
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs args)
        {
            if(args.SocketError == SocketError.Success && args.BytesTransferred > 0)
            {
                bool result = receiveBuffer.ShiftWriteCursor(args.BytesTransferred);
                if(result == false)
                {
                    Disconnect();
                    return;
                }

                ArraySegment<byte> buffer = receiveBuffer.ReadBuffer;

                int processedLength = HandleBuffer(buffer);
                if(processedLength < 0 || receiveBuffer.Size < processedLength)
                {
                    Disconnect();
                    return;
                }

                result = receiveBuffer.ShiftReadCursor(processedLength);
                if(result == false)
                {
                    Disconnect();
                    return;
                }

                Receive(args);
            }
            else
            {
                Disconnect();
            }
        }

        private int HandleBuffer(ArraySegment<byte> buffer)
        {
            int processedLength = 0;

            while(true)
            {
                if (buffer.Count < HeaderSize)
                    break;

                // [길이] [타입] [데이터...]
                ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
                if (buffer.Count > dataSize)
                    break;

                OnPacketReceived(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));

                processedLength += dataSize;
                buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
            }

            return processedLength;
        }
        #endregion
    }
}
