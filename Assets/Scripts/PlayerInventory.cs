using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<KeyItem> inventory_keyItems;

    public void AddKeyItem(KeyItem item)
    {
        if (!inventory_keyItems.Contains(item))
        {
            inventory_keyItems.Add(item);
            return;
        }
        Debug.Log("Player already has " + item.iName);
    }

    public void RemoveKeyItem(KeyItem item)
    {
        if (inventory_keyItems.Contains(item))
        {
            inventory_keyItems.Remove(item);
            return;
        }
        Debug.Log("Player does NOT have " + item.iName);
    }

    public bool HasKeyItem(KeyItem item)
    {
        return inventory_keyItems.Contains(item);
    }

    // Clear duplicate key items
    [ContextMenu("Remove Duplicate Key Items")]
    public void RemoveDuplicateKeyItems()
    {
        inventory_keyItems = inventory_keyItems.Distinct<KeyItem>().ToList();
    }
}
