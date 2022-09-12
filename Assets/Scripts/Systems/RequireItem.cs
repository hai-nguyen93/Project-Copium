using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RequireItem : MonoBehaviour
{
    public KeyItem[] itemsRequired;
    public UnityEvent eventsToInvoke;
    private PlayerInventory pInventory;

    private void Start()
    {
        pInventory = PlayerData.Instance.GetComponent<PlayerInventory>();
    }

    public bool CheckRequiredItems()
    {
        bool hasItems = true;
        foreach(var item in itemsRequired)
        {        
            if (!pInventory.HasKeyItem(item))
            {
                hasItems = false;
                break;
            }
        }

        if (hasItems)
        {
            InvokeEvents();
        }

        return hasItems;
    }

    public void InvokeEvents()
    {
        if (eventsToInvoke != null)
        {
            eventsToInvoke.Invoke();
        }
        else
        {
            Debug.Log( gameObject.transform.root.name + " has NO events to invoke");
        }
    }

    public string printItems()
    {
        string result = "";
        foreach(var item in itemsRequired)
        {
            result += (item.iName + ", ");
        }
        return result;
    }
}
