using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public int damage;
    public float lifeSpan = 2f;

    [Header("Bullit Collision Settings")]
    public bool useTriggerCollider;
    public Transform detectPointOrigin;
    public float detectRadius = 0.1f;
    public LayerMask layerToCollide;
    
    public void OnEnable()
    {
        lifeSpan = 2f;
    }

    private void Update()
    {
        UpdateBulletPosition();
    }

    public void Setup(Vector3 direction, float speed, int damage)
    {
        this.direction = direction;
        this.speed = Mathf.Abs(speed);
        this.damage = damage;
    }

    public void UpdateBulletPosition()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectPointOrigin.position, detectRadius);
    }
}
