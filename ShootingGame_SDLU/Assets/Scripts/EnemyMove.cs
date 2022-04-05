using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 5;
    public UIManager _UIManager;
    private ScoreCount ScoreManager;

    void Start()
    {
        Destroy(gameObject, 2f);
        _UIManager = FindObjectOfType<UIManager>();
        ScoreManager = FindObjectOfType<ScoreCount>();
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
        if(collision.CompareTag("Player") || collision.CompareTag("Bullet"))
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
}
