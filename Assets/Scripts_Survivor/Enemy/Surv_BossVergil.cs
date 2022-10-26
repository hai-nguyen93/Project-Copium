using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Surv_BossVergil : Surv_Enemy
{
    public float aggroRange = 3f;
    public bool isActing = false;

    [Header("Motivated Cut settings")]
    public LayerMask playerLayer;
    public float dashDistance = 10f;
    public float dashTime = 0.1f;
    public BoxCollider selfHitbox;
    public VisualEffect motivatedCutVFX;

    public override void Update()
    {
        if (Surv_GameController.Instance.useMultiThread) return;

        if (Surv_GameController.Instance.state != GameState.Gameplay || player == null || isActing) return;
        if (player == null || player.isDead || isDead) return;

        ChooseAction();
    }

    public void ChooseAction()
    {
        if (isActing) return;
        LookAtPlayer();
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance > aggroRange) ChasePlayer();
        else
        {
            StartCoroutine(MotivatedCut());
        }
    }

    public override void HitPlayer() { return; }

    public IEnumerator MotivatedCut()
    {
        isActing = true;
        Vector3 playerPos = player.transform.position;
        Vector3 startPos = transform.position;
        Vector3 direction = new Vector3(playerPos.x - startPos.x, 0, playerPos.z - startPos.z).normalized;
        Vector3 endPos = startPos + dashDistance * direction;

        // Charge
        yield return new WaitForSeconds(1.5f);

        // Dash
        bool hitPlayer = false;
        float t = 0f;
        while (t < dashTime)
        {
            yield return null;

            // Check collision with player
            if (!hitPlayer)
            {
                var hits = Physics.OverlapBox(transform.position + selfHitbox.center, selfHitbox.bounds.extents, transform.rotation, playerLayer);
                if (hits.Length > 0)
                {
                    foreach(var hit in hits)
                    {
                        if (hit.CompareTag("Player"))
                        {
                            hitPlayer = true;
                            player.canMove = false;
                            break;
                        }
                    }
                }
            }

            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t / dashTime);
        }

        // Motivated Cut
        if (hitPlayer)
        {
            Vector3 pos = player.transform.position;
            motivatedCutVFX.transform.position = new Vector3(pos.x, 0, pos.z);
            motivatedCutVFX.Play();
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < 5; ++i)
            {
                player.ReceiveDamage(1);
                yield return new WaitForSeconds(0.2f);
            }
            motivatedCutVFX.Stop();
        }

        player.canMove = true;
        yield return new WaitForSeconds(2f);
        isActing = false;
    }
}
