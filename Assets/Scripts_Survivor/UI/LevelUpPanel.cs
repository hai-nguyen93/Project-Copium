using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpPanel : MonoBehaviour
{
    public ParticleSystem lvlUpPs;
    public TextMeshProUGUI lvlUpInfoText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI atkText;
    public TextMeshProUGUI speedText;


    private void OnEnable()
    {
        if (!lvlUpPs.gameObject.activeSelf)
        {
            lvlUpPs.gameObject.SetActive(true);
            lvlUpPs.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        lvlUpPs.Play();
    }

    private void OnDisable()
    {
        if (lvlUpPs != null) lvlUpPs.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void SetLvlUpPanel(int level, int hp, int maxHp, int atk, float speed)
    {
        lvlUpInfoText.text = "Level: " + level + " -> " + (level + 1);
        hpText.text = "HP: " + hp + " / " +  maxHp;
        atkText.text = "ATTACK: " + atk;
        speedText.text = "SPEED: " + speed;
    }
}
