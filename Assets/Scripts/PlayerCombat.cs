using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask attackLayer;
    public float attackCooldown = 1f;
    public bool canAttack = true;
    public bool isGuarding = false;
    public SpriteRenderer attackHitBoxVisual;

    private Animator anim;
    private PlayerController pc;


    private void Start()
    {
        anim = GetComponent<Animator>();
        pc = GetComponent<PlayerController>();
        attackHitBoxVisual.enabled = false;
        isGuarding = false;
    }

    private void Update()
    {
        if (!pc.isGrounded && isGuarding) // break guard if player is NOT grounded
        {
            isGuarding = false;
            anim.SetBool("isGuarding", false);
        }
    }

    public void CheckHit()
    {
        StartCoroutine(DrawHitBox(0.25f));
        var hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, attackLayer);
        foreach (var hit in hits)
        {
            Debug.Log("hit " + hit.gameObject.name);
        }
    }

    public void OnAttack(InputValue value)
    {
        float input = value.Get<float>();
        if (input > 0.5f)
        {
            if (canAttack)
            {
                anim.Play("Attack");
                StartCoroutine(CooldownAttack());
            }
        }
    }

    public void OnGuard(InputValue value)
    {
        if (!pc.isGrounded) return;

        float input = value.Get<float>();
        if (input > 0.5f)
        {
            isGuarding = true;           
        }
        else
        {
            isGuarding = false;
        }

        anim.SetBool("isGuarding", isGuarding);
    }

    IEnumerator CooldownAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator DrawHitBox(float time)
    {
        attackHitBoxVisual.enabled = true;
        yield return new WaitForSeconds(time);
        attackHitBoxVisual.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(attackPoint.position, new Vector3(0, 0, 1), attackRange);
    }
}
