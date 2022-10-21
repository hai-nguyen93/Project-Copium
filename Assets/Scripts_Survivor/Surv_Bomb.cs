using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Surv_Bomb : MonoBehaviour
{
    public SpriteRenderer sprite;
    public GameObject telegraph;
    public VisualEffect explosionVFX;
    private bool exploded;

    [Header("Bomb Settings")]
    public int damage = 1;
    public float timeToExplode = 5f;
    private float timer;
    public float whenToShowTelegraph = 2.5f;
    public float explodeRadius = 5f;
    public float destroyAfterSeconds = 0.5f;

    [Header("Color animation")]
    public Color defaultColor = Color.white;
    public Color flashColor = Color.red;
    public AnimationCurve flashCurve;

    [Header("Collision Settings")]
    public bool hitPlayer;
    public bool hitEnemy;
    public LayerMask targetLayer;
    public Vector3 originOffset;

    private void Start()
    {
        if (telegraph)
        {
            telegraph.transform.localScale = new Vector3(explodeRadius * 2, explodeRadius * 2, 0);
            telegraph.SetActive(false);
        }

        if (explosionVFX)
        {
            explosionVFX.SetFloat("Radius", explodeRadius);
            explosionVFX.Stop();
        }

        timer = timeToExplode;
        exploded = false;
    }

    private void Update()
    {
        if (exploded) return;

        if (timer <= 0f) { Explode(); }
        else
        {
            if (telegraph)
            {
                // show telegraph
                if ((timeToExplode - timer) >= whenToShowTelegraph && !telegraph.activeInHierarchy)
                {
                    telegraph.SetActive(true);
                }
            }

            float t = flashCurve.Evaluate(1 - timer / timeToExplode);
            sprite.color = Color.Lerp(defaultColor, flashColor, t);

            timer -= Time.deltaTime;
        }
    }

    public void Explode()
    {
        exploded = true;
        if (telegraph) { telegraph.SetActive(false); }
        sprite.enabled = false;
        if (explosionVFX) explosionVFX.Play();

        var hits = Physics.OverlapSphere(transform.position + originOffset, explodeRadius, targetLayer);

        if (hitPlayer)
        {
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    hit.GetComponent<Surv_PlayerController>().ReceiveDamage(damage);
                }
            }
        }
        if (hitEnemy)
        {
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<Surv_Enemy>(out var enemy))
                {
                    enemy.ReceiveDamage(damage);
                }
            }
        }

        Destroy(gameObject, destroyAfterSeconds);
    }

    public void SetDamage(int dmg) { damage = dmg; }
}
