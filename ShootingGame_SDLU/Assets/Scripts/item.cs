using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public float speed = 0;

    private void Update()
    {
        ItemMove();
    }

    void ItemMove()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (BossEnemyMove.Instance.itemCount >= 1 && BossEnemyMove.Instance.itemCount < 2 && collision.CompareTag("Player"))
    //    {
    //        Destroy(gameObject);
    //        PlayerMove.Instance.StartMethodLeft();
    //    }
    //    if (BossEnemyMove.Instance.itemCount >= 2 && BossEnemyMove.Instance.itemCount < 3 && collision.CompareTag("Player"))
    //    {
    //        Destroy(gameObject);
    //        PlayerMove.Instance.StartMethodRight();
    //    }
    //}
}
