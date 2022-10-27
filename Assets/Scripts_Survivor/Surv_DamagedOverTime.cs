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

    private void Awake()
    {
        timer = 9999f;
        tickTimer = 9999f;
    }

    private void Update()
    {
        if (timer<= 0f) { Destroy(this); return; }
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
        if (TryGetComponent<IDamageable>(out var target))
        {
            target.ReceiveDamage(damagePerTick);
        }
    }

    public void Setup(int dmgPerTick, float duration, float tickDuration=1f)
    {
        this.damagePerTick = dmgPerTick;
        this.duration = duration;
        this.tickDuration = tickDuration;

        timer = duration;
        tickTimer = tickDuration;
    }

    [ContextMenu("Reset DoT timer")]
    public void ResetTimer()
    {
        timer = duration;
        tickTimer = tickDuration;
    }
}
