using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : EnemyBulletMove
{
    new void Start()
    {
        randVal = Random.Range(-0.5f, 0.5f);
        Destroy(gameObject, 3f);
        GameObject target = GameObject.Find("Player");
        _UIManager = FindObjectOfType<UIManager>();
        dir = target.transform.position - transform.position;
        dir = new Vector2(dir.x + randVal, dir.y + randVal).normalized;
    }
}
