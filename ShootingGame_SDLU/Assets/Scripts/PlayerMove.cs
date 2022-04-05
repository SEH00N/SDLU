using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public GameObject Bullet;
    public float FireDelay = 0.5f;
    public float speed = 10f;
    Vector2 max = new Vector2(8.5f, 4.5f);
    Vector2 min = new Vector2(-8.5f, -4.5f);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(BulletFire());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Block();
    }

    private void Move()
    {
        float hori = Input.GetAxisRaw("Horizontal");
        float verti = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(hori, verti).normalized * speed;

    }

    private IEnumerator BulletFire()
    {
        while (true)
        {
            Instantiate(Bullet, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(FireDelay);
        }
    }

    private void Block()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y));
    }
}
