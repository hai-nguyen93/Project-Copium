using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletEffectType { DmgOverTime, Slow, Damage}

public class Surv_BulletEffect : MonoBehaviour
{
    public BulletEffectType type;

    public float duration = 1f;
    public float speedModifier = 0.1f;

    public void Activate(GameObject target)
    {
        switch (type)
        {
            case BulletEffectType.Damage:
                break;

            case BulletEffectType.DmgOverTime:
                break;

            case BulletEffectType.Slow:
                var t = target.GetComponent<ISpeedChange>();
                if (t != null)
                {
                    t.ChangeSpeedModifier(duration, speedModifier);
                }
                break;
        }
    }
}
