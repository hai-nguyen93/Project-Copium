using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Surv_Enemy))]
public class Surv_EnemySpawnPuddleOnDie : MonoBehaviour
{
    public Surv_DamageAreaSphere pfPuddle;
    private Surv_Enemy enemy;

    [Header("Puddle Settings")]
    public float dotDuration = 5f;
    public float slowDuration = 1f;
    public float speedModifier = 0.75f;

    private void Start()
    {
        enemy = GetComponent<Surv_Enemy>();
        enemy.OnEnemyDie += SpawnPuddle;
    }

    private void OnDestroy()
    {
        if (enemy) enemy.OnEnemyDie -= SpawnPuddle;
    }

    private void SpawnPuddle(Surv_Enemy enemy)
    {
        Surv_DamageAreaSphere puddle = Instantiate(pfPuddle, transform.position, Quaternion.identity);
        puddle.damage = enemy.damage;
        puddle.dmgPerTick = Mathf.FloorToInt(Mathf.Clamp(enemy.damage * 0.66f, 1f, float.MaxValue));
        puddle.dotDuration = dotDuration;
        puddle.slowDuration = slowDuration;
        puddle.speedModifier = speedModifier;
    }
}
