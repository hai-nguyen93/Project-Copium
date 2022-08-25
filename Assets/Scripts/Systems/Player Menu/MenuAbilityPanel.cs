using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MenuAbilityPanel : MonoBehaviour
{
    [Header("Learned Abilities Panel")]
    public GameObject pfMenuAbilitySlot;
    public GameObject scrollView;
    public Scrollbar scrollBar;
    public List<GameObject> abilityItems;
    public int currentIndex = 0;
    public int oldIndex = 0;

    [Header("Active Abilities Panel")]
    public MenuActiveAbilityPanel activeAbilitiesPanel;

    [Header("Ability Description Panel")]
    public TextMeshProUGUI aDescription;

    private void OnEnable()
    {
        if (PlayerData.Instance == null)
        {
            Debug.Log("Cannot find player Data");
            return;
        }

        // Generate ability list
        ClearContent();
        for (int i = 0; i < PlayerData.Instance.abilities.Count; ++i)
        {
            Ability a = PlayerData.Instance.abilities[i];
            if (a == null) continue;
            GameObject go = Instantiate(pfMenuAbilitySlot, scrollView.transform);
            abilityItems.Add(go);
            go.GetComponent<MenuAbilitySlot>().SetSlot(a.abilityIcon, a.abilityName, i, this);          
        }

        // Set button navigation for ability items
        foreach(var item in abilityItems)
        {
            item.GetComponent<MenuAbilitySlot>().SetNavigation();
        }

        // Set Active Abilities Panel
        activeAbilitiesPanel.activeAbilitySlots[0].button.Select();

        // Set scroll view to top
        // abilityItems[0].GetComponent<Button>().Select();
        currentIndex = 0;
        oldIndex = currentIndex;
        //StartCoroutine(SetScrollbarTop());
        //scrollBar.value = 1f;
        scrollView.GetComponent<RectTransform>().anchoredPosition = new Vector2(scrollView.GetComponent<RectTransform>().anchoredPosition.x, 0);
        //UpdateDescription();              
    }

    private void Update()
    {       
        if (currentIndex != oldIndex)
        {
            RepositionScrollview();
            UpdateDescription();
            oldIndex = currentIndex;
        }
    }

    public void ClearContent()
    {
        foreach(Transform child in scrollView.transform)
        {
            Destroy(child.gameObject);
        }
        abilityItems.Clear();
    }

    public void RepositionScrollview()
    {
        scrollBar.value = 1 - ((float)currentIndex / (float)(abilityItems.Count - 1));
    }

    public void UpdateDescription()
    {
        string desc = PlayerData.Instance.abilities[currentIndex].abilityName + "\nCooldown: " 
            + PlayerData.Instance.abilities[currentIndex].cooldown + "s";
        aDescription.text = desc;
    }

    public void EquipSelectedAbility()
    {
        activeAbilitiesPanel.SetAbilityToCurrentSlot(PlayerData.Instance.abilities[currentIndex]);
    }

    IEnumerator SetScrollbarTop()
    {
        yield return new WaitForEndOfFrame();
        scrollBar.value = 1f;
    }
}
