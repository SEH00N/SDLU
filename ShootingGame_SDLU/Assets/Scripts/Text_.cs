using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Text_ : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 5;
    public EnemySpawn EnemySpawn_;
    float time;
    [SerializeField] GameObject submitControls;

    void Start()
    {
        EnemySpawn_ = FindObjectOfType<EnemySpawn>();
    }

    public void TextMove()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    public void DestroyText()
    {
        time += Time.deltaTime;
        EnemySpawn_.StopAllCoroutines();
        if(time >= 2)
        gameObject.SetActive(false);
    }

    //void OnDestroy()
    //{
    //    EnemySpawn_.StopCoroutine(EnemySpawn_.EnemyCreate());
    //    Application.Quit();
    //}
}
