using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_EnemyAutoAttackBase : MonoBehaviour
{
    public bool randomCdTime;
    public float constantCdTime;
    public float minCdTime;
    public float maxCdTime;
    private float timer;

    protected virtual void Start()
    {
        ResetTimer();
    }

    protected virtual void Update()
    {
        UpdateTimer();
    }

    public virtual void UpdateTimer()
    {
        if (timer > 0f) { timer -= Time.deltaTime; }
        else
        {
            Attack();
        }
    }

    public virtual void Attack() { ResetTimer(); }

    public virtual void ResetTimer()
    {
        if (randomCdTime) { timer = Random.Range(minCdTime, maxCdTime); }
        else { timer = constantCdTime; }
    }
}
