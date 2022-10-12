using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Surv_EnemySpawner : MonoBehaviour
{
    public Surv_PlayerController player;
    public Surv_EnemySpawnCollection spawnPfCollection;

    [Space]
    public bool enableSpawner;
    public float spawnCooldown { get => spawnPfCollection.GetSpawnCooldown(); }
    private float spawnTimer;
    public int maxEnemyCount = 80;
    public List<Surv_Enemy> enemyList;

    [Space]
    public float minSpawnRadius = 7f;
    public float maxSpawnRadius = 20f;

    [Header("Dropped Item Prefabs")]
    public GameObject pfHealItem;
    public GameObject pfAtkItem;

    private void Start()
    {
        spawnPfCollection.Reset();

        if (player == null)
        {
            player = FindObjectOfType<Surv_PlayerController>();
        }

        enemyList = new List<Surv_Enemy>();
        if (enableSpawner)
        {
            SpawnEnemy();
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
        if (enemyList.Count >= maxEnemyCount) return;

        var e = Instantiate(spawnPfCollection.Get(), transform);
        e.gameObject.SetActive(true);
        Vector3 pos = GetRandomPosition();
        e.transform.position = pos;
        e.SetPlayer(player);
        e.SetSpanwer(this);
        enemyList.Add(e);
    }

    public void OnEnemyDie(Surv_Enemy enemy, bool spawnItem = false)
    {
        if (spawnItem)
        {
            if (Random.value <= enemy.data.chanceToSpawnHealItem)
            {             
                SpawnItem(pfHealItem, enemy.transform.position);
            }

            if (Random.value <= enemy.data.chanceToSpawnAtkItem)
            {              
                SpawnItem(pfAtkItem, enemy.transform.position);
            }
        }

        enemyList.Remove(enemy);
    }

    public void SpawnItem(GameObject pfItem, Vector3 basePos)
    {
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        Vector3 pos = basePos + Random.Range(0.5f, 1.5f) * direction;
        var go = Instantiate(pfItem, pos, Quaternion.identity, null);
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        Vector3 pos = player.transform.position + Random.Range(minSpawnRadius, maxSpawnRadius) * direction;

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
