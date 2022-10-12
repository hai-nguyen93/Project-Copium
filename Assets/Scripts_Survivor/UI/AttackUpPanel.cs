using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackUpPanel : MonoBehaviour
{
    public ParticleSystem atkUpPs;
    private Surv_PlayerCombat pCombat;
    public Button[] upgradeButtons;

    private void OnEnable()
    {
        if (!atkUpPs.gameObject.activeSelf)
        {
            atkUpPs.gameObject.SetActive(true);
            atkUpPs.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        atkUpPs.Play();
    }

    private void OnDisable()
    {
        if (atkUpPs != null) atkUpPs.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void Setup()
    {
        pCombat = Surv_GameController.Instance.player.pCombat;
        var availableAttackIds = pCombat.GetAvailableUpgrades();

        if (availableAttackIds == null || availableAttackIds.Count == 0)
        {
            Debug.Log("All attacks are maxed Lvl, increase ATK instead");
            for (int i = 0; i < 3; ++i)
            {
                upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "ATK + 1";
                upgradeButtons[i].onClick.RemoveAllListeners();
                upgradeButtons[i].onClick.AddListener(() => { pCombat.atk += 1; });
                upgradeButtons[i].onClick.AddListener(() => Surv_GameController.Instance.ResumeGame());
            }
        }
        else
        {
            for (int i = 0; i < 3; ++i)
            {
                int id = availableAttackIds[Random.Range(0, availableAttackIds.Count)];
                var a = pCombat.GetLearnedAttack(id);
                string txt = "";
                if (a)
                {
                    txt = a.attackName + " - Lv: " + a.level + " -> " + (a.level + 1);
                }
                else
                {
                    txt = "Learn " + pCombat.attackDict[id].attackName;
                }

                upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = txt;
                upgradeButtons[i].onClick.RemoveAllListeners();
                upgradeButtons[i].onClick.AddListener(() => pCombat.LearnAttack(id));
                upgradeButtons[i].onClick.AddListener(() => Surv_GameController.Instance.ResumeGame());
            }
        }
    }
}
