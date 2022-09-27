using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Collections;

public class Surv_Enemy : MonoBehaviour
{
    public Surv_PlayerController player;
    private Surv_EnemySpawner spawner;

    public Surv_EnemyData data;
    public int currentHP;
    public bool isDead = false;
    public bool isMoving = true;
    public bool facingRight;
    public int damage { get { return data.damage; } }
    private float speed;

    public virtual void Start()
    {
        isDead = false;
        currentHP = data.maxHp;
        isMoving = true;
    }

    void Update()
    {
        if (Surv_GameController.Instance.useMultiThread) return;

        UpdatePosition();
    }

    public virtual void UpdatePosition()
    {
        if (Surv_GameController.Instance.state != GameState.Gameplay) return;
        if (player == null || player.isDead || !isMoving || isDead) return;

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
        if (spawner != null) spawner.OnEnemyDie(this);
        Destroy(gameObject);
    }

    public void SetSpanwer(Surv_EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    public void SetPlayer(Surv_PlayerController player)
    {
        this.player = player;
    }
}

public struct Surv_EnemyTransformJob : IJobParallelForTransform
{
    public Vector3 playerPos;
    public float deltaTime;
    [ReadOnly] public NativeArray<float> speed;
    public NativeArray<bool> facingRight;

    public void Execute(int index, TransformAccess transform)
    {
        Vector3 directionToPlayer = (playerPos - transform.position).normalized;

        // look at player
        if (facingRight[index] && directionToPlayer.x < 0f) Flip(index, transform);
        if (!facingRight[index] && directionToPlayer.x > 0f) Flip(index, transform);

        transform.position += (directionToPlayer * speed[index] * deltaTime);
    }

    public void Flip(int index, TransformAccess transform)
    {
        facingRight[index] = !(facingRight[index]);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 180f, 0));
    }
}
