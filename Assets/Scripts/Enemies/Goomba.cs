using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : EnemyBase
{
    public float speed = 3f;
    private float xVel;
    public Transform forwardDetection;
    public float forwardDetectRange = 0.05f;
    public LayerMask layerToDetect;

    // Start is called before the first frame update
    void Start()
    {
        xVel = facingRight ? speed : -speed;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(xVel, rb.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        // Change direction if hit something in front of this enemy
        Debug.DrawLine(forwardDetection.position, forwardDetection.position + new Vector3((facingRight ? 1 : -1) * forwardDetectRange, 0, 0));
        var hit = Physics2D.Raycast(forwardDetection.position, new Vector2(facingRight ? 1 : -1, 0), forwardDetectRange, layerToDetect);
        if (hit)
        {
            Flip();
        }
    }

    public override void Flip()
    {
        xVel = -xVel;
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();
        if (player)
        {
            //if (PlayerData.Instance.isDead) return;

            Debug.Log("hit player");
            float xDifferenceToPlayer = player.transform.position.x - transform.position.x;
            if (xDifferenceToPlayer * xVel > 0)
            {
                player.Damage(1, (int) Mathf.Sign(xVel));
                Flip();
            }
            else
            {
                player.Damage(1, (int)-Mathf.Sign(xVel));
            }
        }
    }
}

