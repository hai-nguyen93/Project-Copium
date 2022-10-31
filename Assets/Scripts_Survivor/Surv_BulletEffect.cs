using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_BulletEffect : MonoBehaviour
{
    public bool slow, dmgOverTime;

    [Header("Slow effect settings")]
    public float slowDuration = 1f;
    public float speedModifier = 0.1f;

    [Header("Dmg Over Time effect settings")]
    public int dmgPerTick = 1;
    public float dotDuration = 5f;
    public float dotTickDuration = 1f;

    public void Activate(GameObject target)
    {
        if (slow)
        {
            var t = target.GetComponent<ISpeedChange>();
            if (t != null)
            {
                t.ChangeSpeedModifier(slowDuration, speedModifier);
            }
        }

        if (dmgOverTime)
        {
            var t = target.GetComponent<IDamageable>();
            if (t != null)
            {
                var dot = target.AddComponent<Surv_DamagedOverTime>();
                dot.Setup(dmgPerTick, dotDuration, dotTickDuration);
            }
        }
    }

    public void SetSlowEffect(float slowDuration = 1f, float speedModifier = 0.2f)
    {
        this.slow = true;
        this.slowDuration = slowDuration;
        this.speedModifier = speedModifier;
    }

    public void SetDotEffect(int dmgPerTick = 1, float dotDuration = 5f, float tickDuration = 1f)
    {
        this.dmgOverTime = true;
        this.dmgPerTick = dmgPerTick;
        this.dotDuration = dotDuration;
        this.dotTickDuration = tickDuration;
    }
}
