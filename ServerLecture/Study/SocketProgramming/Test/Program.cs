using System.Net;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry ipHost = Dns.GetHostEntry("www.xvideos.com");
            foreach(var i in ipHost.AddressList)
                Console.WriteLine(i);
        }
    }
}