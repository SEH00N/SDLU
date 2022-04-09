using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerMove>();
            }
            return instance;
        }
    }
    private static PlayerMove instance;

    private Rigidbody2D rb;
    public GameObject bullet;
    public GameObject bullet2;
    public GameObject bullet1;
    public float FireDelay = 0.5f;
    public float speed = 10f;
    Vector2 max = new Vector2(8.5f, 4.5f);
    Vector2 min = new Vector2(-8.5f, -4.5f);
    public int item;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartMethod();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Block();
    }

    private void Move()
    {
        float hori = Input.GetAxisRaw("Horizontal");
        float verti = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(hori, verti).normalized * speed;

    }

    IEnumerator coroutineLeft;

    public void StartMethodLeft()
    {
        coroutineLeft = BulletFireLeft();
        StartCoroutine(coroutineLeft);
    }

    public void StopMethodLeft()
    {
        if (coroutineLeft != null)
        {
            StopCoroutine(coroutineLeft);
        }
    }

    IEnumerator coroutineRight;
    public void StartMethodRight()
    {
        coroutineRight = BulletFireRight();
        StartCoroutine(coroutineRight);
    }

    public void StopMethodRight()
    {
        if (coroutineRight != null)
        {
            StopCoroutine(coroutineRight);
        }
    }

    IEnumerator coroutine;
    public void StartMethod()
    {
        coroutine = BulletFire();
        StartCoroutine(coroutine);
    }

    public void StopMethod()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator BulletFire()
    {
        while (true)
        {
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 0.1f, 0), Quaternion.identity);
            yield return new WaitForSeconds(FireDelay);
        }
    }

    private IEnumerator BulletFireLeft()
    {
        while (true)
        {
            Instantiate(bullet, new Vector3(transform.position.x - 0.5f, transform.position.y + 0.1f, 0), Quaternion.EulerRotation(0, 0, 0.1f));
            yield return new WaitForSeconds(FireDelay);
        }
    }

    private IEnumerator BulletFireRight()
    {
        while (true)
        {
            Instantiate(bullet, new Vector3(transform.position.x + 0.5f, transform.position.y + 0.1f, 0), Quaternion.EulerRotation(0, 0, -0.1f));
            yield return new WaitForSeconds(FireDelay);
        }
    }

    private void Block()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item1"))
        {
            Destroy(collision.gameObject);
            StopMethod();
            StartMethod();
            StartMethodLeft();
            item++;
        }
        else if (collision.CompareTag("Item2"))
        {
            Destroy(collision.gameObject);
            StopMethod();
            StopMethodLeft();
            StartMethod();
            StartMethodLeft();
            StartMethodRight();
            item++;
        }
    }
}
