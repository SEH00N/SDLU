using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;

    public bool isFacingRight { get; private set; } = false;

    protected Rigidbody2D rb2d = null;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void ChangeChasing()
    {
        if(rb2d.velocity.x >= 0.1f) //오른쪽 방향
            isFacingRight = true;
        else //왼쪽 방향
            isFacingRight = false;
    }
}
