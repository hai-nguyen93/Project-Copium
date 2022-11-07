using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_DamagedOverTime : MonoBehaviour
{
    public int damagePerTick = 1;
    public float tickDuration = 1f;
    public float duration = 5f;
    private float timer;
    private float tickTimer;
    private IDamageable target;

    private void Awake()
    {
        timer = 5f;
        tickTimer = 1f;
    }

    private void Update()
    {
        if (timer <= 0f || target == null) { Destroy(this); return; }
        if (tickTimer <= 0f)
        {
            ApplyDamage();
            tickTimer = tickDuration;
        }

        timer -= Time.deltaTime;
        tickTimer -= Time.deltaTime;
    }

    private void ApplyDamage()
    {
        target.ReceiveDamage(damagePerTick);
    }

    public void Setup(int dmgPerTick, float duration, float tickDuration=1f)
    {
        this.damagePerTick = dmgPerTick;
        this.duration = duration;
        this.tickDuration = tickDuration;
        target = GetComponent<IDamageable>();

        ResetTimer();
    }

    public void Setup(IDamageable target, int dmgPerTick, float duration, float tickDuration = 1f)
    {
        this.damagePerTick = dmgPerTick;
        this.duration = duration;
        this.tickDuration = tickDuration;
        this.target = target;

        ResetTimer();
    }

    [ContextMenu("Reset DoT timer")]
    public void ResetTimer()
    {
        timer = duration;
        tickTimer = tickDuration;
    }
}
