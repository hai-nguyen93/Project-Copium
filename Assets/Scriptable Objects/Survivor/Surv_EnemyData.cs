using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSurvivorEnemy", menuName = "Survivor/Enemy")]
public class Surv_EnemyData : ScriptableObject
{
    public string enemyName = "New Enemy";
    public int baseMaxHp = 1;
    public int baseDmg = 1;
    public float baseSpeed = 1.5f;
    public bool canBeCC = true;
    public int baseExp = 1;

    [Header("Bonus stats over time")]
    public float timeToLevelUp = 60f;
    public int hpUpAmount = 2;
    public int dmgUpAmount = 1;
    public int expUpAmount = 1;

    public int maxHp { get => baseMaxHp + Mathf.FloorToInt(Time.time / timeToLevelUp) * hpUpAmount; }   
    public int damage { get => baseDmg + Mathf.FloorToInt(Time.time / timeToLevelUp) * dmgUpAmount; } 
    public int exp { get => baseExp + Mathf.FloorToInt(Time.time / timeToLevelUp) * expUpAmount; }
}

