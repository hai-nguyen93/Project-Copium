using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Surv_PlayerController : MonoBehaviour, IDamageable, ISpeedChange
{
    public SpriteRenderer sr;
    public Animator anim;
    private CharacterController controller;
    public Surv_PlayerHP playerHp { get; private set; }
    public Surv_PlayerLevel playerLevel { get; private set; }
    public Surv_PlayerCombat pCombat { get; private set; }

    [Header("Player Status")]
    public bool isDead = false;
    public bool facingRight;

    [Header("Movement Settings")]
    public bool canMove = true;
    [Tooltip("Speed at level 1")] public float startMoveSpeed = 3f;
    public float baseMoveSpeed = 2f;
    private float speed;
    private float speedModifier;
    private Vector3 moveInput;
    public event Action<bool> OnPlayerFlipped;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerHp = GetComponent<Surv_PlayerHP>();
        playerLevel = GetComponent<Surv_PlayerLevel>();
        pCombat = GetComponent<Surv_PlayerCombat>();
    }

    private void Start()
    {
        sr.enabled = true;
        canMove = true;
        baseMoveSpeed = startMoveSpeed;
        speedModifier = 1f;
        isDead = false;
    }

    private void Update()
    {
        if (Surv_GameController.Instance.state != GameState.Gameplay) return;

        /// for testing
        if (Input.GetKeyDown(KeyCode.J))
        {
            Surv_DamagedOverTime dot;
            if (!TryGetComponent<Surv_DamagedOverTime>(out dot))
            {
                dot = gameObject.AddComponent<Surv_DamagedOverTime>();
            }
            dot.Setup(1, 5);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            playerHp.HealDamage(1);
        }
        ///

        if (isDead) return;

        speed = baseMoveSpeed * speedModifier;
        Vector3 moveVector = (canMove)? speed * moveInput : Vector3.zero;
        controller.Move(Time.deltaTime * moveVector);
        anim.SetFloat("move", moveVector.magnitude);

        if (!facingRight && moveInput.x > 0f) Flip();
        if (facingRight && moveInput.x < 0f) Flip();       
    }

    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0f, 180f, 0f));
        OnPlayerFlipped?.Invoke(facingRight);
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
        sr.enabled = false;
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

        // play damaged animation if not dead
        if (!isDead) { anim.Play("player_damaged"); }
    }

    public void ChangeSpeedModifier(float duration, float amount)
    {
        StartCoroutine(ChangeSpeedCoroutine(duration, amount));
    }
    public IEnumerator ChangeSpeedCoroutine(float duration, float amount)
    {
        speedModifier = Mathf.Clamp(amount, 0f, 2f);
        yield return new WaitForSeconds(duration);
        speedModifier = 1f;
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

    public void OnPause(InputValue value)
    {
        float input = value.Get<float>();
        if (input > 0.5f)
        {
            if (Surv_GameController.Instance.state == GameState.Gameplay)
            {
                Surv_GameController.Instance.PauseGame();
            }
            else if (Surv_GameController.Instance.state == GameState.Pause)
            {
                Surv_GameController.Instance.ResumeGame();
            }
        }
    }
    #endregion
}
