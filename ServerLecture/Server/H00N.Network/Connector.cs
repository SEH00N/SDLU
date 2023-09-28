using System;
using System.Net;
using System.Net.Sockets;

namespace H00N.Network
{
    public class Connector
    {
        private Socket socket;
        private Session session;
        private bool onConnecting = false;

        public Connector(IPEndPoint endPoint, Session session)
        {
            this.session = session;
            socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            onConnecting = false;
        }

        public void StartConnect(IPEndPoint endPoint)
        {
            if (onConnecting)
                return;

            onConnecting = true;

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = endPoint;
            args.Completed += OnConnectCompleted;

            Connect(args);
        }

        private void Connect(SocketAsyncEventArgs args)
        {
            bool pending = socket.ConnectAsync(args);
            if (pending == false)
                OnConnectCompleted(null, args);
        }

        private void OnConnectCompleted(object sender, SocketAsyncEventArgs args)
        {
            if(args.SocketError == SocketError.Success)
            {
                Socket socket = args.ConnectSocket;
                session.Open(socket);
                session.OnConnected(socket.RemoteEndPoint);
            }
        }
    }
}
