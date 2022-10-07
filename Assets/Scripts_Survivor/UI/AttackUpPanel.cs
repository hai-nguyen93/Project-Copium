using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpPanel : MonoBehaviour
{
    public ParticleSystem atkUpPs;

    private void OnEnable()
    {
        if (!atkUpPs.gameObject.activeSelf)
        {
            atkUpPs.gameObject.SetActive(true);
            atkUpPs.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        atkUpPs.Play();
    }

    private void OnDisable()
    {
        if (atkUpPs != null) atkUpPs.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
