using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Surv_LaserBeam : Surv_PlayerAttack
{
    [Header("Laser Beam Settings")]
    public Transform firePoint;
    public List<int> numOfBeamsAtLevel;
    public float attackRadius = 15f;
    public LayerMask enemyLayer;
    public float beamTime = 0.1f;
    public float timeBetweenBeams = 0.1f;
    public LineRenderer beam;

    protected override void Start()
    {
        base.Start();
        beam.gameObject.SetActive(false);
    }

    private void Update()
    {
        base.UpdateAttack();
    }

    public override void Attack()
    {
        StopCoroutine(AttackCoroutine());
        StartCoroutine(AttackCoroutine());
    }
    public IEnumerator AttackCoroutine()
    {
        autoAttack = false; // disable timer to prevent clipping 
        int fireAmount = numOfBeamsAtLevel[Mathf.Min(level, ATTACK_MAX_LV) - 1];

        Collider[] hits;
        do
        {
            hits = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);
            yield return null;
        } while (hits != null && hits.Length <= 0);

        Vector3 beamEndPos = firePoint.position + 5f * firePoint.right;
        for (int i = 0; i < fireAmount; ++i)
        {
            var target = hits[Random.Range(0, hits.Length)];
            Surv_Enemy enemy = null;
            beam.gameObject.SetActive(true);
            beam.SetPosition(0, firePoint.position);

            if (target != null) // prevent error if target dies to previous beam
            {
                enemy = target.GetComponent<Surv_Enemy>();
                if (enemy)
                {
                    beamEndPos = enemy.GetComponent<Collider>().bounds.center;
                }
            }

            float t = 0;
            Vector3 endPos = beam.GetPosition(0);
            while(t < beamTime)
            {
                endPos = Vector3.Lerp(endPos, beamEndPos, t/beamTime);
                beam.SetPosition(1, endPos);

                yield return null;
                t += Time.deltaTime;
            }
            enemy?.ReceiveDamage(damage);

            beam.gameObject.SetActive(false);
            yield return new WaitForSeconds(timeBetweenBeams);
        }

        ResetAttackTimer();
        autoAttack = true; // re-enable timer
    }


    public override void OnValidate()
    {
        base.OnValidate();

        // Resize number of bullets fired at each level list
        if (numOfBeamsAtLevel.Count < ATTACK_MAX_LV)
        {
            for (int i = numOfBeamsAtLevel.Count; i < ATTACK_MAX_LV; ++i)
            {
                numOfBeamsAtLevel.Add(1);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
