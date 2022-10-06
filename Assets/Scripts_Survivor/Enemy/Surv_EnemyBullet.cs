using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Surv_EnemyBullet : MonoBehaviour
{
    public float lifeSpan = 2f;
    public Vector3 direction;
    public float speed = 5f;
    public int damage = 1;

    public List<Surv_BulletEffect> effects;

    private void Start()
    {
        StartCoroutine(DestroyAfterSeconds(lifeSpan));
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    private IEnumerator DestroyAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        DestroyBullet();
    }

    public void DestroyBullet()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public void Setup(Vector3 direction, float speed, int damage)
    {
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
    }

    public void HitPlayer(Surv_PlayerController player)
    {
        if (player.isDead) return;

        player.ReceiveDamage(damage);

        foreach(var e in effects)
        {
            e.Activate(player.gameObject);
        }

        DestroyBullet();
    }
}
