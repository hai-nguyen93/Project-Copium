using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class EnemyBossPikachu : EnemyBase
{
    private PlayerController player;
    public PlayableDirector director;
    
    [Header("Ram Attack Settings")]
    public bool isAttacking;
    public float ramAggroRange = 10f;
    public float ramSpeed = 10f;
    public float ramSlowdownRate = 2f;
    public float ramTime = 3f;
    public float attackCooldown = 5f;
    public GameObject ramHitbox;

    [Header("Laser Attack Settings")]
    public TimelineAsset laserTimeline;
    public GameObject laserHitbox;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        isAttacking = false;

        ramHitbox.SetActive(false);
        laserHitbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking) return;
        else
        {
            LookAtPlayer(player.transform.position.x);
        }

        if (Vector2.Distance(player.transform.position, transform.position) < ramAggroRange && !isAttacking)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
                StartCoroutine(RamAttack());
            else
                StartCoroutine(LaserAttack());
        }
    }

    public IEnumerator RamAttack()
    {
        isAttacking = true;
        float ramTimer = ramTime;
        float targetX = player.transform.position.x;
        int direction = (transform.position.x < targetX) ? 1 : -1;

        // build up
        sr.color = Color.red;
        yield return new WaitForSeconds(0.5f);

        // attack    
        ramHitbox.SetActive(true);
        while ((direction == 1 && transform.position.x < targetX)
            || (direction == -1 && transform.position.x > targetX)){
            rb.velocity = new Vector2(direction * ramSpeed, rb.velocity.y);

            yield return null;
            
            ramTimer -= Time.deltaTime;
            if (ramTimer <= 0) break;
        }

        // slow down after ram
        while (Mathf.Abs(rb.velocity.x) > 0.01f)
        {
            float xSpeed = Mathf.MoveTowards(rb.velocity.x, 0, ramSlowdownRate * Time.deltaTime);
            rb.velocity = new Vector2(xSpeed, rb.velocity.y);
            yield return null;
        }
        rb.velocity = new Vector2(0, rb.velocity.y);

        ramHitbox.SetActive(false);
        sr.color = Color.white;

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    public IEnumerator LaserAttack()
    {
        isAttacking = true;
        LookAtPlayer(player.transform.position.x);
        director.playableAsset = laserTimeline;
        float time = (float) laserTimeline.duration;
        director.Play();

        yield return new WaitForSeconds(time + 2f);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, new Vector3(0, 0, 1), ramAggroRange);
    }
}
