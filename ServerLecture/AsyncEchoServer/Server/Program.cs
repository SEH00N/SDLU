using System.Net;
using System.Net.Sockets;
using System;

namespace Server
{
    public class Program
    {
        private static Listener listener;

        static void Main(string[] args)
        {
            string host = Dns.GetHostName(); // 해당 로컬 컴퓨터의 DNS 호스트 이름을 불러옴
            IPHostEntry ipHost = Dns.GetHostEntry(host); // DNS를 바탕으로 IP 주소 정보 객체 생성
            IPAddress ipAddress = ipHost.AddressList[1]; // 같은 IP리스트에서 두번째 IP 주소를 뽑음
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081); // 8081 포트로 소켓이 열릴 종단점 생성

            listener = new Listener(endPoint, 10, OnMessageReceived); // 리스너 생성
            listener.StartAccept(); // 소켓 받아들이기 시작

            while(true)
            {
                // 메인 스레드가 꺼지지 않도록 유지
            }
        }

        private static void OnMessageReceived(Socket socket, string msg)
        {
            IPEndPoint clientEndPoint = (socket.RemoteEndPoint as IPEndPoint); // 소켓 종단점 받기

            string message = $"[{clientEndPoint.Address}] {msg}"; // 출력할 메세지 생성 {보낸 IP : 메세지} 형태
            Console.WriteLine(message); // 메세지 출력
            listener.Broadcast(message); // 받은 메세지를 모든 클라이언트들에게 전송

            if (msg.IndexOf("exit") > -1) // 만약 exit 라는 메세지가 포함되어있었으면
                listener.Kick(socket); // 해당 소켓 내보내기
        }
    }
}