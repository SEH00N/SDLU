using H00N.Network;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal class Program
    {
        public static GameRoom Room;
        public static ushort PlayerCount = 0;

        public static Listener listener;

        static void Main(string[] args)
        {
            Room = new GameRoom();

            IPAddress ipAddress = IPAddress.Parse("172.31.1.175");
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            listener = new Listener(endPoint);
            if (listener.Listen(10))
                listener.StartAccept(OnAccepted);

            while(true) {}
        }

        private static void OnAccepted(Socket socket)
        {
            ClientSession session = new ClientSession();
            session.Open(socket);
            session.OnConnected(socket.RemoteEndPoint);
        }
    }
}