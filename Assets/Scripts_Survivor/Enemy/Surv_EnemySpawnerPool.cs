using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_EnemySpawnerPool : MonoBehaviour
{
    //public Surv_EnemySpawnSettings spawnSettings;
    public List<Surv_Enemy> spawnCollection;



    public Surv_Enemy Get()
    {
        int index = Random.Range(0, spawnCollection.Count);
        Surv_Enemy e = Instantiate(spawnCollection[index], transform);
        return e;
    }
}
