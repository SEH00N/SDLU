using System;
using System.Collections.Generic;
using H00N.Network;
using Packets;
using UnityEngine;

public class PacketManager
{
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

    private Dictionary<ushort, Func<ArraySegment<byte>, Packet>> packetFactories = new Dictionary<ushort, Func<ArraySegment<byte>, Packet>>();
    private Dictionary<ushort, Action<Session, Packet>> packetHandlers = new Dictionary<ushort, Action<Session, Packet>>();

    public PacketManager()
    {
        packetFactories.Clear();
        packetHandlers.Clear();

        RegisterHandler();
    }

    private void RegisterHandler()
    {
        packetFactories.Add((ushort)PacketID.S_PlayerJoinPacket, PacketUtility.CreatePacket<S_PlayerJoinPacket>);
        packetHandlers.Add((ushort)PacketID.S_PlayerJoinPacket, PacketHandler.S_PlayerJoinPacket);
        
        packetFactories.Add((ushort)PacketID.S_RoomEnterResponsePacket, PacketUtility.CreatePacket<S_RoomEnterResponsePacket>);
        packetHandlers.Add((ushort)PacketID.S_RoomEnterResponsePacket, PacketHandler.S_RoomEnterResponsePacket);

        packetFactories.Add((ushort)PacketID.S_MovePacket, PacketUtility.CreatePacket<S_MovePacket>);
        packetHandlers.Add((ushort)PacketID.S_MovePacket, PacketHandler.S_MovePacket);

        packetFactories.Add((ushort)PacketID.S_PlayerListPacket, PacketUtility.CreatePacket<S_PlayerListPacket>);
        packetHandlers.Add((ushort)PacketID.S_PlayerListPacket, PacketHandler.S_PlayerListPacket);
    }

    public void HandlePacket(Session session, ArraySegment<byte> buffer)
    {
        ushort packetID = PacketUtility.ReadPacketID(buffer);

        if (packetFactories.ContainsKey(packetID))
        {
            Packet packet = packetFactories[packetID]?.Invoke(buffer);
            if (packetHandlers.ContainsKey(packetID))
                packetHandlers[packetID]?.Invoke(session, packet);
        }
    }
}