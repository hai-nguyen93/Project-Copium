using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Surv_ItemBase : MonoBehaviour
{
    public string itemName = "base item";


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Pick up item: " + itemName);
            Destroy(gameObject);
        }
    }
}
