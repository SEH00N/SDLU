using H00N.Network;
using System.Net;
using System.Net.Sockets;

namespace TestServer
{
    internal class Program
    {
        public static Listener Listener;
        public static ChatRoom Room;

        public static JobQueue JobQueue;


        static void Main(string[] args)
        {
            Room = new ChatRoom();
            JobQueue = new JobQueue();

            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            //IPAddress ipAddress = ipHost.AddressList[1];
            IPAddress ipAddress = IPAddress.Parse("192.168.0.14");
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            Listener = new Listener(endPoint);
            if (Listener.Listen(10))
                Listener.StartAccept(OnAccepted);

            FlushLoop(1000 / 20);
        }

        private static void FlushLoop(int delay)
        {
            int last = Environment.TickCount;

            while (true)
            {
                int now = Environment.TickCount;
                if (now - last > delay)
                {
                    JobQueue.Push(() => Room.Flush());
                    last = now;
                }
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