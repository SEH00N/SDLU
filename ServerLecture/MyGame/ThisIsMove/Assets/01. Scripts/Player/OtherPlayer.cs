using Packets;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
	public void SetPosition(PlayerPacket playerData)
    {
        Vector3 pos = transform.position;
        pos.x = playerData.x;
        pos.y = playerData.y;
        pos.z = playerData.z;

        transform.position = pos;
    }
}
