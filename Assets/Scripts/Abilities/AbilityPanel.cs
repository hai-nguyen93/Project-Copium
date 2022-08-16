using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPanel : MonoBehaviour
{
    public List<AbilitySlot_UI> UI_abilitySlots;
    public Vector2 hideScale = new Vector2(0.8f, 0.8f);
    public Vector2 showScale = new Vector2(1f, 1f);
    public float animationTime = 0.2f;

    IEnumerator panelLerp;

    private PlayerCombat pCombat;

    private void Start()
    {
        pCombat = FindObjectOfType<PlayerCombat>();
        UpdateAbilitySlots();
    }

    public void UpdateAbilitySlots()
    {
        for (int i = 0; i < 4; ++i)
        {
            if (i < pCombat.equippedAbilities.Count)
            {
                if (pCombat.equippedAbilities[i] != null && pCombat.equippedAbilities[i].abilityData != null)
                {
                    UI_abilitySlots[i].SetAbility(pCombat.equippedAbilities[i]);
                    //Debug.Log(pCombat.equippedAbilities[i].abilityData.abilityName + " set.");
                }
                else
                {
                    UI_abilitySlots[i].RemoveAbility();
                }
            }
        }
    }

    public void Show()
    {
        if (panelLerp != null) StopCoroutine(panelLerp);

        panelLerp = LerpScale(showScale, animationTime);
        StartCoroutine(panelLerp);
    }

    public void Hide()
    {
        if (panelLerp != null) StopCoroutine(panelLerp);

        panelLerp = LerpScale(hideScale, animationTime);
        StartCoroutine(panelLerp);
    }

    IEnumerator LerpScale(Vector2 newScale, float time)
    {
        float t = 0;
        float startScaleX = transform.localScale.x;
        float startScaleY = transform.localScale.y;
        float x, y;

        while (t < time)
        {
            x = Mathf.Lerp(startScaleX, newScale.x, t / time);
            y = Mathf.Lerp(startScaleY, newScale.y, t / time);

            transform.localScale = new Vector3(x, y, 1f);

            t += Time.deltaTime;
            yield return null;
        }
    }
}
