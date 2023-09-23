using System;
using System.Net.Sockets;
using System.Net;

namespace H00N.Network
{
    public class Listener
    {
        private Socket listenSocket = null;
        private Action<Socket> onAccepted;

        private bool active = false;

        public Listener(IPEndPoint endPoint)
        {
            listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(endPoint);

            active = false;
        }

        public bool Listen(int backlog = 10)
        {
            if (active)
            {
                Console.WriteLine("Listen Socket is Already Listening");
                return false;
            }

            listenSocket.Listen(backlog);
            return true;
        }

        public void StartAccept(Action<Socket> onAccepted)
        {
            SocketAsyncEventArgs acceptArgs = new SocketAsyncEventArgs();
            acceptArgs.Completed += OnAcceptCompleted;
            this.onAccepted = onAccepted;

            Accept(acceptArgs);
        }

        private void Accept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            bool pending = listenSocket.AcceptAsync(args);
            if (pending == false)
                OnAcceptCompleted(null, args);
        }

        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Socket clientSocket = args.AcceptSocket;
                onAccepted?.Invoke(clientSocket);
            }

            Accept(args);
        }
    }
}
