using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSpriteSortingLayer : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        sr.sortingOrder = -(int)(transform.position.z * 10);
    }
}
