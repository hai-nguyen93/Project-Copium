using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surv_GroundController : MonoBehaviour
{
    public Surv_PlayerController player;
    private Transform pTransform;
    private Transform t; // to cache transform of grounds
    public float referenceSize = 100;
    private float repositionThreshold;
    [Tooltip("yOffset to make sure ground surface is at y = 0")] public float yOffset = 0.5f;

    public GameObject pfGround;

    public List<GameObject> grounds;

    private void Start()
    {
        grounds = new List<GameObject>();
        repositionThreshold = 1.5f * referenceSize;
        pTransform = player.transform;

        CreateStartGrounds();
    }

    private void Update()
    {
        if (player.isDead || Surv_GameController.Instance.state != GameState.Gameplay) return;

        foreach (var g in grounds)
        {
            t = g.transform;
            float dx = pTransform.position.x - t.position.x;
            float dz = pTransform.position.z - t.position.z;

            if ((Mathf.Abs(dx) <= repositionThreshold) && (Mathf.Abs(dz) <= repositionThreshold)) continue;

            Vector3 translation = Vector3.zero;
            if (Mathf.Abs(dx) > repositionThreshold)
            {
                translation.x = Mathf.Sign(dx) * 3 * referenceSize;
            }
            if (Mathf.Abs(dz) > repositionThreshold)
            {
                translation.z = Mathf.Sign(dz) * 3 * referenceSize;
            }
            RepositionGround(g, t.position.x + translation.x, t.position.z + translation.z);
        }
    }

    public void CreateStartGrounds()
    {
        for (int i = -1; i <= 1; ++i)
        {
            for (int j = -1; j <= 1; ++j)
            {
                CreateGround(i * referenceSize, j * referenceSize);
            }
        }       
    }

    public void CreateGround(float x, float z)
    {
        GameObject go = Instantiate(pfGround, new Vector3(x, -yOffset, z), Quaternion.identity, this.transform);
        grounds.Add(go);
    }

    public void RepositionGround(GameObject ground, float newX, float newZ)
    {
        ground.transform.position = new Vector3(newX, -yOffset, newZ);
    }
}
