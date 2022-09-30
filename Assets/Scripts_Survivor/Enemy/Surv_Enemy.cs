using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Collections;

public class Surv_Enemy : MonoBehaviour, IDamageable, ISpeedChange
{
    public Surv_PlayerController player;
    private Surv_EnemySpawner spawner;

    public Surv_EnemyData data;
    public int currentHP;
    public bool isDead = false;
    public bool isMoving = true;
    public bool facingRight;
    public int damage { get { return data.damage; } }
    public float speedModifier = 1f;
    private float speed;
    public Color dmgTextColor;
    public Color dieParticleColor;

    public virtual void Start()
    {
        isDead = false;
        currentHP = data.maxHp;
        isMoving = true;
        speedModifier = 1f;
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

        speed = data.baseSpeed * speedModifier;
        transform.Translate(speed * Time.deltaTime * directionToPlayer, Space.World);
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

    public void ReceiveDamage(int dmg)
    {
        Debug.Log(gameObject.name + " takes " + dmg + " damage.");
        if (PopupTextPool.instance != null)
        {
            PopupText3D p = PopupTextPool.instance.GetPopupText3D();
            Bounds b = GetComponent<BoxCollider>().bounds;
            Vector3 pos = transform.position + new Vector3(0f, b.size.y + 0.2f, 0f);
            p.Setup(dmg.ToString(), pos, dmgTextColor, true);
        }
        
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            if (player != null)
            {
                player.GainExp(data.exp);
                Surv_GameController.Instance.OnEnemyKilled(this);
            }
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

    public void ChangeSpeedModifier(float duration, float amount)
    {
        if (!data.canBeCC) return;
        StartCoroutine(ChangeSpeedCoroutine(duration, amount));
    }
    public IEnumerator ChangeSpeedCoroutine(float duration, float amount)
    {
        speedModifier = Mathf.Clamp(amount, 0f, 2f);
        yield return new WaitForSeconds(duration);
        speedModifier = 1f;
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
