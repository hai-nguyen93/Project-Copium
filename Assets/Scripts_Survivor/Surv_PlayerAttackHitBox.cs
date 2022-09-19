using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerAttackHitBox : MonoBehaviour
{
    public int damage;

    public void SetDamage(int value)
    {
        damage = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        Surv_Enemy enemy = other.GetComponent<Surv_Enemy>();
        if (enemy)
        {
            Debug.Log(enemy.gameObject.name + " takes " + damage + " damage.");
            enemy.Die();
        }
    }
}
