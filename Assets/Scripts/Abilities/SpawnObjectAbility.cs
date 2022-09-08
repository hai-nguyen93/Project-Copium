using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnObjectType { bullet, bomb}

[CreateAssetMenu(menuName = "MyAssets/BulletAbility")]
public class SpawnObjectAbility : Ability
{
    public GameObject pfBullet;
    public float bulletSpeed = 10f;
    public SpawnObjectType objType;

    public override void Activate(GameObject user)
    {
        base.Activate(user);

        GameObject go = Instantiate(pfBullet, user.transform.position, user.transform.rotation);
        PlayerController pc = user.GetComponent<PlayerController>();
        switch (objType)
        {
            case SpawnObjectType.bullet:
                Bullet b = go.GetComponent<Bullet>();
                int direction = pc.facingRight ? 1 : -1;
                b.GetComponent<Bullet>().SetVeclocity(new Vector2(direction * bulletSpeed, 0f));
                break;

            case SpawnObjectType.bomb:
                go.transform.position = user.GetComponent<PlayerCombat>().attackPoint.position;
                break;
        }
    }
}
