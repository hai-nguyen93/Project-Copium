using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class HiddenAreaTrigger : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D col;
    private IEnumerator changeAlphaCoroutine;

    [Range(0f, 1f)] public float showAlpha = 0.3f;
    [Range(0f, 1f)] public float hideAlpha = 1f;
    public float changeSpeed = 1f;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        changeAlphaCoroutine = ChangeAlpha(sr.color.a, hideAlpha, changeSpeed);
        HideArea();
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ShowArea();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HideArea();
        }
    }

    public void HideArea()
    {
        if (sr.color.a >= hideAlpha) return;

        StopCoroutine(changeAlphaCoroutine);
        changeAlphaCoroutine = ChangeAlpha(sr.color.a, hideAlpha, changeSpeed);
        StartCoroutine(changeAlphaCoroutine);
    }

    public void ShowArea()
    {
        if (sr.color.a <= showAlpha) return;

        StopCoroutine(changeAlphaCoroutine);
        changeAlphaCoroutine = ChangeAlpha(sr.color.a, showAlpha, changeSpeed);
        StartCoroutine(changeAlphaCoroutine);
    }

    IEnumerator ChangeAlpha(float startAlpha, float endAlpha, float speed)
    {
        if (startAlpha == endAlpha) yield break;

        Color c = sr.color;
        int direction = (int)Mathf.Sign(endAlpha - startAlpha);
        float minAlpha = (startAlpha < endAlpha) ? startAlpha : endAlpha;
        float maxAlpha = (startAlpha > endAlpha) ? startAlpha : endAlpha;

        while (Mathf.Abs(c.a - endAlpha) > 0.01f)
        {
            c.a = Mathf.Clamp(c.a + (direction * speed * Time.deltaTime), minAlpha, maxAlpha);
            sr.color = c;
            yield return null;
        }

        c.a = endAlpha;
        sr.color = c;
    }
}
