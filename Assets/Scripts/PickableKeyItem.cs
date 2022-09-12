using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableKeyItem : MonoBehaviour
{
    public KeyItem item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInventory pi = PlayerData.Instance.GetComponent<PlayerInventory>();
            pi.AddKeyItem(item);
            Debug.Log("Picker up " + item.iName);

            Destroy(gameObject);
        }
    }
}
