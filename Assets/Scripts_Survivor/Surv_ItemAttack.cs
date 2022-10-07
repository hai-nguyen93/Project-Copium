using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_ItemAttack : Surv_ItemBase
{
    public bool ultAtk;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!ultAtk)
            {
                Surv_GameController.Instance.PlayerAtkUp();
            }
        }
        Destroy(gameObject);
    }
}
