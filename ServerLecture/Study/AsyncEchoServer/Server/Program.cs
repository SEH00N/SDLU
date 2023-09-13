using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal class Program
    {
        private static Listener listener;

        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHost.AddressList[1];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            listener = new Listener(endPoint, 10, OnMessageReceived);
            listener.StartAccept();

            while(true)
            {
                // 서버가 끝나지 않도록
            }
        }

        private static void OnMessageReceived(Socket socket, string msg)
        {
            IPEndPoint clientEndPoint = (socket.RemoteEndPoint as IPEndPoint);

            string message = $"[{clientEndPoint.Address}] {msg}";
            Console.WriteLine(message);
            listener.Broadcast(message);

            if (msg.IndexOf("exit") > -1)
                listener.Kick(socket);
        }
    }
}