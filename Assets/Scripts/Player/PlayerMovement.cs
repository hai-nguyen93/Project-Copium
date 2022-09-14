using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerController pc;

    [Header("Move Settings")]
    [Tooltip("Force applied to self when staggered")] public float staggerForce = 2f;
    public bool canMove = true;
    public bool facingRight;
    public float moveSpeed = 2f;
    public float defaultGravityScale = 1.75f;
    public float fallModifier = 3.5f;
    private Vector2 moveDirection;

    [Header("Jump/Wall Settings")]
    public bool isGrounded = false;
    public bool isOnWall = false;
    public float jumpPower = 5f;
    public Transform wallCheck;
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public bool unlockDoubleJump = false;
    private bool doubleJump = false;

    [Header("Dash/Backstep Settings")]
    public ParticleSystem dashParticle;
    public float backStepPower = 10f;
    public float backStepTime = 0.15f;
    public float dashPower = 20f;
    public float dashTime = 0.15f;
    public float dashCooldown = 1f;
    public bool isDashing = false;
    public bool canDash = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        pc = GetComponent<PlayerController>();
    }

    private void Update()
    {
        isGrounded = IsGrounded();
        isOnWall = IsOnWall();
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSliding", isOnWall);
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    void UpdateMovement()
    {
        if (isDashing) return;

        // check if grabbing wall
        if (isOnWall && (moveDirection.x * (facingRight ? 1 : -1) > 0.5f))
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
            anim.Play("WallSlide");
            return;
        }

        // gravity update       
        float yVel = rb.velocity.y;
        if (isGrounded)
        {
            rb.gravityScale = 0f;
            //yVel = 0f;
        }
        else
        {
            rb.gravityScale = (rb.velocity.y > 0f) ? defaultGravityScale : defaultGravityScale * fallModifier;
        }

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
        anim.SetFloat("speedX", Mathf.Abs(moveDirection.x));
        anim.SetFloat("speedY", rb.velocity.y);
        if (facingRight && moveDirection.x < -0.5f) Flip();
        if (!facingRight && moveDirection.x > 0.5f) Flip();
        //anim.SetFloat("speedY", Mathf.Clamp(Mathf.Abs(rb.velocity.y), 0f, 10f));
    }

    public void Move(Vector2 input)
    {
        float x = (Mathf.Abs(input.x) > 0.5f) ? Mathf.Sign(input.x) : 0f;
        moveDirection = new Vector2(x , 0f);
    }

    public void Jump()
    {
        // wall jump if is on wall
        if (isOnWall)
        {
            float xDirection = facingRight ? 1 : -1;
            rb.velocity = new Vector2(moveSpeed * xDirection * -1, jumpPower);
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

    public void Dash(float xInput)
    {
        if (!canDash || isDashing) return;

        if (Mathf.Abs(xInput) < 0.1f) // back step when dash with no movement
        {
            StartCoroutine(BackStepCoroutine());
        }
        else
        {
            StartCoroutine(DashCoroutine());
        }
    }
    public IEnumerator DashCoroutine()
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

    public void BackStep()
    {
        StartCoroutine(BackStepCoroutine());
    }
    public IEnumerator BackStepCoroutine()
    {
        // begin back step
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(-backStepPower * (facingRight ? 1 : -1), 0f);
        yield return new WaitForSeconds(backStepTime);

        // end back step
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void Stagger(int direction)
    {
        if (PlayerData.Instance.isDead) return;

        Debug.Log("stagger push direction" + direction);
        rb.velocity = new Vector2(0f, rb.velocity.y);
        rb.AddForce(new Vector2((direction > 0) ? 1 : -1, 0f) * staggerForce, ForceMode2D.Impulse);
        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(1f));
    }

    public IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    public bool IsOnWall()
    {
        return !isGrounded && Physics2D.OverlapCircle(wallCheck.position, checkRadius, wallLayer);
    }

    public bool canDoubleJump()
    {
        return unlockDoubleJump;
    }

    public void Flip()
    {
        //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        transform.Rotate(new Vector3(0, 180, 0));
        facingRight = !facingRight;
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
