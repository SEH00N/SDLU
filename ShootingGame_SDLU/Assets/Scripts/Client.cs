using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class Client : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] InputField nicknameInput;
    WebSocket _client;

    private void Start()
    {
        _client = new WebSocket("ws://localhost:8080");

        _client.OnOpen += (sender, e) => {
            Debug.Log("접속 성공!");
        };

        _client.OnMessage += OnMessage;

        _client.ConnectAsync();
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        string result = e.Data.Replace('|', '\n').Replace(",", ": ");
        scoreText.text = result;
    }

    public void SubmitScore()
    {
        string data = "score:" + nicknameInput.text + "," + TestUIManager.Instance.score;
        _client.Send(data);
    }
}