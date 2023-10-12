using System;
using System.Collections.Generic;
using H00N.Network;
using Packets;

public class PacketManager
{
    // 싱글톤패턴
    private static PacketManager instance;
    public static PacketManager Instance
    {
        get
        {
            if (instance == null)
                instance = new PacketManager();
            return instance;
        }
    }

    private Dictionary<ushort, Func<ArraySegment<byte>, Packet>> packetFactories = new Dictionary<ushort, Func<ArraySegment<byte>, Packet>>(); // 패킷 생성자
    private Dictionary<ushort, Action<Session, Packet>> packetHandlers = new Dictionary<ushort, Action<Session, Packet>>(); // 패킷 핸들러


    public PacketManager()
    {
        // 전역변수 초기화
        packetFactories.Clear();
        packetHandlers.Clear();

        RegisterHandler(); // 핸들러 구독
    }

    private void RegisterHandler() // 핸들러 구독
    {
        packetFactories.Add((ushort)PacketID.S_LogInPacket, PacketUtility.CreatePacket<S_LogInPacket>);
        packetHandlers.Add((ushort)PacketID.S_LogInPacket, PacketHandler.S_LogInPacket);
        packetFactories.Add((ushort)PacketID.S_MovePacket, PacketUtility.CreatePacket<S_MovePacket>);
        packetHandlers.Add((ushort)PacketID.S_MovePacket, PacketHandler.S_MovePacket);
        packetFactories.Add((ushort)PacketID.S_PlayerJoinPacket, PacketUtility.CreatePacket<S_PlayerJoinPacket>);
        packetHandlers.Add((ushort)PacketID.S_PlayerJoinPacket, PacketHandler.S_PlayerJoinPacket);
        packetFactories.Add((ushort)PacketID.S_RoomEnterPacket, PacketUtility.CreatePacket<S_RoomEnterPacket>);
        packetHandlers.Add((ushort)PacketID.S_RoomEnterPacket, PacketHandler.S_RoomEnterPacket);
    }

    public Packet CreatePacket(ArraySegment<byte> buffer)
    {
        ushort packetID = PacketUtility.ReadPacketID(buffer);

        if (packetFactories.ContainsKey(packetID))
            return packetFactories[packetID]?.Invoke(buffer);
        else
            return null;
    }

    public void HandlePacket(Session session, Packet packet)
    {
        if(packet != null)
            if (packetHandlers.ContainsKey(packet.ID)) // 핸들러가 존재하면
                packetHandlers[packet.ID]?.Invoke(session, packet); // 패킷 핸들링
    }
}
