using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public EnemyData data;
    protected Vector2 startPosition;
    private int currentHp;
    public bool facingRight = false;
    public bool isDead = false;

    protected Rigidbody2D rb;
    protected Collider2D bc;
    protected SpriteRenderer sr;

    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<Collider2D>();
        startPosition = transform.position;
    }
    
    void Start()
    {
        currentHp = data.maxHp;
    }

    void Update()
    {

    }

    public virtual void Flip()
    {
        facingRight = !facingRight;
        //transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
        transform.Rotate(new Vector3(0, 180, 0));
    }

    public void LookAtPlayer(float playerPositionX)
    {
        if (facingRight && transform.position.x > playerPositionX) Flip();
        if (!facingRight && transform.position.x < playerPositionX) Flip();

    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
    }
}
