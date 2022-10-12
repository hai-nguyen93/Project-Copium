using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_PlayerCombat : MonoBehaviour
{
    private Surv_PlayerController player;
    public Surv_PlayerAttackDatabase playerAttackDB;
    public Dictionary<int, Surv_PlayerAttack> attackDict;

    public int MAX_NUM_OF_ATTACKS = 10;
    public int startAtk = 1;
    public int atk = 1;
    public List<Surv_PlayerAttack> learnedAttacks;

    public Surv_PlayerUltAttack pUlt;

    [Header("UI elements")]
    public HpPanel ultPanel;

    private void Awake()
    {
        player = GetComponent<Surv_PlayerController>();

        // initialize attack dictionary
        attackDict = new Dictionary<int, Surv_PlayerAttack>();
        foreach (var a in playerAttackDB.attackList)
        {
            attackDict.Add(a.attackID, a);
        }
    }

    private void Start()
    {
        atk = startAtk;
        ultPanel.gameObject.SetActive(false);
        learnedAttacks = new List<Surv_PlayerAttack>();
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
            if (learnedAttacks.Count < MAX_NUM_OF_ATTACKS) learnedAttacks.Add(a);
            else Destroy(a.gameObject);
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
        var attackPrefab = attackDict[attackID]; 

        // check if already learned the attack
        foreach( var a in learnedAttacks)
        {
            if (a.attackID == attackID)
            {
                a.AttackLevelUp();
                return;
            }
        }

        // learn new attack, instantiate the attack gameObj 
        var attack = Instantiate(attackPrefab, transform);
        attack.transform.localPosition = Vector3.zero;
        attack.transform.localRotation = Quaternion.Euler(0, 0, 0);
        attack.transform.localScale = Vector3.one;
        learnedAttacks.Add(attack);
    }

    public List<int> GetAvailableUpgrades()
    {
        List<int> result = new List<int>();
        if (learnedAttacks.Count >= MAX_NUM_OF_ATTACKS)
        {
            foreach(var a in learnedAttacks)
            {
                if (!a.maxedLvl)
                {
                    result.Add(a.attackID);
                }
            }
        }
        else
        {
            result = new List<int>(attackDict.Keys);
            foreach (var a in learnedAttacks)
            {
                if (a.maxedLvl)
                {
                    result.RemoveAll(id => id == a.attackID);
                }
            }
        }

        return result;
    }

    public bool LearnedAttack(int attackID)
    {
        foreach (var a in learnedAttacks)
        {
            if (a.attackID == attackID) return true;
        }
        return false;
    }

    public Surv_PlayerAttack GetLearnedAttack(int attackID)
    {
        foreach (var a in learnedAttacks)
        {
            if (a.attackID == attackID) return a;
        }
        return null;
    }
}
