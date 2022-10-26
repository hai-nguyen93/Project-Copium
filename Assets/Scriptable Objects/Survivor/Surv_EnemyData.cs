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
    [Range(0f, 1f)] public float chanceToSpawnHealItem = 0.1f;
    [Range(0f, 1f)] public float chanceToSpawnAtkItem = 0.01f;

    [Header("Bonus stats over time \n(timeToLevelUp <= 0: no increasing stat over time)")]
    public float timeStartToLevelUp = 0f;
    public float timeToLevelUp = 60f;
    public int hpUpAmount = 2;
    public int dmgUpAmount = 1;
    public int expUpAmount = 1;

    public int maxHp {
        get
        {
            if (timeToLevelUp <= 0) return baseMaxHp;
            return baseMaxHp + Mathf.FloorToInt(Mathf.Clamp(Time.time - timeStartToLevelUp, 0, float.MaxValue) / timeToLevelUp) * hpUpAmount;
        }
    }   

    public int damage
    {
        get
        {
            if (timeToLevelUp <= 0) return baseDmg;
            return baseDmg + Mathf.FloorToInt(Mathf.Clamp(Time.time - timeStartToLevelUp, 0, float.MaxValue) / timeToLevelUp) * dmgUpAmount;
        }
    }

    public int exp
    {
        get
        {
            if (timeToLevelUp <= 0) return baseExp;
            return baseExp + Mathf.FloorToInt(Mathf.Clamp(Time.time - timeStartToLevelUp, 0, float.MaxValue) / timeToLevelUp) * expUpAmount;
        }
    }

    private void OnValidate()
    {
        timeStartToLevelUp = Mathf.Abs(timeStartToLevelUp);
    }
}

