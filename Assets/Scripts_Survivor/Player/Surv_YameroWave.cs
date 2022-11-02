using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_YameroWave : Surv_PlayerAttack
{
    [Header("Attack Settings")]
    public List<float> pushBackForceAtLevel;
    public List<float> radiusAtLevel;
    public LayerMask enemyLayer;
    public float attackradius { get => radiusAtLevel[Mathf.Min(level, ATTACK_MAX_LV) - 1]; }
    public float pushBackForce { get => pushBackForceAtLevel[Mathf.Min(level, ATTACK_MAX_LV) - 1]; }

    [Header("Visual Settings")]
    public GameObject telegraph;
    public Transform visualEffect;
    public Vector2 waveLerpRange;
    private Renderer vfxRenderer;
    private MaterialPropertyBlock vfxMatPropBlock;
    public float visualDuration = 0.5f;

    protected override void Start()
    {
        base.Start();
        telegraph.SetActive(false);

        visualEffect.gameObject.SetActive(false);
        vfxMatPropBlock = new MaterialPropertyBlock();
        vfxRenderer = visualEffect.GetComponent<Renderer>();
    }

    private void Update()
    {
        base.UpdateAttack();
    }

    public override void Attack()
    {
        StartCoroutine(ShowVFX());

        var hits = Physics.OverlapSphere(transform.position, attackradius, enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Surv_Enemy>(out var enemy))
            {
                Vector3 pushDirection = new Vector3(enemy.transform.position.x - transform.position.x, 0,
                    enemy.transform.position.z - transform.position.z).normalized;
                enemy.PushBack(pushDirection, pushBackForce);
            }
        }
        ResetAttackTimer();
    }

    private IEnumerator ShowVFX()
    {
        telegraph.SetActive(true);
        telegraph.transform.localScale = new Vector3(2 * attackradius, 2 * attackradius, 1f);

        visualEffect.gameObject.SetActive(true);
        visualEffect.localScale = new Vector3(2 * attackradius, 2 * attackradius, 1f);

        float t = visualDuration;
        while (t > 0f) {
            vfxRenderer.GetPropertyBlock(vfxMatPropBlock);
            float amount = Mathf.Lerp(waveLerpRange.x, waveLerpRange.y, 1 - (t / visualDuration));
            vfxMatPropBlock.SetFloat("_LerpAmount", amount);
            vfxRenderer.SetPropertyBlock(vfxMatPropBlock);

            t -= Time.deltaTime;
            yield return null;
        }

        telegraph.SetActive(false);
        visualEffect.gameObject.SetActive(false);
    }

    public override void OnValidate()
    {
        base.OnValidate();

        if (pushBackForceAtLevel.Count < ATTACK_MAX_LV)
        {
            for (int i = pushBackForceAtLevel.Count; i < ATTACK_MAX_LV; ++i)
            {
                pushBackForceAtLevel.Add(10f);
            }
        }

        if (radiusAtLevel.Count < ATTACK_MAX_LV)
        {
            for (int i = radiusAtLevel.Count; i < ATTACK_MAX_LV; ++i)
            {
                radiusAtLevel.Add(2.5f);
            }
        }
    }
}
