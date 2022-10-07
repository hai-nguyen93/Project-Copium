using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderFlashWhenFilled : MonoBehaviour
{
    public HpPanel panel;
    public Image fillImage;

    private Color startColor;
    public Color flashColor;
    public AnimationCurve curve;

    private void Start()
    {
        startColor = fillImage.color;
    }

    private void Update()
    {
        if (panel.fillAmount >= 1)
        {
            float t = curve.Evaluate(Time.time);
            fillImage.color = Color.Lerp(startColor, flashColor, t);
        }
        else
        {
            fillImage.color = startColor;
        }
    }
}
