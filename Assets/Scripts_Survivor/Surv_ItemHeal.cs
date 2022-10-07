using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_ItemHeal : Surv_ItemBase
{
    [Tooltip("The relative amount of heal (1 = 100%")]
    [Range(0f, 1f)] public float healAmountNormalized = 0.35f;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Surv_PlayerHP>(out var playerHP))
        {
            playerHP.HealDamage(Mathf.RoundToInt(Random.Range(0.85f, 1.2f) * healAmountNormalized * playerHP.maxHP));
            Destroy(gameObject);
        }
    }
}
