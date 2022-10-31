using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_ApplyDotSphere : MonoBehaviour
{
    public LayerMask targetLayer;
    public float radius = 2.5f;
    public float lifespan = 5f;
    private float timer;

    [Header("Dmg Over Time Settings")]
    public int dmgPerTick = 1;
    public float dotDuration = 5f;
    public float dotTickDuration = 1f;

    private void Start()
    {
        timer = lifespan;
    }

    private void Update()
    {
        UpdateTimer();

        var hits = Physics.OverlapSphere(transform.position, radius, targetLayer);
        if (hits != null)
        {
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<IDamageable>(out var target))
                {
                    Surv_DamagedOverTime dot;
                    if (!hit.TryGetComponent<Surv_DamagedOverTime>(out dot))
                    {
                        dot = hit.gameObject.AddComponent<Surv_DamagedOverTime>();
                    }
                    else { target.ReceiveDamage(dmgPerTick); }
                    dot.Setup(target, dmgPerTick, dotDuration, dotTickDuration);
                }
            }
        }
    }

    void UpdateTimer()
    {
        if (timer <= 0f) { Destroy(gameObject); }
        timer -= Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
