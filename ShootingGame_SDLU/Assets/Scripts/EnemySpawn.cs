using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // Start is called before the first frame update
    float EnemySpawnDelay = 0.5f;
    public GameObject Enemy;
    public Transform MinValue = null;
    public Transform MaxValue = null;
    float randval;
    public GameObject FireEnemy;
    public GameObject bossEnemy;
    public float time;

    public static EnemySpawn Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemySpawn>();
            }
            return instance;
        }
    }

    private static EnemySpawn instance;

    void Start()
    {
            StartMethod();
    }

    private void Update()
    {
        time += Time.deltaTime;
        BossEnemySpawn();
    }

    IEnumerator coroutine;
    public void StartMethod()
    {
        coroutine = EnemyCreate();
        StartCoroutine(coroutine);
    }

    public void StopMethod()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    public IEnumerator EnemyCreate()
    {
        while (true)
        {
            randval = Random.Range(1f, 3f);
            if (randval >= 2)
            {
                Instantiate(Enemy, new Vector2(Random.Range(MinValue.position.x, MaxValue.position.x), MaxValue.position.y), Quaternion.identity);
                yield return new WaitForSeconds(EnemySpawnDelay);
                randval = 0;

            }
            else
            {
                Instantiate(FireEnemy, new Vector2(Random.Range(MinValue.position.x, MaxValue.position.x), MaxValue.position.y), Quaternion.identity);
                yield return new WaitForSeconds(EnemySpawnDelay);
                randval = 0;
            }
        }
    }

    void BossEnemySpawn()
    {
        if (ScoreCount.Instance.score % 15 == 0 && ScoreCount.Instance.score != 0)
        {
            Instantiate(bossEnemy, transform.position, Quaternion.identity);
            ++ScoreCount.Instance.score;
        }
    }
}
