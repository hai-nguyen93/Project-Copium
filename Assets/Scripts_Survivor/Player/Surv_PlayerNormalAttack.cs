using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerNormalAttack : Surv_PlayerAttack
{
    public Surv_PlayerAttackHitBox hitBox;
    public float attackDuration = 0.2f;

    protected override void Start()
    {
        base.Start();
        autoAttack = true;
        hitBox.gameObject.SetActive(false);   
    }

    private void Update()
    {
        UpdateAttack();
    }

    public override void UpdateAttack()
    {
        base.UpdateAttack();
    }

    public override void Attack()
    {
        if (hitBox.gameObject.activeSelf) return; // skip if attack is still in progress

        StopCoroutine(AttackCoroutine());
        StartCoroutine(AttackCoroutine());
    }
    public IEnumerator AttackCoroutine()
    {
        damage = baseDamage + pCombat.atk;
        hitBox.SetDamage(damage);
        hitBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        hitBox.gameObject.SetActive(false);

        ResetAttackTimer();
    }
}
