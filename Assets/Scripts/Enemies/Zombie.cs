using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Zombie : EnemyBase
{
    public float speed = 3f;
    public float aggroRange = 5f;

    private PlayerController player;
    private bool isChasing = false;
    private float xVel;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        isChasing = false;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        isChasing = (distanceToPlayer < aggroRange); 
    }

    private void FixedUpdate()
    {
        // chasing player
        if (isChasing)
        {
            float moveDirectionX = Mathf.Sign(player.transform.position.x - transform.position.x);
            xVel = moveDirectionX*speed;
            rb.velocity = new Vector2(xVel, rb.velocity.y);
        }
        else
        {
            // return to original position.x
            if (Mathf.Abs(transform.position.x - startPosition.x) > 0.1f)
            {
                float moveDirectionX = Mathf.Sign(startPosition.x - transform.position.x);
                xVel = moveDirectionX * speed;
                rb.velocity = new Vector2(xVel, rb.velocity.y);
            }
            else // stay at original position.x
            {
                xVel = 0f;
                rb.velocity = new Vector2(xVel, rb.velocity.y);
            }
        }

        if (facingRight && xVel < 0f) Flip();
        if (!facingRight && xVel > 0f) Flip();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();
        if (player)
        {
            Debug.Log("hit player");
            float xDifferenceToPlayer = player.transform.position.x - transform.position.x;
            if (xDifferenceToPlayer * xVel > 0)
            {
                player.Stagger((int)xVel);
            }
            else
            {
                player.Stagger((int)-xVel);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, new Vector3(0, 0, 1), aggroRange);
    }
}
