using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

    public enum AbilityState { ready, cooldown, active}

[System.Serializable]
public class ActiveAbility
{
    public Ability abilityData;
    public float cdTimer;
    public AbilityState state;

    public void SetCdTimer(float time) { cdTimer = time; }

    public void SetAbilityState (AbilityState newState) { state = newState; }

    public void UpdateAbility()
    {
        switch (state)
        {
            case AbilityState.ready:
                break;
            case AbilityState.cooldown:
                if (cdTimer > 0f)
                {
                    cdTimer -= Time.deltaTime;
                }
                else
                {
                    cdTimer = 0f;
                    state = AbilityState.ready;
                    Debug.Log(abilityData.abilityName + " is ready.");
                }
                break;
            case AbilityState.active:
                break;
        }
    }

    public void Activate(GameObject user)
    {
        abilityData.Activate(user);
        cdTimer = abilityData.cooldown;
        state = AbilityState.cooldown;
    }
}

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask attackLayer;
    public float attackCooldown = 1f;
    public bool canAttack = true;
    public bool isGuarding = false;
    public SpriteRenderer attackHitBoxVisual;

    [Header("Abilities")]
    public List<ActiveAbility> equippedAbilities;
    public GameObject AbilityPanelUI;

    private bool skillKeyPressed = false;
    private Animator anim;
    private PlayerController pc;


    private void Start()
    {
        anim = GetComponent<Animator>();
        pc = GetComponent<PlayerController>();
        attackHitBoxVisual.enabled = false;
        isGuarding = false;

        foreach (var a in equippedAbilities)
        {
            a.SetCdTimer(0f);
            a.SetAbilityState(AbilityState.ready);
        }
    }

    private void Update()
    {
        foreach (var a in equippedAbilities)
        {
            a.UpdateAbility();
        }

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
        if (skillKeyPressed) return;

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
        if (!pc.isGrounded || skillKeyPressed) return;

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

    public void OnAbilityTrigger(InputValue value)
    {
        float input = value.Get<float>();

        if (input > 0.5f)
        {
            skillKeyPressed = true;
            if (AbilityPanelUI != null)
            {
                AbilityPanelUI.GetComponent<AbilityPanel>().Show();
            }
        }
        else
        {
            skillKeyPressed = false;
            if (AbilityPanelUI != null)
            {
                AbilityPanelUI.GetComponent<AbilityPanel>().Hide();
            }
        }
    }

    public void OnAbility0(InputValue value)
    {
        float input = value.Get<float>();
        if (input > 0.5f)
        {
            UseEquippedAbility(0);
        }
    }

    public void OnAbility1(InputValue value)
    {
        float input = value.Get<float>();
        if (input > 0.5f)
        {
            UseEquippedAbility(1);
        }
    }

    public void OnAbility2(InputValue value)
    {
        float input = value.Get<float>();
        if (input > 0.5f)
        {
            UseEquippedAbility(2);
        }
    }

    public void OnAbility3(InputValue value)
    {
        float input = value.Get<float>();
        if (input > 0.5f)
        {
            UseEquippedAbility(3);
        }
    }

    public void UseEquippedAbility(int index)
    {
        var ability = equippedAbilities.ElementAtOrDefault(index);
        if (ability == null || ability.abilityData == null)
        {
            Debug.Log("No Ability in this slot");
            return;
        }

        if (ability.state == AbilityState.ready)
        {
            ability.Activate(gameObject);
        }
        else
        {
            Debug.Log(ability.abilityData.abilityName + " not ready");
        }
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
