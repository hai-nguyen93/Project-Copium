using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MenuAbilitySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI abilityName;
    public int index;
    private MenuAbilityPanel parentPanel;
    public Button button;

    private void OnEnable()
    {
        button.onClick.AddListener(EquipAbility);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(EquipAbility);
    }

    public void EquipAbility()
    {
        parentPanel.EquipSelectedAbility();
    }

    private void Update()
    {
        if  (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            parentPanel.currentIndex = index;

            if (EventSystem.current.currentInputModule.input.GetButtonDown("Cancel"))
            {
                int index = parentPanel.activeAbilitiesPanel.currentSlot;
                parentPanel.activeAbilitiesPanel.activeAbilitySlots[index].button.Select();
            }
        }
    }

    public void SetSlot(Sprite abilityIcon, string name, int slotIndex, MenuAbilityPanel parent)
    {
        icon.sprite = abilityIcon;
        abilityName.text = name;
        index = slotIndex;
        parentPanel = parent;
    }

    public void SetNavigation()
    {
        var nav = button.navigation;
        nav.mode = Navigation.Mode.Explicit;
        nav.wrapAround = false;
        
        if (index > 0) // set upper button
        {
            nav.selectOnUp = parentPanel.abilityItems[index - 1].GetComponent<Button>();
        }
        if (index < parentPanel.abilityItems.Count - 1) //set lower button
        {
            nav.selectOnDown = parentPanel.abilityItems[index + 1].GetComponent<Button>();
        }

        button.navigation = nav;
    }
}
