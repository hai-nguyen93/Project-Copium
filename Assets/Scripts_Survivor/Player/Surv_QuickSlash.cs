using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_QuickSlash : Surv_PlayerAttack
{
    public Surv_PlayerAttackHitBox hitBox;
    public Surv_PlayerAttackHitBox hitBox2;
    public float attackDuration = 0.2f;

    protected override void Start()
    {
        base.Start();
        autoAttack = true;
        hitBox.gameObject.SetActive(false);
        hitBox2.gameObject.SetActive(false);
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
        hitBox.SetDamage(damage);
        hitBox.gameObject.SetActive(true);
        if (level >= 3)
        {
            hitBox2.SetDamage(damage);
            hitBox2.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(attackDuration);
        hitBox.gameObject.SetActive(false);
        hitBox2.gameObject.SetActive(false);

        ResetAttackTimer();
    }
}
