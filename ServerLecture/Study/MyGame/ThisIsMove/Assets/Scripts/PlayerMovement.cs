using System.Text;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;

	private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = transform.right * h + transform.forward * v;
        transform.Translate(dir.normalized * Time.deltaTime * speed);
    }
}
