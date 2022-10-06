using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerAttack : MonoBehaviour
{
    protected Surv_PlayerController player;
    protected Surv_PlayerCombat pCombat;

    [Header("Attack info")]
    public int attackID;
    public string attackName;
    public Sprite attackIcon;

    [Header("Attack Stat")]
    public int level = 1;
    public float baseAtkCD = 1f;
    protected float atkCD;
    protected float attackTimer;
    public float baseDmgPotency = 1;
    protected float dmgPotency;
    public int damage { get => Mathf.CeilToInt((player.pCombat.atk * dmgPotency * Random.Range(0.9f, 1.25f))); }
    public bool autoAttack;

    protected virtual void Start()
    {
        FindPlayer();
        ResetAttackStat();
        attackTimer = atkCD;
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

    public virtual void Attack() { ResetAttackTimer(); }

    public void FindPlayer()
    {
        player = GetComponentInParent<Surv_PlayerController>();
        pCombat = player.GetComponent<Surv_PlayerCombat>();
    }

    public void ResetAttackTimer()
    {
        attackTimer = atkCD;
    }

    public virtual void AttackLevelUp()
    {
        level += 1;
        
        // Upgrade attack (power, cooldown, range, etc.)
    }

    public void ResetAttackStat()
    {
        level = 1;
        atkCD = baseAtkCD;
        dmgPotency = baseDmgPotency;
    }

    public void OnValidate()
    {
        baseDmgPotency = Mathf.Abs(baseDmgPotency);
    }
}
