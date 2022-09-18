using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Surv_PlayerController : MonoBehaviour
{
    public SpriteRenderer sr;
    private Rigidbody2D rb;

    [Header("Player Status")]
    public bool isDead = false;

    [Header("Movement Settings")]
    public float baseMoveSpeed = 2f;
    private float speed;
    private Vector2 moveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        isDead = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            isDead = !isDead;
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        speed = baseMoveSpeed;
        // rb.velocity = moveInput * speed;
        Vector2 pos = (Vector2)transform.position + moveInput * speed * Time.deltaTime;
        rb.MovePosition(pos);
    }

    #region Player Input
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>().normalized;
    }
    #endregion
}
