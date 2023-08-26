using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Client
    {
        private Action<string, Socket> onMessageReceivedEvent;
        private IPEndPoint endPoint;
        private Socket serverSocket;

        public Client(IPEndPoint endPoint)
        {
            this.endPoint = endPoint;
            serverSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect()
        {
            serverSocket.Connect(endPoint);
        }

        public void RegisterOnReceiveMessage(Action<string, Socket> callback) => onMessageReceivedEvent += callback;
        public void UnegisterOnReceiveMessage(Action<string, Socket> callback) => onMessageReceivedEvent -= callback;

        public void Send(string msg)
        {
            Console.WriteLine(msg);
            byte[] messageBytes = Encoding.UTF8.GetBytes(msg);

            SocketAsyncEventArgs evt = new SocketAsyncEventArgs();
            evt.SetBuffer(messageBytes, 0, messageBytes.Length);

            serverSocket.SendAsync(evt);
        }

        public void Receive()
        {
            SocketAsyncEventArgs evt = new SocketAsyncEventArgs();
            evt.Completed += new EventHandler<SocketAsyncEventArgs>((sender, e) => {
                string message = Encoding.UTF8.GetString(e.Buffer);
                onMessageReceivedEvent?.Invoke(message, serverSocket);

                Receive();
            });

            serverSocket.ReceiveAsync(evt);
        }
    }
}
