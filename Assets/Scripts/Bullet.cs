using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bullet : MonoBehaviour
{
    public Vector2 velocity = Vector2.zero;
    public float lifeSpan = 5f;
    public int power = 1;
    [Tooltip("Is this from enemy (hit player)?")] public bool isEnemy = false;

    [Header("Bullet collision settings")]
    public Transform detectPointOrigin;
    public float detectRange = 0.25f;
    public LayerMask layerToDetect;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeSpan);
    }

    // Update is called once per frame
    void Update()
    {
        var hits = Physics2D.OverlapCircleAll(detectPointOrigin.position, detectRange, layerToDetect);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
                HandleCollision(hit);
        }

        transform.position += new Vector3(velocity.x, velocity.y, 0f) * Time.deltaTime;
    }

    public void SetVeclocity(Vector2 vel)
    {
        velocity = vel;
    }

    public void HandleCollision(Collider2D collider)
    {
        if (isEnemy) // if bullet is from enemy
        {
            if (collider.CompareTag("Player"))
            {
                //if (PlayerData.Instance.isDead) return;

                Debug.Log("bullet hit player");
                collider.GetComponent<PlayerController>().Stagger((int)velocity.x);
                Destroy(gameObject);
                return;
            }
        }
        else // if bullet is from player
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("Player bullet hit enemy");
                Destroy(gameObject);
                return;
            }
        }
        
        // if hit ground
        Debug.Log("bullet hit platform");
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectPointOrigin.position, detectRange);
    }
}
