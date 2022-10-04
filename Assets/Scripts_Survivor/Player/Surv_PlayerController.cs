using System.Collections;
using System.Collections.Generic;
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
    public float baseMoveSpeed = 2f;
    private float speed;
    private float speedModifier;
    private Vector3 moveInput;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerHp = GetComponent<Surv_PlayerHP>();
        playerLevel = GetComponent<Surv_PlayerLevel>();
        pCombat = GetComponent<Surv_PlayerCombat>();
        speedModifier = 1f;
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

        speed = baseMoveSpeed * speedModifier;
        Vector3 moveVector = speed * Time.deltaTime * moveInput;
        controller.Move(moveVector);
        anim.SetFloat("move", moveVector.magnitude);

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
    #endregion
}
