using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuItemPanel : MonoBehaviour
{
    private PlayerInventory pInventory;

    [Header("Inventory Panel")]
    public GameObject pfItemSlot;
    public GameObject scrollView;
    public Scrollbar scrollBar;
    public List<GameObject> items;
    public int currentIndex = 0;
    public int oldIndex = -1;
    public bool noItems = false;

    [Header("Item Description Panel")]
    public TextMeshProUGUI iDescription;

    private void OnEnable()
    {
        if (PlayerData.Instance == null)
        {
            Debug.Log("Cannot find player Data");
            return;
        }

        // Generate item list
        ClearContent();
        pInventory = PlayerData.Instance.GetComponent<PlayerInventory>();
        noItems = false;
        if (pInventory.inventory_keyItems.Count < 1)
        {
            noItems = true;
            UpdateDescription();
            scrollView.transform.parent.gameObject.SetActive(false);
            return;
        }

        for (int i = 0; i < pInventory.inventory_keyItems.Count; ++i)
        {
            KeyItem item = pInventory.inventory_keyItems[i];
            if (item == null) continue;
            GameObject go = Instantiate(pfItemSlot, scrollView.transform);
            items.Add(go);
            // set up item slot
            go.GetComponent<MenuItemSlot>().SetSlot(item.icon, item.iName, i, this);
        }

        // Set button navigation for items
        foreach (var item in items)
        {
            item.GetComponent<MenuItemSlot>().SetNavigation();
        }

        // Set default button Panel
        items[0].GetComponent<Button>().Select();

        // Set scroll view to top
        currentIndex = 0;
        oldIndex = currentIndex;
        scrollView.GetComponent<RectTransform>().anchoredPosition = new Vector2(scrollView.GetComponent<RectTransform>().anchoredPosition.x, 0);
        UpdateDescription();              
    }

    private void Update()
    {
        if (noItems) return;

        if (currentIndex != oldIndex)
        {
            RepositionScrollview();
            UpdateDescription();
            oldIndex = currentIndex;
        }
    }

    public void ClearContent()
    {
        foreach (Transform child in scrollView.transform)
        {
            Destroy(child.gameObject);
        }
        items.Clear();
    }

    public void RepositionScrollview()
    {
        scrollBar.value = 1 - ((float)currentIndex / (float)(items.Count - 1));
    }

    public void UpdateDescription()
    {
        if (noItems)
        {
            iDescription.text = "No Items";
            return;
        }

        string desc = pInventory.inventory_keyItems[currentIndex].iName + "\n\nDescription: "
            + pInventory.inventory_keyItems[currentIndex].description;
        iDescription.text = desc;
    }
}
