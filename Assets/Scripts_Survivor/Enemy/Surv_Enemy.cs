using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Collections;

public class Surv_Enemy : MonoBehaviour, IDamageable, ISpeedChange
{
    public Surv_PlayerController player;
    protected Rigidbody rb;
    public SpriteRenderer sr;
    protected MaterialPropertyBlock matPropBlock;
    protected Surv_EnemySpawner spawner;

    // Stats
    public Surv_EnemyData data;
    public int currentHP;
    public int damage;
    public int exp;
    public float speedModifier = 1f;
    public float speed;
    
    public bool isDead = false;
    public bool canMove = true;
    public bool facingRight;
    public Color dieParticleColor;

    // Events
    public event Action<Surv_Enemy> OnEnemyDie;
    
    public virtual void OnEnable()
    {
        isDead = false;
        canMove = true;
        speedModifier = 1f;
        InitStat();
        matPropBlock = new MaterialPropertyBlock();
    }

    public void InitStat()
    {
        currentHP = data.maxHp;
        damage = data.damage;
        exp = data.exp;
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Update()
    {
        if (Surv_GameController.Instance.useMultiThread) return;

        if (Surv_GameController.Instance.state != GameState.Gameplay) return;
        if (player == null || player.isDead || isDead) return;       

        ChasePlayer();
    }

    public void ChasePlayer()
    {
        if (!canMove) return;

        LookAtPlayer();
        Vector3 playerPos = player.transform.position;
        Vector3 directionToPlayer = new Vector3(playerPos.x - transform.position.x, 0, playerPos.z - transform.position.z).normalized;       

        speed = data.baseSpeed * speedModifier;
        transform.Translate(speed * Time.deltaTime * directionToPlayer, Space.World);
    }

    public void LookAtPlayer()
    {
        float xDirToPlayer = player.transform.position.x - transform.position.x;
        if (facingRight && xDirToPlayer < 0f) Flip();
        if (!facingRight && xDirToPlayer > 0f) Flip();
    }

    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0f, 180f, 0f));
    }

    public virtual void HitPlayer()
    {
        Die();
    }

    public void ReceiveDamage(int dmg)
    {
        if (isDead) return;

        Debug.Log(gameObject.name + " takes " + dmg + " damage.");
        if (PopupTextPool.instance != null) // create pop up damage number
        {
            PopupText3D p = PopupTextPool.instance.GetPopupText3D();
            Bounds b = GetComponent<BoxCollider>().bounds;
            Vector3 pos = transform.position + new Vector3(0f, b.size.y + 0.2f, 0f);
            p.Setup(dmg.ToString(), pos, PopupTextPool.instance.dmgTextColor, true);
        }
        
        currentHP -= dmg;       

        if (currentHP <= 0) // check hp whether enemy dies or not
        {
            if (player != null)
            {
                player.GainExp(exp);
                Surv_GameController.Instance.OnEnemyKilled(this);
            }
            Die(true);
            return;
        }

        // Play take dmg animation
        StopCoroutine(FlashSprite());
        StartCoroutine(FlashSprite());
    }

    public IEnumerator FlashSprite()
    {
        sr.GetPropertyBlock(matPropBlock);
        matPropBlock.SetColor("_TintColor", Color.red);
        sr.SetPropertyBlock(matPropBlock);
        yield return new WaitForSeconds(0.2f);
        matPropBlock.SetColor("_TintColor", Color.white);
        sr.SetPropertyBlock(matPropBlock);
    }

    public virtual void Die(bool spawnItem = false)
    {
        OnEnemyDie?.Invoke(this);

        isDead = true;
        if (spawner != null) spawner.OnEnemyDie(this, spawnItem);
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

    public void PushBack(Vector3 direction, float power)
    {
        if (!data.canBeCC) return;

        rb.AddForce(power * direction, ForceMode.Impulse);
    }

    [ContextMenu("Test Push back, force = 10")]
    public void TestPushBack()
    {
        PushBack(Vector3.right, 10f);
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
