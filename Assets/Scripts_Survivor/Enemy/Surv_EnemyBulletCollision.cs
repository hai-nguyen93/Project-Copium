using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_EnemyBulletCollision : MonoBehaviour
{
    private Surv_EnemyBullet bullet;

    public bool collideWithGround = true;
    public LayerMask groundLayer;
    public Transform detectOrigin;
    public float detectRange = 0.1f;

    private void Start()
    {
        bullet = GetComponent<Surv_EnemyBullet>();
    }

    private void Update()
    {
        var hits = Physics.OverlapSphere(detectOrigin.position, detectRange, groundLayer);
        if (hits.Length > 0)
        {
            bullet.DestroyBullet();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bullet.HitPlayer(other.GetComponent<Surv_PlayerController>());
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (collideWithGround)
        {
            Gizmos.DrawWireSphere(detectOrigin.position, detectRange);
        }
    }
}
