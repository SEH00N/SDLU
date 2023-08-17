using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : EntityBase
{
    [SerializeField] private string objectName = null;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Dead(objectName);
    }
}
