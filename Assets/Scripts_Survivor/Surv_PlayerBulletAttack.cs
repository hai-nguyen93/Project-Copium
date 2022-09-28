using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Surv_PlayerBulletAttack : Surv_PlayerAttack
{
    [Header("Bullet Pool Settings")]
    public Surv_Bullet pfBullet;
    public int initialAmount = 10;
    public int maxAmount = 20;
    public ObjectPool<Surv_Bullet> bulletPool;

    [Header("Bullet Attack Settings")]
    public int fireAmount = 1;
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
            return Instantiate(pfBullet);
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
        var b = bulletPool.Get();
    }
}
