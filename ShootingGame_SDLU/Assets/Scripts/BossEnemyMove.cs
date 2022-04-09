using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyMove : MonoBehaviour
{
    public static BossEnemyMove Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BossEnemyMove>();
            }
            return instance;
        }
    }

    private static BossEnemyMove instance;

    float speed = 5;
    float time;
    Rigidbody2D rb;
    public int hp;
    public GameObject bullet;
    public GameObject item1;
    public GameObject item2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        EnemySpawn.Instance.StopMethod();
        StartCoroutine(FireBullet());
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        BossEnemyMovement();
        DestroyBoss();
    }

    void BossEnemyMovement()
    {
        rb.velocity = new Vector3(0, -1, 0) * speed;
        if (time > 0.5f)
            rb.velocity = new Vector3(0, 0, 0) * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.DecreaseHP();
        }
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            hp--;
        }
    }

    void DestroyBoss()
    {
        if(hp <= 0)
        {
            Destroy(gameObject);
            EnemySpawn.Instance.StartMethod();
            if(PlayerMove.Instance.item == 0)
            {
                Instantiate(item1, transform.position, Quaternion.identity);
            }
            if(PlayerMove.Instance.item == 1)
            {
                Instantiate(item2, transform.position, Quaternion.identity);
            }
        }
    }

    IEnumerator FireBullet()
    {
        while (true)
        {
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 0.5f, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
