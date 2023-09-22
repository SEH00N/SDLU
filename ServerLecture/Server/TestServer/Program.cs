using H00N.Network;
using System.Net;
using System.Net.Sockets;

namespace TestServer
{
    internal class Program
    {
        static Listener listener;

        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            listener = new Listener(endPoint);
            if (listener.Listen(10))
                listener.StartAccept(OnAccepted);
        }

        private static void OnAccepted(Socket socket)
        {
            ClientSession session = new ClientSession();
            session.Open(socket);
            session.OnConnected(socket.LocalEndPoint);
        }
    }
}