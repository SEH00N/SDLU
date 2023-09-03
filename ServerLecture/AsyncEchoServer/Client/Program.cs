using System;
using System.Net;

namespace Client
{
    public class Program
    {
        private static Connector connector; // 서버와 통신하는 주체

        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("172.31.1.175"); // 접속할 IP
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 8081); // 접속할 종단점 생성

            connector = new Connector(endPoint, OnMessageReceived); // 서버와 통신하는 주체 Connsector 생성
            connector.Connect(endPoint); // 서버와 연결

            while(true)
            {
                string message = Console.ReadLine(); // 보낼 메세지 입력받기
                connector.Send(message); // 메세지 전송

                if (message.IndexOf("exit") > -1) // 만약 메세지에 exit 가 포함되어있으면
                {
                    connector.Disconnect(); // 접속 종료
                    break; // 루프 끝내기
                }
            }

            Console.WriteLine("클라이언트 종료.");
        }

        private static void OnMessageReceived(string msg)
        {
            Console.WriteLine(msg); // 수신한 메세지 출력
        }
    }
}