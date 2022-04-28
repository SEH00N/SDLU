using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;

    public bool IsFacingRight { get; private set; } = false;

    protected Rigidbody2D rb2d = null;

    protected virtual void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void ChangeChasing()
    {
        if(rb2d.velocity.x >= 0.1f) //오른쪽 방향
            IsFacingRight = true;
        else //왼쪽 방향
            IsFacingRight = false;
    }
}
