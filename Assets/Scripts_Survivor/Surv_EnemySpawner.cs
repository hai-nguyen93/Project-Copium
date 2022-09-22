using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_EnemySpawner : MonoBehaviour
{
    public Surv_PlayerController player;
    public GameObject[] enemyPrefabs;

    [Space]
    public bool enableSpawner;
    public float spawnCooldown = 2f;
    private float spawnTimer;

    [Space]
    public BoxCollider outerBounds;
    public BoxCollider innerBounds;
    private float minX, maxX;
    private float minZ, maxZ;

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Surv_PlayerController>();
        }

        minX = outerBounds.bounds.center.x - outerBounds.bounds.extents.x;
        maxX = outerBounds.bounds.center.x + outerBounds.bounds.extents.x;
        minZ = outerBounds.bounds.center.z - outerBounds.bounds.extents.z;
        maxZ = outerBounds.bounds.center.z + outerBounds.bounds.extents.z;

        if (enableSpawner)
        {
            SpawnEnemy(0);
            spawnTimer = spawnCooldown;
        }
    }
    
    private void Update()
    {
        if (!enableSpawner || player.isDead) return;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnCooldown;
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }
    
    public void SpawnEnemy()
    {
        int i = Random.Range(0, enemyPrefabs.Length);
        SpawnEnemy(i);
    }

    public void SpawnEnemy(int index)
    {
        if (index < 0 || index >= enemyPrefabs.Length) index = 0;

        Vector3 pos = GetRandomPosition();
        GameObject go = Instantiate(enemyPrefabs[index], pos, Quaternion.identity);
        go.GetComponent<Surv_Enemy>().SetPlayer(player);
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 pos = new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ));
        if (innerBounds.bounds.Contains(pos))
        {
            // Debug.Log("Spawn pos inside safe area -> Reposition");

            Ray ray = new Ray(player.transform.position, pos - player.transform.position);
            float distance;
            innerBounds.bounds.IntersectRay(ray, out distance);
            Vector3 newPos = player.transform.position + ray.direction * distance;

            pos.x = newPos.x;
            pos.z = newPos.z;
        }

        return pos;
    }
}
