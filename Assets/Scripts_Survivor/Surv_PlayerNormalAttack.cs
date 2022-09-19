using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerNormalAttack : MonoBehaviour
{
    private Surv_PlayerController player;

    public float attackCooldown = 1f;
    public float attackDuration = 0.25f;
    private float attackTimer;
    public int damage = 1;
    public Surv_PlayerAttackHitBox hitBox;

    private void Start()
    {
        FindPlayer();
        attackTimer = attackCooldown;
        hitBox.gameObject.SetActive(false);   
    }

    private void Update()
    {
        if (player.isDead) return;

        if (attackTimer <= 0f )
        {
            if (!hitBox.gameObject.activeSelf) Attack();
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        StopCoroutine(AttackCoroutine());
        StartCoroutine(AttackCoroutine());
    }
    public IEnumerator AttackCoroutine()
    {
        hitBox.SetDamage(damage);
        hitBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        hitBox.gameObject.SetActive(false);

        attackTimer = attackCooldown;
    }

    public void FindPlayer()
    {
        player = GetComponentInParent<Surv_PlayerController>();
    }
}
