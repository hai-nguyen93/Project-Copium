using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot_UI : MonoBehaviour
{
    public Sprite defaultIcon;
    public Image abilityIcon;
    public Image cdFillImage;
 
    private ActiveAbility ability;

    void Awake()
    {
        RemoveAbilityIcon();
    }

    void Update()
    {
        if (ability == null || ability.abilityData == null) return;

        // update cd UI
        if (ability.state == AbilityState.ready)
        {
            cdFillImage.fillAmount = 0f;
        }
        else if (ability.state == AbilityState.cooldown)
        {
            cdFillImage.fillAmount = ability.cdTimer / ability.abilityData.cooldown;
        }
    }

    public void SetAbility(ActiveAbility _ability)
    {
        if (_ability == null || _ability.abilityData == null)
        {
            RemoveAbilityIcon();
            return;
        }

        ability = _ability;
        SetAbilityIcon(ability.abilityData.abilityIcon);
    }

    public void SetAbilityIcon(Sprite icon)
    {
        if (icon != null)
        {
            abilityIcon.sprite = icon;
            //Debug.Log(icon.name + " icon set.");
        }
    }

    public void RemoveAbilityIcon()
    {
        //Debug.Log(gameObject.name + "remove icon");
        abilityIcon.sprite = defaultIcon;
    }

    public void RemoveAbility()
    {
        ability = null;
        RemoveAbilityIcon();
    }
}
