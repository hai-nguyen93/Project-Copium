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
        canMove = !(player == null || player.isDead || isDead);

        if (isIdling) return;

        if (ReachDestination()) { StartCoroutine(Idling()); }
        else {
            speed = data.baseSpeed * speedModifier;
            transform.Translate(speed * Time.deltaTime * direction, Space.World);

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
        //Vector3 delta = new Vector3(destination.x - transform.position.x, 0, destination.z - transform.position.z);
        return (Mathf.Abs(destination.x - transform.position.x) < 0.1f &&
            Mathf.Abs(destination.z - transform.position.z) < 0.1f);
    }

    public void PickNewDestination()
    {
        destination = player ? player.transform.position : Vector3.zero;
        direction = new Vector3(destination.x - transform.position.x, 0, destination.z - transform.position.z).normalized;
    }
}
