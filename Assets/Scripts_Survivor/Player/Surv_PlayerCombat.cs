using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerCombat : MonoBehaviour
{
    private Surv_PlayerController player;

    public int startAtk = 1;
    public int atk = 1;
    public List<Surv_PlayerAttack> attackList;

    public Surv_PlayerUltAttack pUlt;

    [Header("UI elements")]
    public HpPanel ultPanel;

    private void Start()
    {
        atk = startAtk;
        ultPanel.gameObject.SetActive(false);
        attackList = new List<Surv_PlayerAttack>();
        var attacks = GetComponentsInChildren<Surv_PlayerAttack>();
        foreach(var a in attacks)
        {
            if (a.CompareTag("PlayerUltimate"))
            {
                pUlt = a.GetComponent<Surv_PlayerUltAttack>();
                ultPanel.gameObject.SetActive(true);
                UpdateUltimateUI();
                continue;
            }
            attackList.Add(a);
        }
    }

    private void Update()
    {
        UpdateUltimateUI();

        // testing
        if (Input.GetKeyDown(KeyCode.R) && pUlt.ultReady)
        {
            pUlt.UseUltimate();
        }
        //
    }

    public void UpdateUltimateUI()
    {
        ultPanel.UpdateFillAmount(pUlt.GetCooldownFillAmount());
    }

    public void LearnAttack(int attackID)
    {
        //  get the attack prefab from databse
        var attackPrefab = Surv_GameController.Instance.playerAttackDB.GetPlayerAttack(attackID); 

        // check if already learned the attack
        foreach( var a in attackList)
        {
            if (a.attackID == attackPrefab.attackID)
            {
                Debug.Log("Already learned this attack or invalid attackID: " + attackID + " (ID fot from database: " + attackPrefab.attackID + ")");
                return;
            }
        }

        // instantiate the attack gameObj if valid
        var attack = Instantiate(attackPrefab, transform);
        attack.transform.localPosition = Vector3.zero;
        attack.transform.localRotation = Quaternion.Euler(0, 0, 0);
        attack.transform.localScale = Vector3.one;
        attackList.Add(attack);
    }
}
