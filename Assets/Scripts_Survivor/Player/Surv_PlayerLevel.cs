using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Surv_PlayerLevel : MonoBehaviour
{
    private Surv_PlayerController player;
    public int level = 1;
    public int exp = 0;
    public int expToLevelUp = 10;

    [Header("UI elements")]
    public TextMeshProUGUI levelText;
    public HpPanel expPanel;
    public GameObject pfPopuptext3D;
    public Color textColor;

    private void Start()
    {
        player = GetComponent<Surv_PlayerController>();
        ResetLevel();
        UpdateUI();
    }

    public void GainExp(int value)
    {
        exp += value;
        CheckExp();
        UpdateUI();
    }

    public void LevelUp()
    {
        level++;
        player.PlayerLevelUp();

        GameObject go = Instantiate(pfPopuptext3D, transform.position + new Vector3(0f, 1.5f, 0f), Quaternion.identity);
        go.GetComponent<PopupText3D>().SimpleSetup("LEVEL UP!", textColor);

        int remainingExp = exp - expToLevelUp;
        exp = 0;
        if (remainingExp > 0)
        {
            GainExp(remainingExp);
        }
    }

    public void CheckExp()
    {
        if (exp >= expToLevelUp)
        {
            LevelUp();
        }
    }

    public void UpdateUI()
    {
        levelText.text = "Lv: " + level;
        expPanel.UpdateFillAmount(1f * exp / expToLevelUp);
    }

    public void ResetLevel()
    {
        level = 1;
        exp = 0;
    }
}
