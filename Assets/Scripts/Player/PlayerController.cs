using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    PlayerMovement pMovement;
    PlayerCombat pCombat;
    public UnityEngine.InputSystem.PlayerInput input;

    public bool facingRight { get { return pMovement.facingRight; } }
    public bool canMove = true;
    public bool isGrounded = false;
    public bool isOnWall = false;
    private Vector2 moveInput;

    void Start()
    {
        PlayerData.Instance.SetPlayerController(this);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        pCombat = GetComponent<PlayerCombat>();
        pMovement = GetComponent<PlayerMovement>();
        EnableInput();
    }

    void Update()
    {
        isGrounded = pMovement.isGrounded;
        isOnWall = pMovement.isOnWall;
    }

    public void OnDash()
    {
        if (pCombat.isGuarding) return;

        pMovement.Dash(moveInput.x);
    }
    
    public void OnMove(InputValue value)
    {
        if (pCombat.isGuarding) return;

        moveInput = new Vector2(value.Get<Vector2>().x, 0f);
        pMovement.Move(moveInput);
    }

    public void OnJump(InputValue value)
    {
        if (pCombat.isGuarding) return;

        float input = value.Get<float>();
        // if pressed
        if (input > 0.5f)
        {
            pMovement.Jump();
        }
        else  // if released
        {
            if (rb.velocity.y > 0f && !isGrounded)
                rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }

    public void Die()
    {
        Debug.Log("Player died.");
        DisableInput();
    }

    // Stagger direction :  default = 0 = no stagger
    //                      > 0 = push right
    //                      < 0 = push left                   
    public void Damage(int dmg , int staggerDirection = 0)
    {
        if (PlayerData.Instance.isDead) return;

        PlayerData.Instance.ReceiveDamage(dmg);
        if (staggerDirection > 0) pMovement.Stagger(1);
        if (staggerDirection < 0) pMovement.Stagger(-1);
    }

    public void DisableInput()
    {
        input.currentActionMap.Disable();
    }

    public void EnableInput()
    {
        input.currentActionMap.Enable();
    }
}
