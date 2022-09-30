using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyDiePSPool : MonoBehaviour
{
    public ParticleSystem pfParticleSystem;
    public int initialCapacity;
    public int maxCapacity;

    public ObjectPool<ParticleSystem> pool;

    private void Start()
    {
        pool = new ObjectPool<ParticleSystem>(CreatePoolObj, OnGetPoolObj, OnReleasePoolObj, OnDestroyPoolObj, true, initialCapacity, maxCapacity);

        // Prepare objs in pool
        for (int i =0; i < initialCapacity; ++i)
        {
            var p = CreatePoolObj();
            Release(p);
        }
    }

    public ParticleSystem CreatePoolObj()
    {
        ParticleSystem p = Instantiate(pfParticleSystem, this.transform);
        p.GetComponent<PooledParticleSystem>().SetPool(this);
        p.Stop();
        return p;
    }
    public void OnGetPoolObj(ParticleSystem ps)
    {
        ps.gameObject.SetActive(true);
    }
    public void OnReleasePoolObj(ParticleSystem ps)
    {
        ps.gameObject.SetActive(false);
    }
    public void OnDestroyPoolObj(ParticleSystem ps)
    {
        Destroy(ps.gameObject);
    }

    public ParticleSystem Get()
    {
        return pool.Get();
    }

    public void Release(ParticleSystem ps)
    {
        pool.Release(ps);
    }
}
