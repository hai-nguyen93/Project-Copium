using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Survivor Enemy", menuName = "Survivor/Enemy")]
public class Surv_EnemyData : ScriptableObject
{
    public string enemyName = "New Enemy";
    public int maxHp = 1;
    public int damage = 1;
    public float baseSpeed = 1.5f;
    public int exp = 1;
}

