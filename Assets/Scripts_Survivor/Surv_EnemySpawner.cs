using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Surv_EnemySpawner : MonoBehaviour
{
    public Surv_PlayerController player;
    public GameObject[] enemyPrefabs;

    [Space]
    public bool enableSpawner;
    public float spawnCooldown = 2f;
    private float spawnTimer;
    public int maxEnemyCount = 50;
    public List<Surv_Enemy> enemyList;

    [Space]
    public float minSpawnRadius = 7f;
    public float maxSpawnRadius = 20f;
    //public BoxCollider outerBounds;
    //public BoxCollider innerBounds;
    //private float minX, maxX;
    //private float minZ, maxZ;

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Surv_PlayerController>();
        }

        //minX = outerBounds.bounds.center.x - outerBounds.bounds.extents.x;
        //maxX = outerBounds.bounds.center.x + outerBounds.bounds.extents.x;
        //minZ = outerBounds.bounds.center.z - outerBounds.bounds.extents.z;
        //maxZ = outerBounds.bounds.center.z + outerBounds.bounds.extents.z;

        enemyList = new List<Surv_Enemy>();
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
        if (enemyList.Count >= maxEnemyCount) return;

        if (index < 0 || index >= enemyPrefabs.Length) index = 0;

        Vector3 pos = GetRandomPosition();
        GameObject go = Instantiate(enemyPrefabs[index], pos, Quaternion.identity);
        var e = go.GetComponent<Surv_Enemy>();
        e.SetPlayer(player);
        e.SetSpanwer(this);
        enemyList.Add(e);
    }

    public Vector3 GetRandomPosition()
    {
        /*
        Vector3 pos = new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ));
        if (innerBounds.bounds.Contains(pos))
        {
            // Debug.Log("Spawn pos inside safe area -> Reposition");

            Ray ray = new Ray(player.transform.position, pos - player.transform.position);
            float distance;
            innerBounds.bounds.IntersectRay(ray, out distance);
            Vector3 newPos = player.transform.position + (ray.direction * distance);

            pos.x = newPos.x;
            pos.z = newPos.z;
        }
        */

        Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        Vector3 pos = player.transform.position + direction * Random.Range(minSpawnRadius, maxSpawnRadius);

        return pos;
    }

    public void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(player.transform.position + new Vector3(0f, 0.5f, 0f), Vector3.up, minSpawnRadius);
        Handles.DrawWireDisc(player.transform.position + new Vector3(0f, 0.5f, 0f), Vector3.up, maxSpawnRadius);
    }
}
