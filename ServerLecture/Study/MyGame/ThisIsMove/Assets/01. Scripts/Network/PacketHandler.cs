using System;
using H00N.Network;
using Packets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PacketHandler
{
    public static void S_PlayerJoinPacket(Session session, Packet packet)
    {
        S_PlayerJoinPacket joinPacket = packet as S_PlayerJoinPacket;
        ServerSession serverSession = session as ServerSession;

        GameManager.Instance.AddPlayer(joinPacket.playerID);
    }

    public static void S_MovePacket(Session session, Packet packet)
    {
        S_MovePacket movePacket = packet as S_MovePacket;
        ServerSession serverSession = session as ServerSession;

        Transform player = GameManager.Instance[movePacket.playerID];
        if (player != null)
        {
            Vector3 pos = new Vector3(movePacket.x, movePacket.y, movePacket.z);
            player.position = pos;
        }
    }

    public static void S_PlayerListPacket(Session session, Packet packet)
    {
        S_PlayerListPacket playerListPacket = packet as S_PlayerListPacket;
        ServerSession serverSession = session as ServerSession;

        playerListPacket.playerList.ForEach(player =>
        {
            GameManager.Instance.AddPlayer(player);
        });
    }

    public static void S_RoomEnterResponsePacket(Session session, Packet packet)
    {
        S_RoomEnterResponsePacket resPacket = packet as S_RoomEnterResponsePacket;
        GameManager.Instance.playerID = resPacket.playerID;

        SceneManager.LoadScene("InGameScene");
    }
}
