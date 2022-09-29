using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpPanel : MonoBehaviour
{
    public ParticleSystem lvlUpPs;
    public TextMeshProUGUI lvlUpInfoText;

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

    public void SetLvlUpInfoText(int level)
    {
        lvlUpInfoText.text = "Level: " + level + " -> " + (level + 1);
    }
}
