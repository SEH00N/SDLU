using System.Timers;
using UnityEngine;

public class PlayerMove : CharacterMove
{
    [SerializeField] private float jumpSpeed = 10f;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float moveSmooth = 5f;

    private Collider2D col;

    protected override void Start()
    {
        base.Start();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        PlayerMovement();
        PlayerJump();
    }

    private void PlayerMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");

        rb2d.velocity = new Vector2(Mathf.Lerp(rb2d.velocity.x, h * speed, moveSmooth * Time.deltaTime), rb2d.velocity.y);
    }

    private void PlayerJump()
    {
        if(Input.GetButtonDown("Jump") && isGround())
        {
            rb2d.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }

    private bool isGround()
    {
        return Physics2D.OverlapBox(transform.position, col.bounds.size, 0f, groundLayer);
    }
}
