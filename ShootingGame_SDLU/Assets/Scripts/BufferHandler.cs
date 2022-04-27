using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BufferHandler : MonoBehaviour
{

    public static Queue<string> _textQueue = new Queue<string>();

    [SerializeField] Text _scores = null;

    public void Add(string data)
    {
        _textQueue.Enqueue(data);
    }

    private void Update()
    {
        if(_textQueue.Count > 0)
        {
            _scores.text = _textQueue.Dequeue();
        }
    }

}
