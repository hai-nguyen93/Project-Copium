using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool requireKey;
    public bool isLocked;
    public float unlockAnimationDuration = 1f;
    public GameObject doorSprite;
    public Collider2D[] colliders;

    public RequireItem ri;

    private void Start()
    {
        if (requireKey)
        {
            ri = GetComponent<RequireItem>();
            if (ri == null)
            {
                Debug.LogWarning(gameObject.name + " needs RequireItem component.");
            }
        }
    }

    public void UnlockDoor()
    {
        isLocked = false;
        StartCoroutine(Unlock());
    }

    public IEnumerator Unlock()
    {
        float timer = unlockAnimationDuration;
        float startScale = doorSprite.transform.localScale.y;
        while (timer > 0)
        {
            float yScale = Mathf.Lerp(startScale, 0.5f, 1f - timer / unlockAnimationDuration);
            doorSprite.transform.localScale = new Vector3(doorSprite.transform.localScale.x, yScale, 1f);
            timer -= Time.deltaTime;
            yield return null;
        }

        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }

    public void CheckDoor()
    {
        if (isLocked && requireKey)
        {
            bool hasItems = ri.CheckRequiredItems();
            if (!hasItems)
            {
                Debug.Log("Needs " + ri.printItems() + " to unlock.");
            }
        }
    }
}
