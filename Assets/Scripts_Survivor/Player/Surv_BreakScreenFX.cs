using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_BreakScreenFX : MonoBehaviour
{
    public Material mat;

    public IEnumerator CoroutineScreenShot()
    {
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;
        Texture2D scTex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        scTex.ReadPixels(rect, 0, 0);
        scTex.Apply();

        mat.SetTexture("_BaseMap", scTex);
    }
}
