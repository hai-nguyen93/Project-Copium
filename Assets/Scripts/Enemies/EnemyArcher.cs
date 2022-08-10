using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyArcher : EnemyBase
{
    public GameObject pfBullet;
    public Transform firePoint;
    public float aggroRange = 10f;
    public float bulletSpeed = 10f;
    public float fireCooldown = 1f;
    private bool canFire = true;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if ((distanceToPlayer < aggroRange) && canFire)
        {
            FireBullet();
        }

        if (player.transform.position.x < transform.position.x && facingRight) Flip();
        if (player.transform.position.x > transform.position.x && !facingRight) Flip();
    }

    IEnumerator FireCooldown(float time)
    {
        canFire = false;
        yield return new WaitForSeconds(time);
        canFire = true;
    }

    public void FireBullet()
    {
        Vector2 fireDirection = player.transform.position - firePoint.position;
        GameObject bullet = Instantiate(pfBullet, firePoint.position, transform.rotation);
        bullet.GetComponent<Bullet>().SetVeclocity(fireDirection * bulletSpeed);
        StartCoroutine(FireCooldown(fireCooldown));
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, new Vector3(0, 0, 1), aggroRange);
    }
}
