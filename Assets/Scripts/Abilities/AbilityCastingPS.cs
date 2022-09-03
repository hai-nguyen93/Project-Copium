using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCastingPS : MonoBehaviour
{
    public ParticleSystem channelingLinesPS;
    public ParticleSystem channelingRingPS;
    public ParticleSystem readySparkPS;
    public ParticleSystem readyPS;

    public void Start()
    {
        StopAllParticles();
    }

    public void PlayChannelingPS(float castTime)
    {
        var ringMain = channelingRingPS.main;
        ringMain.startLifetime = castTime;

        readySparkPS.Stop();
        readyPS.Stop();
        channelingLinesPS.Play();
        channelingRingPS.Play();
    }

    public void PlayReadyPS()
    {
        channelingRingPS.Stop();
        channelingLinesPS.Stop();

        readySparkPS.Play();
        readyPS.Play();
    }

    public void StopAllParticles()
    {
        channelingRingPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        channelingLinesPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        readySparkPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        readyPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
