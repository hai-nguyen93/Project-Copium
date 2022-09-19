using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCastShadow : MonoBehaviour
{
    private SpriteRenderer sr;

    public bool receiveShadow;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        sr.receiveShadows = receiveShadow;
    }
}
