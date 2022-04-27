using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIManager : MonoBehaviour
{
    public static TestUIManager Instance //�̱��� ���� get set proeprty �� �� ������Ƽ
    {
        get 
        {
            if(instance == null)
            {
                instance = FindObjectOfType<TestUIManager>();
            }
            return instance;
        }
    }

    private static TestUIManager instance;

    [SerializeField] private string _scorekey = "SCORE";

    public int score = 0;

    public void PlusScore()
    {
        PlayerPrefs.SetInt(_scorekey, score);
        int _score = PlayerPrefs.GetInt(_scorekey, 0);
        score++;
    }
}
