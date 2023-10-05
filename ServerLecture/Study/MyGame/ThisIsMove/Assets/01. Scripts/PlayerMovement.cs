using Packets;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;

    private const float SyncDelay = 1 / 100f;
    private const float SyncOffest = 0.5f;
    private float lastSyncTime = 0f;
    private Vector3 lastPosition;

    private void Start()
    {
        C_EnterRoomPacket packet = new C_EnterRoomPacket();
        GameManager.Instance.Send(packet);
    }

	private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = transform.right * h + transform.forward * v;
        transform.Translate(dir.normalized * Time.deltaTime * speed);

        if(lastSyncTime + SyncDelay < Time.time)
        {
            lastSyncTime = Time.time;
            
            if((lastPosition - transform.position).magnitude > SyncOffest)
            {
                if(GameManager.Instance.playerID == ushort.MaxValue)
                    return;

                C_MovePacket movePacket = new C_MovePacket();
                movePacket.playerID = GameManager.Instance.playerID;
                movePacket.x = (ushort)transform.position.x;
                movePacket.y = (ushort)transform.position.y;
                movePacket.z = (ushort)transform.position.z;

                GameManager.Instance.Send(movePacket);

                lastPosition = transform.position;
            }
        }
    }
}
