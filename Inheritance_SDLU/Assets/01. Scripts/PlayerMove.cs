using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;

    private Collider2D col2d = null;

    private Rigidbody2D rb2d = null;

    private float speed = 10f;

    private float jumpPwr = 12f;
    
    void Start()
    {
        col2d = GetComponent<Collider2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        PlayerJump();
    }

    private void PlayerMovement()
    {
        float h = Input.GetAxis("Horizontal");

        rb2d.velocity = new Vector2(h * speed, rb2d.velocity.y);
    }

    private void PlayerJump()
    {
        if(Input.GetButtonDown("Jump") && isGround())
            rb2d.AddForce(Vector2.up * jumpPwr, ForceMode2D.Impulse);
    }

    private bool isGround()
    {
        return Physics2D.OverlapBox(transform.position, col2d.bounds.size, 0f, groundLayer);
    }
}
