using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_EnemySpawner : MonoBehaviour
{
    public Surv_PlayerController player;
    public GameObject[] enemyPrefabs;

    [Space]
    [Tooltip("how many enemies spawned per second")] public float spawnRate = 1f;
    private float spawnTimer;

    [Space]
    public BoxCollider stageBounds;
    private float minX, maxX;
    private float minZ, maxZ;

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Surv_PlayerController>();
        }

        minX = stageBounds.bounds.center.x - stageBounds.bounds.extents.x;
        maxX = stageBounds.bounds.center.x + stageBounds.bounds.extents.x;
        minZ = stageBounds.bounds.center.z - stageBounds.bounds.extents.z;
        maxZ = stageBounds.bounds.center.z + stageBounds.bounds.extents.z;

        SpawnEnemy(0);
        spawnTimer = 1 / spawnRate;
    }
    
    private void Update()
    {
        if (player.isDead) return;

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = 1 / spawnRate;
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

        Vector3 pos = new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ));
        GameObject go = Instantiate(enemyPrefabs[index], pos, Quaternion.identity);
        go.GetComponent<Surv_Enemy>().SetPlayer(player);
    }
}
