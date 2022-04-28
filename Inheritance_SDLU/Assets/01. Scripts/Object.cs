using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : EntityBase
{
    [SerializeField] private string objectName = null;

    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            Dead(objectName);
    }
}
