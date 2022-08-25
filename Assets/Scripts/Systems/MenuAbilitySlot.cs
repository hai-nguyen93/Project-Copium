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

    private void Update()
    {
        if  (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            parentPanel.currentIndex = index;
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
