using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public EnemyData data;
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
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
    }
}
