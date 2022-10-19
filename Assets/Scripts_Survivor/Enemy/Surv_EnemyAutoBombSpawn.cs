using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_EnemyAutoBombSpawn : Surv_EnemyAutoAttackBase
{
    public Surv_Bomb pfBomb;
    public Surv_Enemy user;

    public override void Attack()
    {
        var b = Instantiate(pfBomb, transform.position, Quaternion.identity);
        b.SetDamage(user.damage);
        base.Attack();
    }
}
