using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Surv_PlayerController : MonoBehaviour
{
    public SpriteRenderer sr;
    private CharacterController controller;
    private Surv_PlayerHP playerHp;
    private Surv_PlayerLevel playerLevel;

    [Header("Player Status")]
    public bool isDead = false;
    public bool facingRight;
    public int atk = 0;

    [Header("Movement Settings")]
    public float baseMoveSpeed = 2f;
    private float speed;
    private Vector3 moveInput;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerHp = GetComponent<Surv_PlayerHP>();
        playerLevel = GetComponent<Surv_PlayerLevel>();
        isDead = false;
    }

    private void Update()
    {
        /// for testing
        if (Input.GetKeyDown(KeyCode.J))
        {
            isDead = !isDead;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            playerHp.HealDamage(1);
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

    public void CheckPlayerHP()
    {
        isDead = playerHp.CheckIsDead();
        if (isDead)
        {
            Die();
        }
    }

    public void Die()
    {
        Surv_GameController.Instance.GameOver();
    }

    public void GainExp(int value)
    {
        playerLevel.GainExp(value);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        Surv_Enemy enemy = other.GetComponent<Surv_Enemy>();
        if (enemy)
        {
            Debug.Log("Player takes " + enemy.damage + " damage.");
            playerHp.ReceiveDamage(enemy.damage);
            CheckPlayerHP();
            enemy.HitPlayer();
            return;
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
