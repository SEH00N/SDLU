using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => instance;

    private static GameManager instance = null;

    public Transform maxPos;
    public Transform minPos;
    public Transform Pooling { get; private set; }

    private void Awake()
    {
        if(instance == null)
            instance = this;
        Pooling = GameObject.Find("Pooling").transform;
    }
}
