using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuActiveAbilityPanel : MonoBehaviour
{
    [Tooltip("the button in main menu when exit ablility menu")]
    public Button defaultButtonExitTo;
    public MenuAbilityPanel abilityPanel;
    public List<MenuActiveAbilitySlot> activeAbilitySlots;
    public int currentSlot = 0;

    private PlayerCombat pCombat;

    [Header("Ability Description Panel")]
    public TextMeshProUGUI aDescription;

    private void OnEnable()
    {
        pCombat = PlayerData.Instance.pc.GetComponent<PlayerCombat>();

        // Set 4 UI active ability slots
        for (int i = 0; i < 4; ++i)
        {
            if (pCombat.equippedAbilities[i] == null || pCombat.equippedAbilities[i].abilityData == null)
            {
                activeAbilitySlots[i].icon.sprite = null;
                continue;
            }

            activeAbilitySlots[i].icon.sprite = pCombat.equippedAbilities[i].abilityData.abilityIcon;
        }
    }

    public void SetAbilityToCurrentSlot(Ability abilityToSet)
    {
        pCombat.equippedAbilities[currentSlot].abilityData = abilityToSet;
        pCombat.equippedAbilities[currentSlot].cdTimer = 2f;
        pCombat.equippedAbilities[currentSlot].SetAbilityState(AbilityState.cooldown);
        
        // update icons
        activeAbilitySlots[currentSlot].icon.sprite = pCombat.equippedAbilities[currentSlot].abilityData.abilityIcon;
        pCombat.RefreshAbilityIcons();

        activeAbilitySlots[currentSlot].isSetting = false;
        activeAbilitySlots[currentSlot].button.Select();
    }

    public void SetCurrentSlotIndex(int index)
    {
        if (index > 3)
        {
            Debug.Log("index out of bound (player only has 4 active abilities.");
            return;
        }

        currentSlot = index;
    }

    public void SelectAbilityFromLearnedList()
    {
        abilityPanel.abilityItems[0].GetComponent<Button>().Select();
    }

    public void UpdateDescription()
    {
        if (pCombat.equippedAbilities[currentSlot] == null || pCombat.equippedAbilities[currentSlot].abilityData == null) return;

            string desc = pCombat.equippedAbilities[currentSlot].abilityData.abilityName + "\nCooldown: "
            + pCombat.equippedAbilities[currentSlot].abilityData.cooldown + "s";
        aDescription.text = desc;
    }
}
