using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_EnemyTeemo : Surv_Enemy
{
    [Header("Teemo Settings")]
    public float minIdleTime;
    public float maxIdleTime;    
    private bool isIdling;
    private Vector3 destination;
    private Vector3 direction;

    private void Start()
    {
        PickNewDestination();
    }

    public override void Update()
    {
        if (Surv_GameController.Instance.state != GameState.Gameplay) return;
        if (player == null || player.isDead || isDead) return;

        if (isIdling || !canMove) return;

        if (ReachDestination()) { StartCoroutine(Idling()); }
        else {
            speed = data.baseSpeed * speedModifier;
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            // Look at moving direction
            if (facingRight && direction.x < 0f) Flip();
            if (!facingRight && direction.x > 0f) Flip();
        }
    }

    private IEnumerator Idling()
    {
        isIdling = true;
        yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
        PickNewDestination();
        isIdling = false;
    }

    public override void HitPlayer() { }

    public bool ReachDestination()
    {     
        return (Vector3.Distance(destination, transform.position) < 0.05f);
    }

    public void PickNewDestination()
    {
        if (player) {
            destination = new Vector3(player.transform.position.x + Random.Range(-4f, 4f), 0,
                player.transform.position.z + Random.Range(-4f, 4f));           
        } else { destination = Vector3.zero; }
        direction = new Vector3(destination.x - transform.position.x, 0, destination.z - transform.position.z).normalized;
    }
}
