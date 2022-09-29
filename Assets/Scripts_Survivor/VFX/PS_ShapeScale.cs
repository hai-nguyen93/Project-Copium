using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_ShapeScale : MonoBehaviour
{
    public ParticleSystem ps;
    public bool scaleWithScreenWidth;
    [Range(0, 100f)] public float percentage = 100f;

    private void OnEnable()
    {
        var psShape = ps.shape;
        psShape.scale = new Vector3(Screen.width * percentage / 100, 1f, 1f);
    }
}
