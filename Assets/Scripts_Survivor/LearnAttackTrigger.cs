using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnAttackTrigger : MonoBehaviour
{
    public int attackID;

    public void SetAttackToLearn(int attackID)
    {
        this.attackID = attackID;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Surv_PlayerCombat>(out var pCombat))
        {
            pCombat.LearnAttack(attackID);
            Destroy(gameObject);
        }
    }
}
