using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_FollowTarget : MonoBehaviour
{
    public Transform target;
    private Vector3 lastPos;

    //private void OnEnable()
    //{
    //    if (target != null)
    //    {
    //        lastPos = target.position;
    //    }
    //}

    private void Update()
    {
        if (target == null) return;

        Vector3 deltaMove = target.position - lastPos;
        transform.position += deltaMove;
        lastPos = target.position;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        lastPos = target.position;
    }
}
