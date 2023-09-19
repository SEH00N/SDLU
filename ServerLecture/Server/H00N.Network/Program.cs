
using System.Net;

namespace H00N.Network
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry entry = Dns.GetHostEntry(host);
            IPAddress address = entry.AddressList[1];
            EndPoint endPoint = new IPEndPoint(address, 8081);
            Console.WriteLine(endPoint.ToString());
        }
    }
}