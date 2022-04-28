using UnityEngine;

public class PlayerMove : CharacterMove
{
    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float h = Input.GetAxis("Horizontal");

        rb2d.velocity = new Vector2(h * speed, rb2d.velocity.y);
    }

    private void PlayerJump()
    {

    }
}
