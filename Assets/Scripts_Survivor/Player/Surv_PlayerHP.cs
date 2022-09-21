using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerHP : MonoBehaviour
{
    public bool godMode = false;
    public int maxHP = 20;
    public int currentHP;

    [Header("UI elements")]
    public HpPanel hpPanel;
    public GameObject pfPopupText3D;
    public Color healTextColor;
    public Color dmgTextColor;
    public Vector3 textSpawnOffset;

    private void Start()
    {
        currentHP = maxHP;
        hpPanel.UpdateFillAmount(1f);
    }

    /// <summary>
    /// Player takes 'value' damage
    /// </summary>
    /// <param name="value"></param>
    public void ReceiveDamage(int value)
    {
        CreatePopupText(value.ToString(), dmgTextColor);
        if (godMode) return;
        currentHP = Mathf.Clamp(currentHP - value, 0, maxHP);
        hpPanel.UpdateFillAmount(1f * currentHP / maxHP);
    }

    public void HealDamage(int value)
    {
        currentHP = Mathf.Clamp(currentHP + value, 0, maxHP);
        hpPanel.UpdateFillAmount(1f* currentHP / maxHP);
        CreatePopupText(value.ToString(), healTextColor);
    }

    public bool CheckIsDead()
    {
        return currentHP <= 0;
    }

    public void CreatePopupText(string text, Color color)
    {
        GameObject go = Instantiate(pfPopupText3D, transform.position + textSpawnOffset, Quaternion.identity);
        go.GetComponent<PopupText3D>().SimpleSetup(text, color);
    }
}
