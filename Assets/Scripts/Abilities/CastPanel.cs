using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastPanel : MonoBehaviour
{
    public TextMeshProUGUI prompt;
    public Slider castBar;

    public void SetUp(string abilityName)
    {
        prompt.text = abilityName + " - Casting";
        castBar.value = 0f;
    }

    public void UpdateCastBar(float value)
    {
        castBar.value = value;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
