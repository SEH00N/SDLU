using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Socket serverSocket = CreateServerSocket(out IPEndPoint endPoint); // 서버와 연결할 소켓 생성
            serverSocket.Connect(endPoint); // 서버와 연결

            Console.WriteLine("Success to join server");

            while(true)
            {
                bool isSuccess = Communication(serverSocket); // 통신하기

                if (isSuccess == false) // 통신에 실패했으면
                {
                    serverSocket.Shutdown(SocketShutdown.Both); // 서버 소켓 송수신 제한
                    serverSocket.Close(); // 서버 소켓 연결 끊기
                    Console.WriteLine("Disconnected with server");

                    break;
                }
            }
        }

        private static Socket CreateServerSocket(out IPEndPoint endPoint)
        {
            // 소켓 생성 준비물
            string host = Dns.GetHostName(); // 해당 로컬 컴퓨터의 DNS 호스트 이름을 불러옴
            IPHostEntry ipHost = Dns.GetHostEntry(host); // DNS를 바탕으로 IP 주소 정보 객체 생성
            IPAddress ipAddress = ipHost.AddressList[1]; // 같은 IP리스트에서 두번째 IP 주소를 뽑음 (IPv4)
            endPoint = new IPEndPoint(ipAddress, 8081); // 8081 포트로 소켓이 열릴 종단점 생성

            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // 네트워크 유형, 소켓 타입, 사용할 프로토콜

            return socket;
        }

        // 통신 성공했으면 true를 반환, 통신이 실패했거나 통신을 끝냈으면 false를 반환
        private static bool Communication(Socket serverSocket)
        {
            try
            {
                string message = Console.ReadLine(); // 보낼 메세지 입력
                byte[] messageBytes = Encoding.UTF8.GetBytes(message); // 메세지 바이트로 변환

                serverSocket.Send(messageBytes); // 서버에게 메세지 바이트 전송
                Console.WriteLine($"SENT MESSAGE : {message}"); // 보낸 메세지 출력

                if(message.IndexOf("exit") > -1) // 보낸 메세지에 exit라는 메세지가 포함되어있으면
                    return false;

                byte[] buffer = new byte[1024]; // 데이터를 받을 저장공간
                int receivedSize = serverSocket.Receive(buffer); // 서버가 보낸 메세지 받기

                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, receivedSize); // 받은 메세지를 string으로 변환
                Console.WriteLine(receivedMessage); // 받은 메세지 출력

                return true;
            } 
            catch(Exception err)
            {
                Console.WriteLine(err.Message); // 에러 메세지 출력
                return false; // 통신이 실패했음을 알림
            }
        }
    }
}