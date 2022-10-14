using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MotivatedCut : MonoBehaviour, IBossAttack
{
    public VisualEffect attackVFX;

    void Start()
    {
        attackVFX.Stop();
    }

    public void Activate()
    {
        throw new System.NotImplementedException();
    }


}
