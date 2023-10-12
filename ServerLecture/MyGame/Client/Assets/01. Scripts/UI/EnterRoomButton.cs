using Packets;
using UnityEngine;

public class EnterRoomButton : MonoBehaviour
{
	public void EnterRoom()
    {
        if(GameManager.Instance.PlayerID < 0)
            return;

        C_RoomEnterPacket packet = new C_RoomEnterPacket();
        packet.playerID = (ushort)GameManager.Instance.PlayerID;

        NetworkManager.Instance.Send(packet);
    }
}
