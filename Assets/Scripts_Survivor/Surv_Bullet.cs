using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Surv_Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public int damage;
    public float lifeSpan = 2f;
    public List<Surv_BulletEffect> effects; 
    protected float timer;
    protected ObjectPool<Surv_Bullet> pool;

    [Header("Bullet Collision Settings")]
    public Transform detectPointOrigin;
    public float detectRadius = 0.1f;
    public LayerMask layerToCollide;
    public string targetTag;
    
    public void OnEnable()
    {
        timer = lifeSpan;
    }

    protected virtual void Update()
    {
        UpdateBulletTimer();
        UpdateBulletPosition();
        UpdateBulletCollision();
    }

    public void Setup(Vector3 position, Vector3 direction, float speed, int damage)
    {
        transform.position = position;
        this.direction = direction;
        this.speed = Mathf.Abs(speed);
        this.damage = damage;
    }

    public void SetPool(ObjectPool<Surv_Bullet> bulletPool)
    {
        pool = bulletPool;
    }

    public void UpdateBulletTimer()
    {
        if (timer <= 0f && gameObject.activeInHierarchy)
        {
            DestroyBullet();
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public virtual void UpdateBulletPosition()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    public virtual void UpdateBulletCollision()
    {
        var hits = Physics.OverlapSphere(detectPointOrigin.position, detectRadius, layerToCollide, QueryTriggerInteraction.Collide);       
        if (hits.Length > 0)
        {
            Collider hit = hits[0];
            if (hits.Length > 1) // if hit more than 1 objs, only take the closest obj
            {
                float minDistance = 999;
                foreach (var h in hits)
                {
                    float d = Vector3.Distance(h.transform.position, transform.position);
                    if (d <= minDistance)
                    {
                        minDistance = d;
                        hit = h;
                    }
                }
            }

            var target = hit.GetComponent<Collider>().GetComponent<IDamageable>();
            if (target != null)
            {
                target.ReceiveDamage(damage);
                foreach (var effect in effects)
                {
                    effect.Activate(hit.gameObject);
                }
            }
            DestroyBullet();
        }
    }

    public void DestroyBullet()
    {
        if (pool != null)
        {
            pool.Release(this);
        }
        else { Destroy(gameObject); }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectPointOrigin.position, detectRadius);
    }
}
