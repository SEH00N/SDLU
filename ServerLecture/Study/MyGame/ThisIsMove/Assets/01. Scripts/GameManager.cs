using System.Net;
using System.Collections.Generic;
using H00N.Network;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
    public static GameManager Instance {
        get {
            if(instance == null)
                instance = GameObject.Find("GameManager").GetComponent<GameManager>();
            return instance;
        }
    }

    [SerializeField] GameObject playerPrefab;
    public ushort playerID = ushort.MaxValue;
    public ServerSession session = null;
    private Connector connector;

    public Queue<Tuple<Session, ArraySegment<byte>>> packetQueue = new Queue<Tuple<Session, ArraySegment<byte>>>();
    private object locker = new object();

    private float lastFlushTime = 0f;

    public Dictionary<ushort, Transform> playerList = new Dictionary<ushort, Transform>();
    public Transform this[ushort id] {
        get {
            if(playerList.ContainsKey(id))
                return playerList[id];
            else
                return null;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        session = new ServerSession();

        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("172.31.1.175"), 8081);    

        connector = new Connector(endPoint, session);
        connector.StartConnect(endPoint);
    }

    public void PushPacket(Session session, ArraySegment<byte> buffer)
    {
        lock(locker)
            packetQueue.Enqueue(new Tuple<Session, ArraySegment<byte>>(session, buffer));
    }

    private void Update()
    {
        lock(locker)
        {
            while(packetQueue.Count > 0)
            {
                Tuple<Session, ArraySegment<byte>> packetData = packetQueue.Dequeue();
                Debug.Log(packetData.Item2.Count);
                Debug.Log((Packets.PacketID)PacketUtility.ReadPacketID(packetData.Item2.Array));
                PacketManager.Instance.HandlePacket(packetData.Item1, packetData.Item2);
            }
        }
    }

    public void AddPlayer(ushort id)
    {
        if(playerList.ContainsKey(id))
            return;

        GameObject otherPlayer = Instantiate(playerPrefab);
        playerList.Add(id, otherPlayer.transform);
    }

    public void Send(Packet packet)
    {
        if(session == null)
            return;

        session.Send(packet.Serialize());
    }
}
