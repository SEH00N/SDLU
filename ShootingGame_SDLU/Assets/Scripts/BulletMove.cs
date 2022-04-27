using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public float speed = 15;

    void Update()
    {
        BulletMovement();
        Limit();
    }

    private void BulletMovement()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") || collision.CompareTag("FireEnemy"))
        DeSpawn();
    }

    private void Limit()
    {
        if(transform.position.x > GameManager.Instance.maxPos.position.x ||
           transform.position.y > GameManager.Instance.maxPos.position.y ||
           transform.position.x < GameManager.Instance.minPos.position.x ||
           transform.position.y < GameManager.Instance.minPos.position.y)
        {
            DeSpawn();
        }
    }

    private void DeSpawn()
    {
        transform.SetParent(GameManager.Instance.Pooling);
        gameObject.SetActive(false);
    }
}
