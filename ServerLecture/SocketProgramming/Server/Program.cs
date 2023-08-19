using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Socket listenSocket = CreateListenSocket(); // 소켓 생성
            Socket clientSocket = listenSocket.Accept(); // 접속을 요청한 클라이언트 수락

            Console.WriteLine("Client joined the server");

            while(true)
            {
                bool isSuccess = Communication(clientSocket); // 통신하기
                
                if(isSuccess == false) // 통신에 실패했으면
                {
                    clientSocket.Shutdown(SocketShutdown.Both); // 클라이언트 소켓 송수신 제한
                    clientSocket.Close(); // 클라이언트 소켓 연결 끊기
                    Console.WriteLine("Disconnected with client");

                    break;
                }
            }

            listenSocket.Close(); // 모든 작업 후에 리슨 소켓 종료
            Console.WriteLine("Server closed");
            Console.ReadKey();
        }

        // 소켓 생성기
        private static Socket CreateListenSocket()
        {
            // 소켓 생성 준비물
            string host = Dns.GetHostName(); // 해당 로컬 컴퓨터의 DNS 호스트 이름을 불러옴
            IPHostEntry ipHost = Dns.GetHostEntry(host); // DNS를 바탕으로 IP 주소 정보 객체 생성
            IPAddress ipAddress = ipHost.AddressList[1]; // 같은 IP리스트에서 두번째 IP 주소를 뽑음
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081); // 8081 포트로 소켓이 열릴 종단점 생성

            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // 네트워크 유형, 소켓 타입, 사용할 프로토콜
            socket.Bind(endPoint); // 소켓을 종단점에 할당 (누군가가 전화를 걸 수 있는 상태가 됨)
            socket.Listen(1); // 1개의 대기열을 갖도록 소켓을 대기시킴 (오는 전화를 받을 수 있는 상태가 됨)

            Console.WriteLine($"Server opened on port : {endPoint.Port}");

            return socket;
        }

        // 통신 성공했으면 true를 반환, 통신이 실패했거나 통신을 끝냈으면 false를 반환
        private static bool Communication(Socket clientSocket)
        {
            try
            {
                // 데이터 수신
                byte[] buffer = new byte[1024]; // 데이터를 받을 저장공간
                int receivedSize = clientSocket.Receive(buffer); // client가 보낸 메세지 받기

                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, receivedSize); // 받은 메세지를 string으로 변환
                IPEndPoint clientEndPoint = (clientSocket.RemoteEndPoint as IPEndPoint); // client의 종단점 받아오기
                Console.WriteLine($"MESSAGE FROM {clientEndPoint.Address} : {receivedMessage}"); // 받은 메세지 출력

                if(receivedMessage.IndexOf("exit") > -1) // 받은 메세지에 exit라는 메세지가 포함되어있으면
                    return false; // 클라이언트가 접속 종료했음을 알림

                // 데이터 송신
                string echoMessage = $"SERVER MESSAGE : {receivedMessage}"; // 전송할 메세지 생성
                byte[] echoBytes = Encoding.UTF8.GetBytes(echoMessage); // 전송할 메세지를 byte의 배열로 변환

                clientSocket.Send(echoBytes); // 메세지 전송

                return true; // 통신이 성공했음을 알림
            } 
            catch (Exception err)
            {
                Console.WriteLine(err.Message); // 에러 메세지 출력
                return false; // 통신이 실패했음을 알림
            }
        }
    }
}