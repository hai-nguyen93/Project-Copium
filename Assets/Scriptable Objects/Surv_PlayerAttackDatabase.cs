using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackDB", menuName = "Survivor/Player Attack DB")]
public class Surv_PlayerAttackDatabase : ScriptableObject
{
    public List<Surv_PlayerAttack> attackList;

    public Surv_PlayerAttack GetRandomPlayerAttack()
    {
        return attackList[Random.Range(0, attackList.Count)];
    }

    public Surv_PlayerAttack GetPlayerAttack(int attackID)
    {
        foreach(var a in attackList)
        {
            if (a.attackID == attackID)
            {
                return a;
            }
        }       

        return attackList[0];
    }
}