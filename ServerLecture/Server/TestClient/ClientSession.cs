using H00N.Network;
using System.Net;

namespace TestClient
{
    public class ClientSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] Client Connected : {endPoint}");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] Client Disconnected : {endPoint}");
        }

        public override void OnPacketReceived(ArraySegment<byte> buffer)
        {
            
        }

        public override void OnSent(int length)
        {
            throw new NotImplementedException();
        }
    }
}
