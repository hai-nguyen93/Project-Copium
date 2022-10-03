using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_EnemySpawnSettings : ScriptableObject
{
    public List<List<Surv_Enemy>> enemyCollections;
}

[System.Serializable]
public class EnemySpawnCollection
{
    public float whenToSpawn = 0f;

}