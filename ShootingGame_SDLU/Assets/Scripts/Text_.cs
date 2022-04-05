using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text_ : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 5;
    public EnemySpawn EnemySpawn_;

    void Start()
    {
        EnemySpawn_ = FindObjectOfType<EnemySpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TextMove()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    public void DestroyText()
    {
        EnemySpawn_.StopAllCoroutines();
        Destroy(gameObject, 5f);
    }

    //void OnDestroy()
    //{
    //    EnemySpawn_.StopCoroutine(EnemySpawn_.EnemyCreate());
    //    Application.Quit();
    //}
}
