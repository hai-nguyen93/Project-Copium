using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

public enum AbilityState { ready, cooldown, active, readyToCast}

[System.Serializable]
public class ActiveAbility
{
    public Ability abilityData;
    public float cdTimer;
    public float castTimer;
    public AbilityState state;
    private GameObject _user;
    private PlayerCombat pCombat;

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
                if (abilityData.castType == CastType.channeling)
                {
                    if (_user == null)
                    {
                        Debug.LogWarning("There is no user casting this " + abilityData.abilityName + " ability.");
                        cdTimer = abilityData.cooldown;
                        state = AbilityState.cooldown;
                        return;
                    }

                    if (castTimer > 0f) // casting ability
                    {
                        castTimer -= Time.deltaTime;
                        pCombat.castPanel.UpdateCastBar(1f - castTimer / abilityData.castTime);
                    }
                    else // finish channeling, waiting to cast
                    {
                        state = AbilityState.readyToCast;
                        Debug.Log("Ready to cast " + abilityData.abilityName);
                        pCombat.castPS.PlayReadyPS();
                    }
                }
                break;
        }     
    }

    public void FinishCastAbility()
    {
        Debug.Log("Finish casting " + abilityData.abilityName);
        castTimer = 0f;
        abilityData.Activate(_user);
        ResetCastAbility();
    }

    public void CancelCastAbility()
    {
        Debug.Log("Cancel casting " + abilityData.abilityName);
        ResetCastAbility();
    }

    public void ResetCastAbility()
    {
        cdTimer = abilityData.cooldown;
        state = AbilityState.cooldown;
        pCombat.castPanel.Hide();
        pCombat.castPS.StopAllParticles();
    }

    public void Activate(GameObject user)
    {
        _user = user;
        switch (abilityData.castType)
        {
            case CastType.instant:
                abilityData.Activate(user);
                cdTimer = abilityData.cooldown;
                state = AbilityState.cooldown;
                break;

            case CastType.channeling:
                castTimer = abilityData.castTime;
                state = AbilityState.active;
                Debug.Log("Start casting " + abilityData.abilityName);

                // set up UI if user is player
                pCombat = user.GetComponent<PlayerCombat>();
                if (pCombat != null)
                {
                    pCombat.castPanel.SetUp(abilityData.abilityName);
                    pCombat.castPanel.Show();
                    pCombat.castPS.PlayChannelingPS(abilityData.castTime);
                }
                break;
        }
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
    public AbilityPanel abilityPanelUI;
    public CastPanel castPanel;
    public AbilityCastingPS castPS;

    private bool skillKeyPressed = false;
    private Animator anim;
    private PlayerController pc;


    private void Start()
    {
        anim = GetComponent<Animator>();
        pc = GetComponent<PlayerController>();
        attackHitBoxVisual.enabled = false;
        isGuarding = false;

        castPanel.Hide();
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

    public void RefreshAbilityIcons()
    {
        abilityPanelUI.UpdateAbilitySlots();
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
            if (abilityPanelUI != null)
            {
                abilityPanelUI.Show();
            }
        }
        else
        {
            skillKeyPressed = false;
            if (abilityPanelUI != null)
            {
                abilityPanelUI.Hide();
            }
        }
    }

    public void OnAbility0(InputValue value)
    {
        float input = value.Get<float>();
        UseEquippedAbility(0, input);
    }

    public void OnAbility1(InputValue value)
    {
        float input = value.Get<float>();
        UseEquippedAbility(1, input);
    }

    public void OnAbility2(InputValue value)
    {
        float input = value.Get<float>();
        UseEquippedAbility(2, input);
    }

    public void OnAbility3(InputValue value)
    {
        float input = value.Get<float>();
        UseEquippedAbility(3, input);
    }

    public void UseEquippedAbility(int index, float input)
    {
        var ability = equippedAbilities.ElementAtOrDefault(index);
        if (ability == null || ability.abilityData == null)
        {
            Debug.Log("No Ability in slot " + index);
            return;
        }

        if (input > 0.5f) // key pressed
        {
            if (ability.state == AbilityState.ready)
            {
                ability.Activate(gameObject);
            }
            else
            {
                Debug.Log(ability.abilityData.abilityName + " not ready");
            }
        }
        else // key released
        {
            if (ability.abilityData.castType == CastType.channeling)
            {
                if (ability.state == AbilityState.readyToCast)
                {
                    ability.FinishCastAbility();
                    return;
                }

                if (ability.state == AbilityState.active)
                {
                    // Cancel/Interrupt cast
                    ability.CancelCastAbility();
                    return;
                }
            }
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
