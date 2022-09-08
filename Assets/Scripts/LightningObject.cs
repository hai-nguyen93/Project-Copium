using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningObject : MonoBehaviour
{
    public LightningPS lightningPS;

    public GameObject hitbox;
    public BoxCollider2D box;
    public float duration = 1.75f;


    private void Start()
    {
        hitbox.SetActive(false);
        box = hitbox.GetComponent<BoxCollider2D>();

        Destroy(gameObject, duration);
        StartCoroutine(Strike());
    }

    public IEnumerator Strike()
    {
        yield return null;

        hitbox.SetActive(false);
        lightningPS.Play();
        hitbox.transform.localScale = new Vector3(hitbox.transform.localScale.x, lightningPS.lightning.main.startSizeY.constant, hitbox.transform.localScale.z);
        float time = lightningPS.lightning.main.startLifetime.constant;

        yield return new WaitForSeconds(time - 0.05f);
        hitbox.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        hitbox.SetActive(false);
    }
}
