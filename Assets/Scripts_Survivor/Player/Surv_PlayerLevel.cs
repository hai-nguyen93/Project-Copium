using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Surv_PlayerLevel : MonoBehaviour
{
    private Surv_PlayerController player;
    private Surv_PlayerHP playerHp;

    public int level = 1;
    public int exp = 0;
    public int expToLevelUp = 10;

    [Header("UI elements")]
    public LevelUpPanel lvlUpPanel;
    public TextMeshProUGUI levelText;
    public HpPanel expPanel;

    private void Start()
    {
        player = GetComponent<Surv_PlayerController>();
        playerHp = GetComponent<Surv_PlayerHP>();
        ResetLevel();
    }

    public void GainExp(int value)
    {
        exp += value;
        UpdateUI();
        CheckExp();
    }

    public void LevelUp()
    {
        Surv_GameController.Instance.PlayerLevelUp();
        lvlUpPanel.SetLvlUpPanel(level, playerHp.currentHP, playerHp.maxHP, player.pCombat.atk, player.baseMoveSpeed);    
    }

    public void CheckExp()
    {
        if (exp >= expToLevelUp)
        {
            LevelUp();
        }
    }

    public void UpgradeHp(int value)
    {       
        playerHp.maxHP += value;
        FinishLevelUp();
    }

    public void UpgradeAtk(int value)
    {
        player.pCombat.atk += value;
        FinishLevelUp();
    }

    public void FinishLevelUp()
    {
        // Calculate next exp need to lvl up
        expToLevelUp += (int)(Mathf.Ceil(level / 10f) * 3);
        
        level += 1;
        
        playerHp.HealDamage(playerHp.maxHP, false);
        Surv_GameController.Instance.ResumeGame();

        // check remaining exp after level up
        int remainingExp = exp - expToLevelUp;
        exp = 0;
        if (remainingExp > 0)
        {
            GainExp(remainingExp);
        }
        else
        {
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        levelText.text = "Lv: " + level;
        expPanel.UpdateFillAmount(Mathf.Clamp01(1f * exp / expToLevelUp));
    }

    public void ResetLevel()
    {
        level = 1;
        exp = 0;
        UpdateUI();
    }
}
