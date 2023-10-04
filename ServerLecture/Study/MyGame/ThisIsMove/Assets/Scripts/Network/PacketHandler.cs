using System;
using H00N.Network;
using Packets;

public class PacketHandler
{
    public static void S_PlayerJoinPacket(Session session, Packet packet)
    {
        S_PlayerJoinPacket joinPacket = packet as S_PlayerJoinPacket;
        ServerSession serverSession = session as ServerSession;
    }

    public static void S_MovePacket(Session session, Packet packet)
    {
        S_MovePacket movePacket = packet as S_MovePacket;
        ServerSession serverSession = session as ServerSession;
    }

    public static void S_PlayerListPacket(Session session, Packet packet)
    {
        S_PlayerListPacket playerListPacket = packet as S_PlayerListPacket;
        ServerSession serverSession = session as ServerSession;
    }
}
