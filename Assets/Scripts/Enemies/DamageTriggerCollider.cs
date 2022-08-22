using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageTriggerCollider : MonoBehaviour
{
    public Collider2D trigger;
    public int damageValue;

    void Awake()
    {
        trigger = GetComponent<Collider2D>();
        trigger.isTrigger = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //if (PlayerData.Instance.isDead) return;

            Debug.Log(gameObject.name + " hit Player.");
            collision.gameObject.GetComponent<PlayerController>().Damage(damageValue);
        }
    }
}
