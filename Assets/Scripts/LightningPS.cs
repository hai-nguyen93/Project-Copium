using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningPS : MonoBehaviour
{
    public float length = 10f;
    public LayerMask blockLayer;
    bool isPlaying;

    [Header("Particle Systems")]
    public ParticleSystem startSpark;
    public ParticleSystem lightning;
    public ParticleSystem endSpark;

    private ParticleSystem[] particles;

    private void Start()
    {
        particles = GetComponentsInChildren<ParticleSystem>();

        StopAllParticles();
        EnableEmissionAll();
    }

    private void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.T))
        {
            Play();
        }*/
    }

    public void Setup()
    {
        var lightningMain = lightning.main;
        var hit = Physics2D.Raycast(transform.position, Vector2.down, length, blockLayer);
        if (hit)
        {
            endSpark.transform.position = hit.point;
            lightningMain.startSizeY = transform.position.y - hit.point.y;
        }
        else
        {
            endSpark.transform.position = new Vector3(transform.position.x, transform.position.y - length, transform.position.z);
            lightningMain.startSizeY = length;
        }
    }

    public void StopAllParticles()
    {
        /*startSpark.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        lightning.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        endSpark.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);*/

        isPlaying = false;
        foreach(var p in particles)
        {
            p.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public void EnableEmissionAll()
    {
        foreach (var p in particles)
        {
            var em = p.emission;
            em.enabled = true;
        }
    } 

    public void Play()
    {
        StopAllParticles();
        Setup();
        startSpark.Play();
        lightning.Play();
        endSpark.Play();
    }

    public bool IsPlaying()
    {
        isPlaying = false;
        foreach (var p in particles)
        {
            if (p.isPlaying)
            {
                isPlaying = true;
                break;
            }
        }
        return isPlaying;
    }
}
