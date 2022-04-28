using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    protected virtual void Dead(string name)
    {
        Debug.Log($"{name}사망");
        Destroy(gameObject);
    }
}
