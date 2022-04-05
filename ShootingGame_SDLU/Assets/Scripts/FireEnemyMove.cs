using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemyMove : MonoBehaviour
{
    public float speed = 5;
    public UIManager _UIManager;
    private ScoreCount ScoreManager;
    public float EnemyFireDelay;
    public GameObject EnemyBullet;

    void Start()
    {
        _UIManager = FindObjectOfType<UIManager>();
        ScoreManager = FindObjectOfType<ScoreCount>();
        StartCoroutine(EnemyFire());
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }

        if (collision.CompareTag("Player"))
        {
            _UIManager.DecreaseHP();
        }

        if (collision.CompareTag("Bullet"))
        {
            ScoreManager.Score();
        }
    }

    private IEnumerator EnemyFire()
    {
        while (true)
        {
            EnemyFireDelay = Random.Range(1.5f, 2f);
            Instantiate(EnemyBullet, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(EnemyFireDelay) ;
        }
    }
}
