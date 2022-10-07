using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerHP : MonoBehaviour
{
    public bool godMode = false;
    public int startMaxHP = 10;
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
        maxHP = startMaxHP;
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

    public void HealDamage(int value, bool spawnPopupText = true)
    {
        currentHP = Mathf.Clamp(currentHP + value, 0, maxHP);
        hpPanel.UpdateFillAmount(1f * currentHP / maxHP);
        if (spawnPopupText) CreatePopupText(value.ToString(), healTextColor);
    }

    public bool CheckIsDead()
    {
        return currentHP <= 0;
    }

    public void CreatePopupText(string text, Color color)
    {
        if (PopupTextPool.instance != null)
        {
            PopupText3D p = PopupTextPool.instance.GetPopupText3D();
            p.SimpleSetup(text, transform.position + textSpawnOffset, color);
        }
        else
        {
            GameObject go = Instantiate(pfPopupText3D);
            go.GetComponent<PopupText3D>().SimpleSetup(text, transform.position + textSpawnOffset, color);
        }
    }
}
