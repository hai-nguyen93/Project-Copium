using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledParticleSystem : MonoBehaviour
{
    ParticleSystem ps;
    EnemyDiePSPool pool;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    public void SetPool(EnemyDiePSPool pool)
    {
        this.pool = pool;
    }

    private void OnParticleSystemStopped()
    {
        if (pool != null)
        {
            pool.Release(ps);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
