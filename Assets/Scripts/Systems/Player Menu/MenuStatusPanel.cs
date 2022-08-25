using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuStatusPanel : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI atkText;
    public Image playerDisplay;

    public void OnEnable()
    {
        var player = PlayerData.Instance;
        if (player == null)
        {
            Debug.Log("Cannot find player data");
            return;
        }

        hpText.text = "HP: " + player.stats.hp + " / " + player.stats.maxHp;
        atkText.text = "Attack: " + player.stats.atk;
    }
}
