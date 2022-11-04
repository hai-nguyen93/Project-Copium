using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Surv_DamageAreaSphere : MonoBehaviour
{
    public LayerMask targetLayer;
    public int damage = 1;
    public float radius = 2.5f;
    public float lifespan = 5f;
    public float areaTickDuration = 0.5f;
    private float areaTickTimer;
    private float timer;
    public VisualEffect puddleVfx;
    public Transform telegraph;
    private bool active;

    [Header("Dmg Over Time Settings")]
    public bool applyDot;
    public int dmgPerTick = 1;
    public float dotDuration = 5f;
    public float dotTickDuration = 1f;

    [Header("Slow Settings")]
    public bool applySlow;
    public float slowDuration = 1f;
    public float speedModifier = 0.75f;

    private void Start()
    {
        active = true;
        timer = lifespan;
        areaTickTimer = areaTickDuration;

        if (telegraph)
        {
            telegraph.localScale = new Vector3(2 * radius, 2 * radius, 0);
        }

        puddleVfx.SetFloat("Radius", radius);
        puddleVfx.SetFloat("Lifetime", lifespan);
        puddleVfx.Play();
    }

    private void Update()
    {
        if (!active) return;
        if (timer <= 0f) {
            active = false;
            Destroy(gameObject, 1f);
            telegraph.gameObject.SetActive(false);
        }

        if (areaTickTimer <= 0f) {
            DoDamage();
            areaTickTimer = areaTickDuration;
        }

        areaTickTimer -= Time.deltaTime;
        timer -= Time.deltaTime;
    }

    void DoDamage()
    {
        var hits = Physics.OverlapSphere(transform.position, radius, targetLayer);
        if (hits != null)
        {
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<IDamageable>(out var target))
                {
                    target.ReceiveDamage(damage);

                    if (applyDot)
                    {
                        Surv_DamagedOverTime dot;
                        if (!hit.TryGetComponent<Surv_DamagedOverTime>(out dot))
                        {
                            dot = hit.gameObject.AddComponent<Surv_DamagedOverTime>();
                        }
                        dot.Setup(target, dmgPerTick, dotDuration, dotTickDuration);
                    }

                    if (applySlow)
                    {
                        if (hit.TryGetComponent<ISpeedChange>(out var t))
                        {
                            t.ChangeSpeedModifier(slowDuration, speedModifier);
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
