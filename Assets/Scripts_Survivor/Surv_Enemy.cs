using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_Enemy : MonoBehaviour
{
    private Surv_PlayerController player;

    public Surv_EnemyData data;
    public int currentHP;
    public bool isDead = false;
    public bool facingRight;
    public int damage { get { return data.damage; } }
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        currentHP = data.maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBehaviour();
    }

    public void UpdateBehaviour()
    {
        if (player == null || player.isDead || isDead) return;

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        // look at player
        if (facingRight && directionToPlayer.x < 0f) Flip();
        if (!facingRight && directionToPlayer.x > 0f) Flip();

        speed = data.baseSpeed;
        transform.Translate(directionToPlayer * speed * Time.deltaTime, Space.World);
    }

    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0f, 180f, 0f));
    }

    public void HitPlayer()
    {
        Die();
    }

    public void HitByPlayer(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            player.GainExp(data.exp);
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }

    public void SetPlayer(Surv_PlayerController player)
    {
        this.player = player;
    }
}
