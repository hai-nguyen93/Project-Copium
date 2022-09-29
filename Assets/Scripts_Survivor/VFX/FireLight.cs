using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLight : MonoBehaviour
{
    public Light mainLight;
    private float baseIntensity;
    public AnimationCurve curve;

    private void Start()
    {
        baseIntensity = mainLight.intensity;
    }

    private void Update()
    {
        mainLight.intensity = curve.Evaluate(Time.time) * baseIntensity;
    }
}
