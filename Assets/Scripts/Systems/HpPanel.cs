using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpPanel : MonoBehaviour
{
    public Slider fillImage;
    public float fillAmount = 1f;

    public void UpdateFillAmount(float amount)
    {
        fillAmount = Mathf.Clamp01(amount);
        fillImage.value = fillAmount;
    }
}
