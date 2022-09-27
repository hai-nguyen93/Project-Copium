using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerCombat : MonoBehaviour
{
    private Surv_PlayerController player;

    public int atk = 0;
    public Surv_PlayerNormalAttack normalAttack;
    public List<Surv_PlayerAttack> attackList;

    private void Start()
    {
        attackList = new List<Surv_PlayerAttack>();
        attackList.Add(normalAttack);
    }

    public void AddAttack(Surv_PlayerAttack newAttack)
    {
        if (attackList.Contains(newAttack))
        {
            Debug.Log("Player already has " + newAttack.name);
            return;
        }

        // Generate newAttack GameObject then attach to player
        attackList.Add(newAttack);
    }
}
