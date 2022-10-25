using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_EnergyBomb : Surv_PlayerAttack
{
    [Header("Bomb settings")]
    public Surv_Bomb pfBomb;
    public List<float> radiusAtLevel;

    private void Update()
    {
        base.UpdateAttack();
    }

    public override void Attack()
    {
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
        Surv_Bomb b = Instantiate(pfBomb, pos, Quaternion.identity);
        b.SetDamage(damage);
        

        ResetAttackTimer();
    }

    public override void OnValidate()
    {
        base.OnValidate();

        if (radiusAtLevel.Count < ATTACK_MAX_LV)
        {
            for (int i = radiusAtLevel.Count; i < ATTACK_MAX_LV; ++i)
            {
                radiusAtLevel.Add(2.5f);
            }
        }
    }
}
