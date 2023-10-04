using H00N.Network;
using Packets;
using System.Net;

namespace TestClient
{
    internal class Program
    {
        public static Connector connector;
        public static ServerSession serverSession;

        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("172.31.1.175");
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            serverSession = new ServerSession();

            connector = new Connector(endPoint, serverSession);
            connector.StartConnect(endPoint);

            while (true) { }
        }

        public static async void Chatting()
        {
            while(true)
            {
                if (serverSession.Active == 0)
                    break;

                string message = Console.ReadLine();
                C_ChatPacket chatPacket = new C_ChatPacket();
                chatPacket.message = message;

                serverSession.Send(chatPacket.Serialize());
            }
        }
    }
}