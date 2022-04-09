using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject submitControls;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    private static UIManager instance;

    public Image hpGage;
    //public Text gameOverText;
    public GameObject player;
    private Text_ GameOver;
    float time;

    private void Start()
    {
        GameOver = FindObjectOfType<Text_>();
    }

    public void DecreaseHP()
    {
        hpGage.fillAmount -= 0.2f;
    }

    private void Update()
    {
        if (hpGage.fillAmount <= 0.1f)
        {
            player.SetActive(false);
            //gameOverText.gameObject.SetActive(true);
            GameOver.TextMove();
            GameOver.DestroyText();
            time += Time.deltaTime;
            if(time > 2)
            submitControls.SetActive(true);
        }
    }
}