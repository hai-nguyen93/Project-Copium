using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerController : MonoBehaviour, IDamageable
{
    Rigidbody2D rb;
    Animator anim;
    PlayerCombat pCombat;

    [Header("Move Settings")]
    public float moveSpeed = 2f;
    public float defaultGravityScale = 1f;
    public float fallModifier = 1.5f;
    public bool facingRight = true;
    public bool canMove = true;
    public bool isGrounded = false;
    public bool isOnWall = false;
    private Vector2 moveInput;
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
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    public bool unlockDoubleJump = false;
    private bool  doubleJump = false;

    void Start()
    {
        PlayerData.Instance.SetPlayerController(this);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        pCombat = GetComponent<PlayerCombat>();
    }

    void Update()
    {
        isGrounded = IsGrounded();
        isOnWall = IsOnWall();
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSliding", isOnWall);
    }

    void FixedUpdate()
    {
        UpdateMovement();
    }
    
    #region Movement
    void UpdateMovement()
    {
        if (isDashing) return;

        // check if grabbing wall
        if (isOnWall && (moveInput.x * transform.localScale.x > 0.5f))
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
            anim.Play("WallSlide");
            return;
        }

        // gravity update       
        if (isGrounded)
        {
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = (rb.velocity.y > 0f) ? defaultGravityScale : defaultGravityScale * fallModifier;
        }
        float yVel = rb.velocity.y;

        // slope velocity in y-axis
        /*if (isGrounded)
        {
            if (Mathf.Abs(moveInput.x) > 0.1f)
            {
                yVel = moveDirection.y * moveSpeed;
            }
        }*/

        if (!canMove) return;
        //yVel = rb.velocity.y;
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, yVel);

        // update animation
        anim.SetFloat("speedX", Mathf.Abs(moveInput.x));
        anim.SetFloat("speedY", rb.velocity.y);
        //anim.SetFloat("speedY", Mathf.Clamp(Mathf.Abs(rb.velocity.y), 0f, 10f));
        if (moveInput.x > 0.5f && !facingRight) Flip();
        else if (moveInput.x < -0.5f && facingRight) Flip();
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
        rb.velocity = new Vector2(moveInput.x * dashPower, 0f);        
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
        if (!canDash || isDashing || pCombat.isGuarding) return;

        // dash if on ground && can dash
        if (moveInput.magnitude < 0.1f)
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
        if (pCombat.isGuarding) return;

        moveInput = new Vector2(value.Get<Vector2>().x, 0f);
    }

    public bool IsGrounded()
    {
        moveDirection.Set(moveInput.x, 0f);
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    public bool IsGroundedSlope()
    {       
        Debug.DrawRay(groundCheck.position, Vector2.down * checkRadius, Color.green);
        var hit =  Physics2D.Raycast(groundCheck.position, Vector2.down, checkRadius, groundLayer);
        if (hit)
        {
            Vector2 groundNormalPerp = Vector2.Perpendicular(hit.normal);
            moveDirection = new Vector2(-moveInput.x * groundNormalPerp.x, -moveInput.x * groundNormalPerp.y).normalized;
            Debug.DrawRay(transform.position, groundNormalPerp.normalized, Color.green);
            return true;
        }

        moveDirection = new Vector2(moveInput.x, 0f);
        return false;
    }

    public bool IsOnWall()
    {
        return !isGrounded && Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);
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
        if (pCombat.isGuarding) return;

        float input = value.Get<float>();
        // if pressed
        if (input > 0.5f)
        {
            // wall jump if is on wall
            if (isOnWall)
            {
                rb.velocity = new Vector2(moveSpeed * transform.localScale.x * -1, jumpPower);
                Flip();
                doubleJump = true;
                anim.Play("Jump");
                anim.SetFloat("speedY", Mathf.Clamp(Mathf.Abs(rb.velocity.y), 0f, 10f));
                StopCoroutine(DisableMovement(0));
                StartCoroutine(DisableMovement(0.15f));

                return;
            }

            // else normal jump
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                anim.Play("Jump");
                anim.SetFloat("speedY", Mathf.Clamp(Mathf.Abs(rb.velocity.y), 0f, 10f));
                doubleJump = true;
            }
            else if (canDoubleJump() && doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                anim.Play("Jump");
                anim.SetFloat("speedY", Mathf.Clamp(Mathf.Abs(rb.velocity.y), 0f, 10f));
                doubleJump = false;
            }
        }

        // if released
        else
        {
            if (rb.velocity.y > 0f && !isGrounded)
                rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }
    #endregion

    public void Damage(int dmg)
    {
        PlayerData.Instance.ReceiveDamage(dmg);
    }

    #region Draw gizmos
    public void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(wallCheck.position, new Vector3(0, 0, 1), checkRadius);
        Handles.DrawWireDisc(groundCheck.position, new Vector3(0, 0, 1), checkRadius);
    }
    #endregion
}
