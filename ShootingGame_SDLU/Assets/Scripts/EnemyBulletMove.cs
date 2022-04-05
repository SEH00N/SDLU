using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletMove : MonoBehaviour
{
    public float speed = 5;
    Vector2 dir;
    public UIManager _UIManager;

    void Start()
    {
        Destroy(gameObject, 3f);
        GameObject target = GameObject.Find("Player");
        _UIManager = FindObjectOfType<UIManager>();
        dir = target.transform.position - transform.position;
        dir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyBulletMovement();
    }

    private void EnemyBulletMovement()
    {
        transform.Translate(new Vector2(dir.x, dir.y) * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _UIManager.DecreaseHP();
            Destroy(gameObject);
        }

        if (collision.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
