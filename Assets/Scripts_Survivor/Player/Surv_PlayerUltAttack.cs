using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerUltAttack : Surv_PlayerAttack
{
    public Surv_BreakScreenFX breakScreenFX;
    public float attackRadius = 15f;
    public LayerMask layerToCollide;

    protected override void Start()
    {
        autoAttack = false;
        base.Start();
        breakScreenFX.gameObject.SetActive(false);
        ResetAttackTimer();
    }

    private void Update()
    {
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
        
        // testing
        if (Input.GetKeyDown(KeyCode.R) && attackTimer <= 0f)
        {
            UseUltimate();
        }
        //
    }

    public void UseUltimate()
    {
        ResetAttackTimer();
        Time.timeScale = 0f;
        Surv_GameController.Instance.state = GameState.CannotPause;
        StartCoroutine(UltimateCoroutine());
    }
    public IEnumerator UltimateCoroutine()
    {
        breakScreenFX.gameObject.SetActive(true);
        yield return StartCoroutine(breakScreenFX.CoroutineBreakScreen());      

        FinishUltimate();
    }

    public void FinishUltimate()
    {
        breakScreenFX.gameObject.SetActive(false);
        Time.timeScale = 1f;
        Surv_GameController.Instance.state = GameState.Gameplay;

        // check collision
        var hits = Physics.OverlapSphere(player.transform.position, attackRadius, layerToCollide);
        foreach(var hit in hits)
        {
            var target = hit.GetComponent<IDamageable>();
            if (target != null)
            {
                target.ReceiveDamage(damage);
            }
        }

        ResetAttackTimer();
    }

    public float GetCooldownFillAmount()
    {
        return 1 - (attackTimer / attackCooldown);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
