using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Connector
    {
        private Socket serverSocket;
        private Action<string> onMessageReceived;

        private SocketAsyncEventArgs sendArgs;

        public Connector(IPEndPoint endPoint, Action<string> onMessageReceived)
        {
            serverSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            this.onMessageReceived = onMessageReceived;
            sendArgs = new SocketAsyncEventArgs();
        }

        public void Connect(IPEndPoint endPoint)
        {
            serverSocket.Connect(endPoint);

            Console.WriteLine("서버와 연결되었습니다.");
            StartReceive();
        }

        public void Disconnect()
        {
            serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Close();

            Console.WriteLine("서버와 연결이 끊겼습니다.");
        }

        public void Send(string message)
        {
            byte[] sendBytes = Encoding.UTF8.GetBytes(message);
            sendArgs.SetBuffer(sendBytes);

            serverSocket.SendAsync(sendArgs);
        }

        private void StartReceive()
        {
            SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.Completed += OnReceiveCompleted;

            Receive(receiveArgs);
        }

        private void OnReceiveCompleted(object? sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success && args.BytesTransferred > 0)
            {
                string receivedMessage = Encoding.UTF8.GetString(args.Buffer, 0, args.BytesTransferred);
                onMessageReceived?.Invoke(receivedMessage);

                Receive(args);
            }
            else
                Disconnect();
        }

        private void Receive(SocketAsyncEventArgs args)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024], 0, 1024);
            args.SetBuffer(buffer.Array, buffer.Offset, buffer.Count);

            bool pending = serverSocket.ReceiveAsync(args);
            if (pending == false)
                OnReceiveCompleted(null, args);
        }
    }
}
