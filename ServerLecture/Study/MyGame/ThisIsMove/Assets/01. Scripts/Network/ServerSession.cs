using System;
using System.Net;
using H00N.Network;
using UnityEngine;

public class ServerSession : Session
{
    public override void OnConnected(EndPoint endPoint)
    {
        Debug.Log($"{endPoint} 들어옴");
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
        Debug.Log($"{endPoint} 나감");
    }

    public override void OnPacketReceive(ArraySegment<byte> buffer)
    {
        Debug.Log($"{buffer.Count}만큼 받음");
        GameManager.Instance.PushPacket(this, buffer);
    }

    public override void OnSent(int length)
    {
        Debug.Log($"{length}만큼 보냄");
    }
}
