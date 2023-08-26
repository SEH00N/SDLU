using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Socket listenSocket = CreateListenSocket();
            //Socket clientSocket = listenSocket.Accept();

            //// 통신하는 코드
            //while(true)
            //{
            //    bool isSuccess = Communication(clientSocket);

            //    if(isSuccess == false)
            //    {
            //        clientSocket.Shutdown(SocketShutdown.Both);
            //        clientSocket.Close();

            //        Console.WriteLine("Disconnected with client");

            //        break;
            //    }
            //}

            //listenSocket.Close();
            //Console.WriteLine("Server closed");
            //Console.ReadKey();

            IPAddress ipAddress = IPAddress.Parse("172.31.1.175");
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            Listener listener = new Listener(endPoint);
            listener.RegisterOnReceiveMessage((msg, clientSocket)=> {
                IPEndPoint clientEndPoint = (clientSocket.RemoteEndPoint) as IPEndPoint;
                Console.WriteLine($"{clientEndPoint.Address} : {msg}");

                listener.SendToAllClients($"{clientEndPoint.Address} : {msg}");
            });

            listener.Start(clientSocket => {
                IPEndPoint clientEndPoint = (clientSocket.RemoteEndPoint) as IPEndPoint;
                Console.WriteLine($"{clientEndPoint.Address} Joined");
            });

            Console.ReadKey();
        }

        private static Socket CreateListenSocket()
        {
            // 소켓을 열기 위한 준비물
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHost.AddressList[1];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081);

            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endPoint);
            socket.Listen(1);

            Console.WriteLine($"Server opened on port : {endPoint.Port}");

            return socket;
        }

        private static bool Communication(Socket clientSocket)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int receivedSize = clientSocket.Receive(buffer);

                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, receivedSize);
                IPEndPoint clientEndPoint = (clientSocket.RemoteEndPoint) as IPEndPoint;
                Console.WriteLine($"MESSAGE FROM {clientEndPoint.Address} : {receivedMessage}");

                if (receivedMessage.IndexOf("exit") > -1)
                    return false;

                string echoMessage = $"SERVER MESSAGE : {receivedMessage}";
                byte[] echoBytes = Encoding.UTF8.GetBytes(echoMessage);

                clientSocket.Send(echoBytes);

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