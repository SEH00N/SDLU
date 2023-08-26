using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;

namespace Server
{
    public class Listener
    {
        private Socket listenSocket;
        private List<Socket> clients = new List<Socket>();

        private Action<string, Socket> onMessageReceivedEvent;
        private SocketAsyncEventArgs receiveArgs;


        private object locker = new object();

        public Listener(IPEndPoint endPoint)
        {
            listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            listenSocket.Bind(endPoint);
            listenSocket.Listen(6);

            receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.Completed += onReceiveHandle;
        }

        private void onReceiveHandle(object? sender, SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0)
            {
                Console.WriteLine(e.Buffer.Length);
                string message = Encoding.UTF8.GetString(e.Buffer);
                onMessageReceivedEvent?.Invoke(message, e.ConnectSocket);

            }

            Receive(e.ConnectSocket);
        }

        private void Receive(Socket socket)
        {
            bool pending = socket.ReceiveAsync(receiveArgs);
            if (!pending)
                onReceiveHandle(this, receiveArgs);
        }

        public void Start(Action<Socket> onAcceptClinet)
        {
            SocketAsyncEventArgs evt = new SocketAsyncEventArgs();
            evt.Completed += new EventHandler<SocketAsyncEventArgs>((sender, e) => {
                Socket clientSocket = e.AcceptSocket;
                Receive(clientSocket);

                lock (locker)
                    clients.Add(clientSocket);

                onAcceptClinet?.Invoke(clientSocket);

                Start(onAcceptClinet);
            });

            listenSocket.AcceptAsync(evt);
        }

        public void SendToAllClients(string message)
        {
            lock(locker)
            {
                byte[] sendData = Encoding.UTF8.GetBytes(message);
                SocketAsyncEventArgs evt = new SocketAsyncEventArgs();
                evt.SetBuffer(sendData);

                clients.ForEach(s => listenSocket.SendAsync(evt));
            }
        }

        public void RegisterOnReceiveMessage(Action<string, Socket> callback) => onMessageReceivedEvent += callback;
        public void UnegisterOnReceiveMessage(Action<string, Socket> callback) => onMessageReceivedEvent -= callback;
    }
}
