using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_BreakScreenFX : MonoBehaviour
{
    public Animator anim;
    public Material mat;
    public Camera cam;
    public ParticleSystem slashPS;
    private float psDuration;
    public float psDurationWaitOffset;

    public float animationLength;

    private void OnEnable()
    {
        gameObject.transform.rotation = Quaternion.identity;
        cam.gameObject.SetActive(false);
        anim.enabled = false;
        slashPS.Stop();
        psDuration = slashPS.main.duration;
    }

    public IEnumerator CoroutineBreakScreen()
    {
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;
        Texture2D scTex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        scTex.ReadPixels(rect, 0, 0);
        scTex.Apply();

        mat.SetTexture("_BaseMap", scTex);
        cam.gameObject.SetActive(true);

        slashPS.Play();
        yield return new WaitForSecondsRealtime(psDuration + Mathf.Abs(psDurationWaitOffset));
        anim.enabled = true;
        anim.Play("BreakScreen");
        yield return new WaitForSecondsRealtime(animationLength);
        anim.enabled = false;
    }
}
