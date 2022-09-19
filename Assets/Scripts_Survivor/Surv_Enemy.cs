using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_Enemy : MonoBehaviour
{
    private Surv_PlayerController player;
    public bool isDead = false;
    public bool facingRight;
    public int damage = 1;
    public float baseSpeed = 2.5f;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || isDead) return;

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        // look at player
        if (facingRight && directionToPlayer.x < 0f) Flip();
        if (!facingRight && directionToPlayer.x > 0f) Flip();

        speed = baseSpeed;
        transform.Translate(directionToPlayer * speed * Time.deltaTime, Space.World);
    }

    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0f, 180f, 0f));
    }

    public void HitPlayer()
    {
        isDead = true;
        Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void SetPlayer(Surv_PlayerController player)
    {
        this.player = player;
    }
}
