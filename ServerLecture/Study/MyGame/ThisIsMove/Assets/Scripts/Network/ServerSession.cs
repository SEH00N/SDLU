using System;
using System.Net;
using H00N.Network;
using Packets;

public class ServerSession : Session
{
    public override void OnConnected(EndPoint endPoint)
    {
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
    }

    public override void OnPacketReceive(ArraySegment<byte> buffer)
    {
        PacketManager.Instance.HandlePacket(this, buffer);
    }

    public override void OnSent(int length)
    {
    }
}
