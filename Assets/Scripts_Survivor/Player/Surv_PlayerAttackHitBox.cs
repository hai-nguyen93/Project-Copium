using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerAttackHitBox : MonoBehaviour
{
    public int damage;
    public bool hitOnce;
    private List<Collider> hitColliders;

    private void OnEnable()
    {
        if (hitColliders == null) hitColliders = new List<Collider>();
        hitColliders.Clear();
    }

    public void SetDamage(int value)
    {
        damage = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        Surv_Enemy enemy = other.GetComponent<Surv_Enemy>();
        if (enemy)
        {
            if (hitOnce)
            {
                if (hitColliders.Contains(other)) return;
                hitColliders.Add(other);
            }
            enemy.ReceiveDamage(damage);        
        }
    }
}
