using System.Collections.Generic;
using System.Net;
using H00N.Network;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance = null;

    private ServerSession session = null;
    private Connector connector = null;

    private Queue<Packet> packetQueue = new Queue<Packet>();
    private object locker = new object();

    public bool IsConnect = false;

    private void Awake()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("172.30.1.52"), 8081);
        session = new ServerSession();

        connector = new Connector(endPoint, session);
        connector.StartConnect(endPoint);
    }

    private void Update()
    {
        if(IsConnect == false)
            return;

        while(true)
        {
            lock(locker)
            {
                if(packetQueue.Count <= 0)
                    break;

                Packet packet = packetQueue.Dequeue();
                PacketManager.Instance.HandlePacket(session, packet);
            }
        }
    }

    private void OnDestroy()
    {
        if(IsConnect == true)
        {
            session.Close();
        }
    }

    public void Send(Packet packet)
    {
        session.Send(packet.Serialize());
    }

    public void PushPacket(Packet packet)
    {
        lock(locker)
            packetQueue.Enqueue(packet);
    }
}
