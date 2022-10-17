using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_Sword : MonoBehaviour
{
    public Surv_PlayerAttackHitBox hitbox;
    public BoxCollider box;
    public SpriteRenderer sr;

    public void Show()
    {
        sr.enabled = true;
        hitbox.enabled = true;
        box.enabled = true;
    }

    public void Hide()
    {
        sr.enabled = false;
        hitbox.enabled = false;
        box.enabled = false;
    }
}
