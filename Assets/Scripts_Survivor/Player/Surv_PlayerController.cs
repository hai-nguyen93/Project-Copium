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
    public Surv_PlayerCombat pCombat { get; private set; }

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
        playerHp = GetComponent<Surv_PlayerHP>();
        playerLevel = GetComponent<Surv_PlayerLevel>();
        pCombat = GetComponent<Surv_PlayerCombat>();
        isDead = false;
    }

    private void Update()
    {
        if (Surv_GameController.Instance.state != GameState.Gameplay) return;

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

    public void ReceiveDamage(int damage)
    {
        Debug.Log("Player takes " + damage + " damage.");
        playerHp.ReceiveDamage(damage);
        CheckPlayerHP();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        Surv_Enemy enemy = other.GetComponent<Surv_Enemy>();
        if (enemy)
        {
            ReceiveDamage(enemy.damage);
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