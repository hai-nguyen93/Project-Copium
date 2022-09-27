using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerAttack : MonoBehaviour
{
    protected Surv_PlayerController player;
    protected Surv_PlayerCombat pCombat;

    public float attackCooldown = 1f;
    protected float attackTimer;
    public int baseDamage = 1;
    protected int damage;
    public bool autoAttack;

    protected virtual void Start()
    {
        FindPlayer();
        attackTimer = attackCooldown;
    }

    public virtual void UpdateAttack()
    {
        if (player.isDead) return;

        if (autoAttack)
        {
            if (attackTimer <= 0f)
            {
                Attack();
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }
        }
    }

    public virtual void Attack() { }

    public void FindPlayer()
    {
        player = GetComponentInParent<Surv_PlayerController>();
        pCombat = player.pCombat;
    }
}
