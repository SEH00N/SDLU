using System.Net;

namespace Client
{
    internal class Program
    {
        private static Connector connector;

        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("172.31.1.175");
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            connector = new Connector(endPoint, OnMessageReceived);
            connector.Connect(endPoint);

            while (true)
            {
                string message = Console.ReadLine();
                connector.Send(message);

                if (message.IndexOf("exit") > -1)
                {
                    connector.Disconnect();
                    break;
                }
            }
        }
        private static void OnMessageReceived(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}