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

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Surv_PlayerController>();
        }

        enemyList = new List<Surv_Enemy>();
        if (enableSpawner)
        {
            SpawnEnemy(0);
            spawnTimer = spawnCooldown;
        }
    }

    private void Update()
    {
        if (Surv_GameController.Instance.state != GameState.Gameplay) return;
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

    public void OnEnemyDie(Surv_Enemy enemy)
    {
        enemyList.Remove(enemy);
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        Vector3 pos = player.transform.position + direction * Random.Range(minSpawnRadius, maxSpawnRadius);

        return pos;
    }


#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(player.transform.position + new Vector3(0f, 0.5f, 0f), Vector3.up, minSpawnRadius);
        Handles.DrawWireDisc(player.transform.position + new Vector3(0f, 0.5f, 0f), Vector3.up, maxSpawnRadius);
    }
#endif
}
