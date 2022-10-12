using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackDB", menuName = "Survivor/Player Attack DB")]
public class Surv_PlayerAttackDatabase : ScriptableObject
{
    public List<Surv_PlayerAttack> attackList;
    public List<Surv_PlayerAttack> ultList;
}