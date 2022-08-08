using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="MyAssets/Enemy")]
public class EnemyData: ScriptableObject
{
    public string enemyName = "New Enemy";
    public int maxHp = 5;
    public int atk = 1;
}

public class EnemyBase : MonoBehaviour
{
    public EnemyData data;  
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

    }

    void Update()
    {

    }

    public virtual void Flip() { }

    public virtual void Die()
    {
        gameObject.SetActive(false);
    }
}
