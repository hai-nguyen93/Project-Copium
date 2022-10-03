using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerAttack : MonoBehaviour
{
    protected Surv_PlayerController player;
    protected Surv_PlayerCombat pCombat;

    public int level = 1;
    public float attackCooldown = 1f;
    protected float attackTimer;
    public float dmgPotency = 1;
    public int damage { get => Mathf.CeilToInt((player.pCombat.atk * dmgPotency * Random.Range(0.9f, 1.25f))); }
    public bool autoAttack;

    protected virtual void Start()
    {
        FindPlayer();
        attackTimer = attackCooldown;
    }

    /// <summary>
    /// Count down timer and call Attack() when timer <= 0
    /// </summary>
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
        pCombat = player.GetComponent<Surv_PlayerCombat>();
    }

    public void ResetAttackTimer()
    {
        attackTimer = attackCooldown;
    }

    public virtual void AttackLevelUp()
    {
        level += 1;
        
        // Upgrade attack (power, cooldown, range, etc.)
    }

    public void OnValidate()
    {
        dmgPotency = Mathf.Abs(dmgPotency);
    }
}
