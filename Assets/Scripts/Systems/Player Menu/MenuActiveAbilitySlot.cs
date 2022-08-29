using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuActiveAbilitySlot : MonoBehaviour
{
    public Image icon;
    public Button button;
    [Tooltip("The slot index this button associates with")] public int index = 0;
    public bool isSetting = false;

    [Space]
    public MenuActiveAbilityPanel parentPanel;

    private void Start()
    {
        button.onClick.AddListener(() => parentPanel.SelectAbilityFromLearnedList());
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            parentPanel.SetCurrentSlotIndex(index);
            parentPanel.UpdateDescription();

            if (EventSystem.current.currentInputModule.input.GetButtonDown("Cancel"))
            {
                parentPanel.defaultButtonExitTo.Select();
            }

            if (InputSystem.GetDevice<Keyboard>().uKey.wasPressedThisFrame)
            {
                parentPanel.UnequipAbility(index);
            }
        }

    }
}
