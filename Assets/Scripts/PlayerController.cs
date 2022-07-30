using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Move Settings")]
    public float moveSpeed = 2f;
    public bool facingRight = true;
    private Vector2 moveDirection;

    [Header("Dash/Backstep Settings")]
    public ParticleSystem dashParticle;
    public float dashPower = 20f;
    public float dashTime = 0.15f;
    public float dashCooldown = 1f;
    public bool isDashing = false;
    public bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        UpdateMovement();
    }


    #region Movement
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

    public void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        facingRight = !facingRight;
    }

    void UpdateMovement()
    {
        if (isDashing) return;

        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        if (moveDirection.x > 0.5f && !facingRight) Flip();
        else if (moveDirection.x < -0.5f && facingRight) Flip();
    }

    public void OnDash()
    {
        if (!canDash || moveDirection.magnitude < 0.1f) return;

        // dash if on ground && can dash
        Debug.Log("Dash");
        StartCoroutine(Dash());
    }
    
    public void OnMove(InputValue value)
    {
        moveDirection = new Vector2(value.Get<Vector2>().x, 0f);
    }
    #endregion
}
