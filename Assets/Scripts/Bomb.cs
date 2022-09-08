using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bomb : MonoBehaviour
{
    public int power = 1;
    public float timeToExplode = 5f;
    private float timer;
    [Tooltip("Is this from enemy (hit player)?")] public bool isEnemy;
    public Color defaultColor = Color.white;
    public Color dangerColor = Color.red;
    public AnimationCurve flashCurve;
    private SpriteRenderer sr;

    [Header("Bomb collision settings")]
    public bool explodeOnCollision = false;
    public Transform detectPointOrigin;
    public float radius = 5f;
    public LayerMask layerToDetect;
    public ParticleSystem explosionPS;
    private float explosionDuration;
    private bool exploded = false;

    [Header("Ground collision settings")]
    public LayerMask groundLayer;
    public float fallSpeed = -3f;
    private float ySpeed = 0f;
    public bool isGrounded;

    private void Start()
    {
        timer = timeToExplode;
        sr = GetComponent<SpriteRenderer>();
        isGrounded = false;
        explosionPS.Stop();
        explosionDuration = explosionPS.main.duration;
        exploded = false;

        // adjust particles' size to match bomb radius
        var mainPS = explosionPS.main;
        mainPS.startSpeed = new ParticleSystem.MinMaxCurve(radius * 1.5f, radius * 2.2f);
        var subPS = explosionPS.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        subPS.startSize = radius * 2;
    }

    private void Update()
    {
        if (exploded) return;

        UpdateGravity();
        
        // Update timer for explosion
        timer -= Time.deltaTime;

        float t = flashCurve.Evaluate(1 - timer / timeToExplode);
        sr.color = Color.Lerp(defaultColor, dangerColor, t);

        if (timer <= 0f)
        {
            Explode();
        }
    }

    private void UpdateGravity()
    {
        if (isGrounded) return;

        var hit = Physics2D.Raycast(detectPointOrigin.position, Vector2.down, 0.05f, groundLayer);
        if (hit)
        {
            ySpeed = 0f;
            isGrounded = true;

            // snap bomb position to ground
            transform.position = hit.point;
            return;
        }
        else
        {
            ySpeed = fallSpeed;
            isGrounded = false;
        }

        transform.Translate(new Vector3(0, ySpeed * Time.deltaTime, 0f));
    }

    public void Explode()
    {
        explosionPS.Play();
        var hits = Physics2D.OverlapCircleAll(detectPointOrigin.position, radius, layerToDetect);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (isEnemy) // if bomb is from enemy
                {
                    if (hit.gameObject.CompareTag("Player"))
                    {
                        Debug.Log(gameObject.name + " explodes hit player");
                    }
                }
                else // if bomb is from player
                {
                    Debug.Log(gameObject.name + " explodes hit " + hit.gameObject.name);
                }
            }
        }

        exploded = true;
        gameObject.GetComponent<Collider2D>().enabled = false;
        sr.enabled = false;
        Destroy(gameObject, explosionDuration + 0.1f);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemy) // if bomb is from enemy
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Explode();
            }
        }
        else // if bomb is from player
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Explode();
            }
        }
    }

    public void SetPower(int value)
    {
        power = value;
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(detectPointOrigin.position, new Vector3(0, 0, 1), radius);
    }
}
