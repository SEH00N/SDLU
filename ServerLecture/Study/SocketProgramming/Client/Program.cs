using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        private Action<string, Socket> onMessageReceivedEvent;

        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("172.31.1.175");
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            Client client = new Client(endPoint);
            client.RegisterOnReceiveMessage((msg, socket) => {
                Console.WriteLine(msg);
            });

            client.Connect();
            client.Receive();

            while(true)
            {
                string message = Console.ReadLine();
                client.Send(message);
            }

            Console.ReadKey();

            //Console.WriteLine("Success to join server");

            //// 통신
            //while(true)
            //{
            //    bool isSuccess = Communication(serverSocket);

            //    if (isSuccess == false)
            //    {
            //        serverSocket.Shutdown(SocketShutdown.Both);
            //        serverSocket.Close();

            //        Console.WriteLine("Diconnected with server");

            //        break;
            //    }
            //}

            //Console.ReadKey();
        }

        

        private static Socket CreateServerSocket(out IPEndPoint endPoint)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHost.AddressList[1];
            endPoint = new IPEndPoint(ipAddress, 8081);

            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            return socket;
        }

        private static bool Communication(Socket serverSocket)
        {
            try
            {
                string message = Console.ReadLine();
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);

                serverSocket.Send(messageBytes);
                Console.WriteLine($"SENT MESSAGE : {message}");

                if (message.IndexOf("exit") > -1)
                    return false;

                byte[] buffer = new byte[1024];
                int receivedSize = serverSocket.Receive(buffer);

                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, receivedSize);
                Console.WriteLine(receivedMessage);

                return true;
            }
            catch(Exception err)
            {
                Console.WriteLine(err.Message);
                return false;
            }
        }
    }
}