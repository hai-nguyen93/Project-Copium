using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Collections;
using Unity.Jobs;

public class Surv_EnemyMultiThread : MonoBehaviour
{
    public Surv_PlayerController player;

    [Header("For testing only, default = 0")]
    public int spawnNumberOnStart = 0;
    public GameObject pfEnemy;
    [HideInInspector] public List<Surv_Enemy> enemyList;
    private Surv_EnemySpawner spawner;
    
    private void Start()
    {
        enemyList = new List<Surv_Enemy>();
        spawner = GetComponent<Surv_EnemySpawner>();

        if (Surv_GameController.Instance.useMultiThread)
        {
            for (int i = 0; i < spawnNumberOnStart; ++i)
            {
                GameObject go = Instantiate(pfEnemy, spawner.GetRandomPosition(), Quaternion.identity);
                var enemy = go.GetComponent<Surv_Enemy>();
                enemy.SetPlayer(player);
                enemyList.Add(enemy);
            }
        }
    }

    void Update()
    {
        
    }
}
