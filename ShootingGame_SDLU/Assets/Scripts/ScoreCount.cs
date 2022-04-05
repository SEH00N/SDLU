using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreCount : MonoBehaviour
{
    public Text currentScoreUI;
    public int score = 0;
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
