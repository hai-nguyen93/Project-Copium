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

    [Header("Move Settings")]
    public float moveSpeed = 2f;
    public float defaultGravityScale = 1f;
    public bool facingRight = true;
    public bool canMove = true;
    public bool isGrounded = false;
    public bool isOnWall = false;
    private Vector2 moveDirection;

    [Header("Dash/Backstep Settings")]
    public ParticleSystem dashParticle;
    public float backStepPower = 10f;
    public float backStepTime = 0.15f;
    public float dashPower = 20f;
    public float dashTime = 0.15f;
    public float dashCooldown = 1f;
    public bool isDashing = false;
    public bool canDash = true;

    [Header("Jump/Wall Settings")]
    public float jumpPower = 5f;
    public Transform wallCheck;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public bool unlockDoubleJump = false;
    private bool  doubleJump = false;
    private float fallSpeedModifier = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = IsGrounded();
        isOnWall = IsOnWall();
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSliding", isOnWall);
        UpdateMovement();
    }


    #region Movement
    void UpdateMovement()
    {
        if (isDashing) return;

        // check if grabbing wall
        if (isOnWall && (moveDirection.x * transform.localScale.x > 0.5f))
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
            anim.Play("WallSlide");
            return;
        }

        // gravity update
        rb.gravityScale = defaultGravityScale;
        fallSpeedModifier = 1f;

        if (!canMove) return;
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y * fallSpeedModifier);
        anim.SetFloat("speedX", Mathf.Abs(moveDirection.x));
        anim.SetFloat("speedY", Mathf.Abs(rb.velocity.y));
        if (moveDirection.x > 0.5f && !facingRight) Flip();
        else if (moveDirection.x < -0.5f && facingRight) Flip();
    }

    public IEnumerator BackStep()
    {
        // begin back step
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(-backStepPower * transform.localScale.x, 0f);
        yield return new WaitForSeconds(backStepTime);

        // end back step
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public IEnumerator Dash()
    {
        // begin Dash
        canDash = false;
        isDashing = true;
        float originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(moveDirection.x * dashPower, 0f);        
        dashParticle.Play();
        yield return new WaitForSeconds(dashTime);


        // end Dash, wait for cooldown
        dashParticle.Stop();
        isDashing = false;
        rb.gravityScale = originalGravityScale;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    public void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        facingRight = !facingRight;
    }  

    public void OnDash()
    {
        if (!canDash || isDashing) return;

        // dash if on ground && can dash
        if (moveDirection.magnitude < 0.1f)
        {
            StartCoroutine(BackStep());
        }
        else
        {
            StartCoroutine(Dash());
        }
    }
    
    public void OnMove(InputValue value)
    {
        moveDirection = new Vector2(value.Get<Vector2>().x, 0f);
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public bool IsOnWall()
    {
        return !isGrounded && Physics2D.OverlapCircle(wallCheck.position, 0.2f, groundLayer);
    }

    public bool canDoubleJump()
    {
        /*if (unlockDoubleJump)
        {
            return true;
        }
        else
        {
            return false;
        }*/

        return unlockDoubleJump;
    }

    public void OnJump(InputValue value)
    {
        // wall jump if is on wall
        if (isOnWall)
        {
            rb.velocity = new Vector2(moveSpeed * transform.localScale.x * -1, jumpPower);
            Flip();
            doubleJump = true;
            anim.Play("Jump");
            StopCoroutine(DisableMovement(0));
            StartCoroutine(DisableMovement(0.15f));

            return;
        }

        // else normal jump
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            anim.Play("Jump");
            doubleJump = true;
        }else if (canDoubleJump() && doubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            anim.Play("Jump");
            doubleJump = false;
        }
        
    }
    #endregion


    #region Draw gizmos
    public void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(wallCheck.position, new Vector3(0, 0, 1), 0.2f);
        Handles.DrawWireDisc(groundCheck.position, new Vector3(0, 0, 1), 0.2f);
    }
    #endregion
}
