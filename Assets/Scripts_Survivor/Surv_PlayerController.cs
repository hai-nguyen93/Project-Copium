using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Surv_PlayerController : MonoBehaviour
{
    public SpriteRenderer sr;
    private CharacterController controller;

    [Header("Player Status")]
    public bool isDead = false;
    public bool facingRight;

    [Header("Movement Settings")]
    public float baseMoveSpeed = 2f;
    private float speed;
    private Vector3 moveInput;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        isDead = false;
    }

    private void Update()
    {
        /// for testing
        if (Input.GetKeyDown(KeyCode.J))
        {
            isDead = !isDead;
        }
        ///

        if (isDead) return;

        speed = baseMoveSpeed;
        controller.Move(moveInput * speed * Time.deltaTime);
        if (!facingRight && moveInput.x > 0f) Flip();
        if (facingRight && moveInput.x < 0f) Flip();
    }

    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0f, 180f, 0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        Surv_Enemy enemy = other.GetComponent<Surv_Enemy>();
        if (enemy)
        {
            Debug.Log("Player takes " + enemy.damage + " damage.");
            enemy.HitPlayer();
        }
    }

    #region Player Input
    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveInput = new Vector3(input.x, 0f, input.y).normalized;
    }
    #endregion
}
