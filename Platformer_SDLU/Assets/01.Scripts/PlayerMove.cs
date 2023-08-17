using System;
using System.Collections;
using System.Timers;
using UnityEngine;

public class PlayerMove : CharacterMove
{
    [Flags]
    enum PlayerState
    {
        NONE = 0,
        MOVE = 1 << 0,
        JUMP = 1 << 1,
        DASH = 1 << 2,
    }

    private PlayerState playerState = PlayerState.NONE;

    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float moveSmooth = 5f;
    [SerializeField] float dashSpeed = 20f;
    [SerializeField] float dashDuration = 0.3f;
    [SerializeField] LayerMask groundLayer;

    private Collider2D col;

    protected override void Start()
    {
        base.Start();
        col = GetComponent<Collider2D>();
    }

    protected override void Update()
    {
        base.Update();
        PlayerMovement();
        PlayerJump();
        ReadyToDash();
    }

    private void PlayerMovement()
    {
        if(playerState.HasFlag(PlayerState.DASH)) return;
        playerState |= PlayerState.MOVE;

        float h = Input.GetAxisRaw("Horizontal");

        rb2d.velocity = new Vector2(Mathf.Lerp(rb2d.velocity.x, h * speed, moveSmooth * Time.deltaTime), rb2d.velocity.y);
    }

    private void PlayerJump()
    {
        if(Input.GetButtonDown("Jump") && IsGround())
        {
            playerState |= PlayerState.JUMP;
            rb2d.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }

    private void ReadyToDash()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && Mathf.Abs(rb2d.velocity.x) >= 0)
            StartCoroutine(Dash(dashDuration));
    }

    private IEnumerator Dash(float duration)
    {
        playerState |= PlayerState.DASH;
        rb2d.velocity = new Vector2(IsFacingRight ? dashSpeed : -dashSpeed, rb2d.velocity.y);
        yield return new WaitForSeconds(duration);
        playerState &= ~PlayerState.DASH;
    }

    private bool IsGround()
    {
        return Physics2D.OverlapBox(transform.position, col.bounds.size, 0f, groundLayer);
    }
}
