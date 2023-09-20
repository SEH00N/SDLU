using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Listener
    {
        private Socket listenSocket = null;
        private List<Socket> clientSockets = new List<Socket>();

        private Action<Socket, string> onMessageReceived = null;
        private SocketAsyncEventArgs sendArgs;

        private object listLocker = new object();
        private object handlerLocker = new object();

        public Listener(IPEndPoint endPoint, int backlog, Action<Socket, string> onMessageReceived)
        {
            listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(endPoint);
            listenSocket.Listen(backlog);

            this.onMessageReceived = onMessageReceived;
            sendArgs = new SocketAsyncEventArgs();
        }

        public void Broadcast(string message)
        {
            lock(listLocker)
            {
                byte[] sendBytes = Encoding.UTF8.GetBytes(message);
                sendArgs.SetBuffer(sendBytes);

                clientSockets.ForEach(socket => socket.SendAsync(sendArgs));
            }
        }

        public void Kick(Socket socket)
        {
            lock (listLocker)
            {
                IPEndPoint clientSocketEndPoint = socket.RemoteEndPoint as IPEndPoint;

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

                clientSockets.Remove(socket);

                Console.WriteLine($"클라이언트가 접속 해제하였습니다. [{clientSocketEndPoint.Address}]");
            }
        }

        public void StartAccept()
        {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += OnAcceptCompleted;

            Accept(args);
        }

        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if(args.SocketError == SocketError.Success)
            {
                lock(listLocker)
                {
                    Socket clientSocket = args.AcceptSocket;
                    StartReceive(clientSocket);

                    clientSockets.Add(clientSocket);

                    IPEndPoint clientSocketEndPoint = clientSocket.RemoteEndPoint as IPEndPoint;
                    Console.WriteLine($"클라이언트가 접속하였습니다. [{clientSocketEndPoint.Address}]");
                }
            }
            else
                Console.WriteLine(args.SocketError);

            Accept(args);
        }

        private void Accept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            bool pending = listenSocket.AcceptAsync(args);
            if (pending == false) // 바로 실행이 됐을 때
                OnAcceptCompleted(null, args);
        }

        private void StartReceive(Socket socket)
        {
            SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.Completed += OnReceiveCompleted;
            receiveArgs.UserToken = socket;

            Receive(socket, receiveArgs);
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs args)
        {
            Socket socket = args.UserToken as Socket;

            if (args.SocketError == SocketError.Success && args.BytesTransferred > 0)
            {
                string receivedMessage = Encoding.UTF8.GetString(args.Buffer, 0, args.BytesTransferred);

                lock(handlerLocker)
                    onMessageReceived?.Invoke(socket, receivedMessage);

                Receive(socket, args);
            }
            else
                Kick(socket);
        }

        private void Receive(Socket socket, SocketAsyncEventArgs args)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024], 0, 1024);
            args.SetBuffer(buffer.Array, buffer.Offset, buffer.Count);

            bool pending = socket.ReceiveAsync(args);
            if (pending == false) // 바로 처리됨
                OnReceiveCompleted(null, args);
        }
    }
}
