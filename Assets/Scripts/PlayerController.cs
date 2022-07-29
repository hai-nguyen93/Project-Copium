using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    private Vector2 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        UpdateMovement();
    }

    void UpdateMovement()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
    }

    public void OnMove(InputValue value)
    {
        moveDirection = new Vector2(value.Get<Vector2>().x, 0f);
    }
}
