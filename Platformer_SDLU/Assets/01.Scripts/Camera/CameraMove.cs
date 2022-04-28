using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMove : MonoBehaviour
{

    private Transform _camTransform = null;

    private PlayerMove _player = null;

    private void Start() {
        _player = FindObjectOfType<PlayerMove>();
        _camTransform = Camera.main.transform;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            _camTransform.position = Vector3.one * 100f;
        }
    }
}   