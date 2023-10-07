using Packets;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    [SerializeField] float syncDelay = 0.05f;
    [SerializeField] float syncDistanceErr = 0.1f;
    private float lastSyncTime = 0f;
    private Vector3 lastSyncPosition = Vector3.zero;

	private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = transform.forward * v + transform.right * h;
        transform.position += dir.normalized * Time.deltaTime * speed;
    }

    private void LateUpdate()
    {
        if(lastSyncTime + syncDelay > Time.time)
            return;

        if((lastSyncPosition - transform.position).sqrMagnitude < syncDistanceErr * syncDistanceErr)
            return;

        PlayerPacket playerData = new PlayerPacket();
        playerData.playerID = (ushort)GameManager.Instance.PlayerID;
        playerData.x = transform.position.x;
        playerData.y = transform.position.y;
        playerData.z = transform.position.z;

        C_MovePacket packet = new C_MovePacket();
        packet.playerData = playerData;

        NetworkManager.Instance.Send(packet);

        lastSyncPosition = transform.position;
        lastSyncTime = Time.time;
    }
}
