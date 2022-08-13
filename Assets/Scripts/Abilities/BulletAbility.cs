using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyAssets/BulletAbility")]
public class BulletAbility : Ability
{
    public GameObject pfBullet;
    public float bulletSpeed = 10f;

    public override void Activate(GameObject user)
    {
        base.Activate(user);

        GameObject b = Instantiate(pfBullet, user.transform.position, user.transform.rotation);
        int direction = user.GetComponent<PlayerController>().facingRight ? 1 : -1;
        b.GetComponent<Bullet>().SetVeclocity(new Vector2(direction * bulletSpeed, 0f));
    }
}
