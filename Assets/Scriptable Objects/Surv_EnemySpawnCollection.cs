using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnCollection", menuName = "Survivor/Enemy Spawn Collection")]

public class Surv_EnemySpawnCollection : ScriptableObject
{
    public List<EnemySpawnSet> collection;
    private int currentSetIndex;

    public void Reset()
    {
        collection.Sort((x, y) => x.startTime.CompareTo(y.startTime));
        currentSetIndex = 0;
    }

    public float GetSpawnCooldown() => collection[currentSetIndex].spawnCooldown;

    public Surv_Enemy Get(float elapsedTime = -1)
    {
        if (elapsedTime < 0f) { return collection[0].Get(); }

        // change set based on game time
        if (currentSetIndex < (collection.Count - 1))
        {
            if (elapsedTime >= collection[currentSetIndex + 1].startTime)
            {
                currentSetIndex += 1;
            }
        }
        return collection[currentSetIndex].Get();
    }
}

[System.Serializable]
public class EnemySpawnSet
{
    [Tooltip("Time to start using this set")] public float startTime = 0f;
    public float spawnCooldown = 2f;

    public List<Surv_Enemy> set;

    public Surv_Enemy Get()
    {
        int i = Random.Range(0, set.Count);
        return set[i];
    }
}