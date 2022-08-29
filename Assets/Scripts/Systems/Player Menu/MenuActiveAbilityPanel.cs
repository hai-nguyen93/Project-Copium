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
        var currentAbility = pCombat.equippedAbilities[currentSlot]; // current equipped ability in current slot

        // if ability already in current slot
        if (currentAbility.abilityData == abilityToSet)
        {
            Debug.Log("Ability already in current slot");
            return;
        }

        // if ability already in other slot
        //bool equipped = false;
        for (int i = 0; i < pCombat.equippedAbilities.Count; i++)
        {
            var slotToSwap = pCombat.equippedAbilities[i];
            if (slotToSwap == null || slotToSwap.abilityData == null) continue;
            if (slotToSwap.abilityData == abilityToSet)
            {
                // swap ability with that other slot
                Debug.Log("Swap slot " + i + " with slot " + currentSlot);
                SetAbility(currentAbility.abilityData, i);
                break;
            }
        }

        /*slot.abilityData = abilityToSet;
        slot.cdTimer = 2f;
        slot.SetAbilityState(AbilityState.cooldown);

        // update icons
        activeAbilitySlots[currentSlot].icon.sprite = slot.abilityData.abilityIcon;
        pCombat.RefreshAbilityIcons();*/

        SetAbility(abilityToSet, currentSlot);
        activeAbilitySlots[currentSlot].isSetting = false;
        activeAbilitySlots[currentSlot].button.Select();
    }

    public void SetAbility(Ability abilityToSet, int slotToSet)
    {
        if (slotToSet >= pCombat.equippedAbilities.Count)
        {
            Debug.Log("index > max number of abilities");
            return;
        }

        var slot = pCombat.equippedAbilities[slotToSet];
        if (slot == null)
        {
            Debug.Log("Null reference");
            return;
        }

        slot.abilityData = abilityToSet;
        if (abilityToSet == null)
        {
            slot.cdTimer = 0f;
            slot.SetAbilityState(AbilityState.ready);
            activeAbilitySlots[slotToSet].icon.sprite = null; // update icon
        }
        else
        {
            slot.cdTimer = 2f;
            slot.SetAbilityState(AbilityState.cooldown);            
            activeAbilitySlots[slotToSet].icon.sprite = slot.abilityData.abilityIcon; // update icon
        }
        pCombat.RefreshAbilityIcons();
    }

    public void UnequipAbility(int slotToUnequip)
    {
        if (slotToUnequip >= pCombat.equippedAbilities.Count)
        {
            Debug.Log("index > max number of abilities");
            return;
        }

        var slot = pCombat.equippedAbilities[slotToUnequip];
        if (slot == null)
        {
            Debug.Log("Null reference");
            return;
        }

        slot.abilityData = null;
        slot.cdTimer = 0f;
        slot.SetAbilityState(AbilityState.ready);
        activeAbilitySlots[slotToUnequip].icon.sprite = null; // update icon
        pCombat.RefreshAbilityIcons();
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
