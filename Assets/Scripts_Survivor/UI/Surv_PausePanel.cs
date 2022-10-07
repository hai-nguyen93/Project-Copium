using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Surv_PausePanel : MonoBehaviour
{
    [Header("Status Panel")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI atkText;
    public TextMeshProUGUI speedText;

    public void SetupStatusPanel(Surv_PlayerController player)
    {
        levelText.text = "Level: " + player.playerLevel.level;
        hpText.text = "HP: " + player.playerHp.currentHP + " / " + player.playerHp.maxHP;
        atkText.text = "ATK: " + player.pCombat.atk;
        speedText.text = "Speed: " + player.baseMoveSpeed;
    }
}
