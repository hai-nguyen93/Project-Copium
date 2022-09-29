using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerCombat : MonoBehaviour
{
    private Surv_PlayerController player;

    public int atk = 0;
    public List<Surv_PlayerAttack> attackList;

    private Surv_PlayerUltAttack pUlt;

    [Header("UI elements")]
    public HpPanel ultPanel;

    private void Start()
    {
        ultPanel.gameObject.SetActive(false);
        attackList = new List<Surv_PlayerAttack>();
        var attacks = GetComponentsInChildren<Surv_PlayerAttack>();
        foreach(var a in attacks)
        {
            attackList.Add(a);
            if (a.CompareTag("PlayerUltimate"))
            {
                pUlt = a.GetComponent<Surv_PlayerUltAttack>();
                ultPanel.gameObject.SetActive(true);
                UpdateUltimateUI();
            }
        }
    }

    private void Update()
    {
        UpdateUltimateUI();
    }

    public void AddAttack(Surv_PlayerAttack newAttack)
    {
        if (attackList.Contains(newAttack))
        {
            Debug.Log("Player already has " + newAttack.name);
            return;
        }

        // Generate newAttack GameObject then attach to player
        attackList.Add(newAttack);
    }

    public void UpdateUltimateUI()
    {
        ultPanel.UpdateFillAmount(pUlt.GetCooldownFillAmount());
    }
}
