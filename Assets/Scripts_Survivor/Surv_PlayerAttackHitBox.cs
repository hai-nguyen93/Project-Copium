using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerAttackHitBox : MonoBehaviour
{
    public int damage;

    [Header("UI pop-up damage")]
    public GameObject pfPopupDmg;
    public Color textColor;

    public void SetDamage(int value)
    {
        damage = value;
    }

    public void PopUpDamage(Vector3 position)
    {
        GameObject go = Instantiate(pfPopupDmg, position, Quaternion.identity);
        go.GetComponent<PopupText3D>().Setup(damage.ToString(), textColor, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Surv_Enemy enemy = other.GetComponent<Surv_Enemy>();
        if (enemy)
        {
            Debug.Log(enemy.gameObject.name + " takes " + damage + " damage.");

            Bounds b = enemy.GetComponent<BoxCollider>().bounds;
            Vector3 pos = enemy.transform.position + new Vector3(0f, b.size.y + 0.2f, 0f);
            PopUpDamage(pos);
            enemy.Die();
        }
    }
}
