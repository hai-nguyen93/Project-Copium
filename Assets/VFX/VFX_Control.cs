using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFX_Control : MonoBehaviour
{
    public VisualEffect vfx;

    public bool playOnAwake;

    private void Start()
    {
        if (playOnAwake) vfx.Play();
        else vfx.Stop();
    }
}
