using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerAttack : MonoBehaviour
{
    public const int ATTACK_MAX_LV = 5;

    protected Surv_PlayerController player;
    protected Surv_PlayerCombat pCombat;

    [Header("Attack info")]
    public int attackID;
    public string attackName;
    public Sprite attackIcon;

    [Header("Base stats at each level (x=cooldown; y=dmgPotency")]
    public List<Vector2> statsAtLevel;

    [Header("Attack Stat")]
    public int level = 1;
    public float baseAtkCD { get => statsAtLevel[Mathf.Min(level, ATTACK_MAX_LV) - 1].x; }
    protected float attackTimer;

    public float baseDmgPotency { get => statsAtLevel[Mathf.Min(level, ATTACK_MAX_LV) - 1].y; }
    public int damage { get => Mathf.CeilToInt((player.pCombat.atk * baseDmgPotency * Random.Range(0.9f, 1.25f))); }
    public bool autoAttack;

    protected virtual void Start()
    {
        FindPlayer();
        ResetAttackLevel();
        attackTimer = baseAtkCD;
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
        attackTimer = baseAtkCD;
    }

    public virtual void AttackLevelUp()
    {
        level = Mathf.Clamp(level + 1, 1, ATTACK_MAX_LV);
    }

    public void ResetAttackLevel()
    {
        level = 1;
    }

    public virtual void OnValidate()
    {    
        // Resize stats at each level list
        if (statsAtLevel.Count < ATTACK_MAX_LV)
        {
            for (int i = statsAtLevel.Count; i < ATTACK_MAX_LV; ++i)
            {
                statsAtLevel.Add(Vector2.one);
            }
        }
    }
}
