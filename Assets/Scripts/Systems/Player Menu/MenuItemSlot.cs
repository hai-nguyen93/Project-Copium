using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MenuItemSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI itemName;
    public int index;
    private MenuItemPanel parentPanel;
    public Button button;

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            parentPanel.currentIndex = index;
        }
    }

    public void SetSlot(Sprite itemIcon, string name, int slotIndex, MenuItemPanel parent)
    {
        icon.sprite = itemIcon;
        itemName.text = name;
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
            nav.selectOnUp = parentPanel.items[index - 1].GetComponent<Button>();
        }
        if (index < parentPanel.items.Count - 1) //set lower button
        {
            nav.selectOnDown = parentPanel.items[index + 1].GetComponent<Button>();
        }

        button.navigation = nav;
    }
}
