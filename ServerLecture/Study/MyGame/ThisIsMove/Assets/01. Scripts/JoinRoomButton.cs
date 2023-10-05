using Packets;
using UnityEngine;

public class JoinRoomButton : MonoBehaviour
{
	public void Join()
    {
        C_RoomEnterRequestPacket packet = new C_RoomEnterRequestPacket();
        GameManager.Instance.Send(packet);
    }
}
