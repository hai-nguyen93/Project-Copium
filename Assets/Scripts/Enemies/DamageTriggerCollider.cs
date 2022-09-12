using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTriggerCollider : MonoBehaviour
{
    public Collider2D trigger;
    public int damageValue;
    public bool playerSide = false;

    void Start()
    {
        if (trigger == null) trigger = GetComponent<Collider2D>();
        trigger.isTrigger = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerSide)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //if (PlayerData.Instance.isDead) return;

                Debug.Log(gameObject.transform.root.name + " hit Player.");
                collision.gameObject.GetComponent<PlayerController>().Damage(damageValue);
            }
        }
        else
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log(gameObject.transform.root + " hit " + collision.gameObject.name);
            }
        }
    }
}
