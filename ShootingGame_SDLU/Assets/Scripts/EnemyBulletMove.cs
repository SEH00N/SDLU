using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletMove : MonoBehaviour
{
    public float speed = 5;
    public Vector2 dir;
    public UIManager _UIManager;
    public float randVal;

    protected void Start()
    {
        randVal = Random.Range(-0.5f, 0.5f);
        Destroy(gameObject, 3f);
        GameObject target = GameObject.Find("Player");
        _UIManager = FindObjectOfType<UIManager>();
        dir = target.transform.position - transform.position;
        dir.Normalize();
        //dir = new Vector2(dir.x + randVal, dir.y + randVal);
    }

    // Update is called once per frame
    void Update()
    {
        EnemyBulletMovement();
    }

    protected void EnemyBulletMovement()
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
