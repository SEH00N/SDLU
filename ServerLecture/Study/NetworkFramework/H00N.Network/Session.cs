using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;

namespace H00N.Network
{
    public abstract class Session
    {
        public const int HeaderSize = sizeof(ushort);

        private Socket socket;

        private int active = 0;
        public int Active => active;

        private SharedBuffer receiveBuffer = new SharedBuffer(4096);
        private Queue<ArraySegment<byte>> sendQueue = new Queue<ArraySegment<byte>>();
        private List<ArraySegment<byte>> pendingList = new List<ArraySegment<byte>>();

        private SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();

        private object locker = new object();

        public abstract void OnConnected(EndPoint endPoint);
        public abstract void OnDisconnected(EndPoint endPoint);
        public abstract void OnPacketReceive(ArraySegment<byte> buffer);
        public abstract void OnSent(int length);

        public void Open(Socket socket)
        {
            if (Interlocked.Exchange(ref active, 1) == 1)
                return;

            this.socket = socket;

            SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.Completed += OnReceiveCompleted;
            sendArgs.Completed += OnSendCompleted;

            Receive(receiveArgs);
        }

        public void Close()
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
            lock(locker)
            {
                sendQueue.Enqueue(sendBuffer);
                if (pendingList.Count == 0)
                    FlushSendQueue();
            }    
        }

        private void Release()
        {
            lock(locker)
            {
                sendQueue.Clear();
                pendingList.Clear();
            }
        }

        #region SEND
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
            lock(locker)
            {
                if(args.SocketError == SocketError.Success && args.BytesTransferred > 0)
                {
                    sendArgs.BufferList = null;
                    pendingList.Clear();

                    OnSent(args.BytesTransferred);

                    if (sendQueue.Count > 0)
                        FlushSendQueue();
                }
                else
                    Close();
            }
        }
        #endregion

        #region RECEIVE
        private void Receive(SocketAsyncEventArgs args)
        {
            receiveBuffer.PurifyBuffer();

            ArraySegment<byte> buffer = receiveBuffer.WriteBuffer;
            args.SetBuffer(buffer.Array, buffer.Offset, buffer.Count);

            bool pending = socket.ReceiveAsync(args);
            if (pending == false)
                OnReceiveCompleted(null, args);
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success && args.BytesTransferred > 0)
            {
                bool result = receiveBuffer.ShiftWriteCursor(args.BytesTransferred);

                if (result == false)
                {
                    Close();
                    return;
                }

                ArraySegment<byte> buffer = receiveBuffer.ReadBuffer;

                int processedLength = HandleBuffer(buffer);
                if (processedLength < 0 || receiveBuffer.Size < processedLength)
                {
                    Close();
                    return;
                }

                result = receiveBuffer.ShiftReadCursor(processedLength);
                if (result == false)
                {
                    Close();
                    return;
                }

                Receive(args);
            }
            else
            {
                Close();
            }
        }

        private int HandleBuffer(ArraySegment<byte> buffer)
        {
            int processedLength = 0;

            while(true)
            {
                if (buffer.Count < HeaderSize)
                    break;

                ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
                if (buffer.Count < dataSize)
                    break;

                OnPacketReceive(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));

                processedLength += dataSize;
                buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
            }

            return processedLength;
        }
        #endregion
    }
}
