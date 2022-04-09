using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCount : MonoBehaviour
{
    public static ScoreCount Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreCount>();
            }
            return instance;
        }
    }

    private static ScoreCount instance;

    [SerializeField] GameObject player;
    public Text currentScoreUI;
    public Text bestScoreUI;
    public int bestScore = 1;
    public int score = 2;
    float delay = 0.1f;

    void Start()
    {
        StartCoroutine(GetScore());
    }
        // Update is called once per frame
        void Update()
    {
        
    }

    private IEnumerator GetScore()
    {
        while (true)
        {
            currentScoreUI.text = "Score : "+ score;
            yield return new WaitForSeconds(delay);
        }
    }

    public void Score()
    {
        score++;
    }
}
