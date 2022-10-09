using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Surv_PlayerBulletAttack : Surv_PlayerAttack
{
    [Header("Bullet Pool Settings")]
    public Surv_Bullet pfBullet;
    public int initialAmount = 20;
    public int maxAmount = 20;
    public ObjectPool<Surv_Bullet> bulletPool;

    [Header("Bullet Attack Settings")]
    public Transform firePoint;
    public List<int> numOfBulletsAtLevel;
    public float angleBetweenBullets = 15f;
    public float bulletSpeed = 10f;
    public List<Surv_Bullet> activeBullets;

    protected override void Start()
    {
        base.Start();
        autoAttack = true;
        activeBullets = new List<Surv_Bullet>();

        // set up bullet pool
        bulletPool = new ObjectPool<Surv_Bullet>(() => // Create function
        {
            Surv_Bullet b = Instantiate(pfBullet);
            b.SetPool(bulletPool);
            return b;
        }, bullet =>    // Get bullet function
        {
            bullet.gameObject.SetActive(true);
            activeBullets.Add(bullet);
        }, bullet =>    // Release bullet function
        {
            bullet.gameObject.SetActive(false);
            activeBullets.Remove(bullet);
        }, bullet =>    // Destroy bullet function
        {
            Destroy(bullet.gameObject);
        }, true, initialAmount, maxAmount);
    }

    private void Update()
    {
        base.UpdateAttack();
    }

    public override void Attack()
    {
        int numOfBullets = numOfBulletsAtLevel[Mathf.Min(level, ATTACK_MAX_LV) - 1];
        float startAngle = angleBetweenBullets * (numOfBullets - 1) / 2;

        for (int i = 0; i < numOfBullets; ++i)
        {
            var b = bulletPool.Get();
            b.Setup(firePoint.position, Quaternion.Euler(0, startAngle, 0) * firePoint.right, bulletSpeed, damage);
            startAngle -= angleBetweenBullets;
        }
        ResetAttackTimer();
    }

    public override void OnValidate()
    {
        base.OnValidate();

        // Resize number of bullets fired at each level list
        if (numOfBulletsAtLevel.Count < ATTACK_MAX_LV)
        {
            for (int i = numOfBulletsAtLevel.Count; i < ATTACK_MAX_LV; ++i)
            {
                numOfBulletsAtLevel.Add(1);
            }
        }
    }
}
