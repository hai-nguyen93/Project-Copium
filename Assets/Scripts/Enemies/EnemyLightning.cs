using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLightning : MonoBehaviour
{
    public LightningPS lightningPS;

    public GameObject hitbox;
    private BoxCollider2D box;

    private void Start()
    {
        hitbox.SetActive(false);
        box = hitbox.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!lightningPS.IsPlaying())
            {
                StopCoroutine(Strike());
                StartCoroutine(Strike());
            }
        }
    }

    public IEnumerator Strike()
    {
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

