using H00N.Network;
using System.Net;
using System.Net.Sockets;

namespace TestServer
{
    internal class Program
    {
        public static Listener Listener;
        public static ChatRoom Room;

        static void Main(string[] args)
        {
            Room = new ChatRoom();

            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHost.AddressList[1];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            Listener = new Listener(endPoint);
            if (Listener.Listen(10))
                Listener.StartAccept(OnAccepted);

            while(true)
            {

            }
        }

        private static void OnAccepted(Socket socket)
        {
            ClientSession session = new ClientSession();
            session.Open(socket);
            session.OnConnected(socket.LocalEndPoint);
        }
    }
}