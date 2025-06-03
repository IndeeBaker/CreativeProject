//AI was used for assistance in developing this code

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public Inventory inventory;         // Reference to your Inventory script
    public Image[] iconImages;          // UI Images for item icons in hotbar slots
    public Image[] slotBackgrounds;    // UI Images for slot backgrounds (for selection highlight)

    public int selectedIndex = 0;
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    public List<Item> hotbarItems = new List<Item>();  // Items currently assigned to hotbar slots

    void Start()
    {
        // Initialize hotbar with first items from inventory or empty slots
        for (int i = 0; i < iconImages.Length; i++)
        {
            if (inventory != null && i < inventory.items.Count)
                hotbarItems.Add(inventory.items[i]);
            else
                hotbarItems.Add(null);
        }

        UpdateHotbarUI();
        UpdateSelection();
    }

    void Update()
    {
        // Change selected hotbar slot with number keys 1-9
        for (int i = 0; i < slotBackgrounds.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedIndex = i;
                UpdateSelection();
            }
        }
    }

    public void UpdateHotbarUI()
    {
        for (int i = 0; i < iconImages.Length; i++)
        {
            if (hotbarItems[i] != null)
            {
                iconImages[i].sprite = hotbarItems[i].icon;
                iconImages[i].color = Color.white;
            }
            else
            {
                iconImages[i].sprite = null;
                iconImages[i].color = new Color(1, 1, 1, 0); // Make icon transparent if no item
            }
        }
    }

    void UpdateSelection()
    {
        for (int i = 0; i < slotBackgrounds.Length; i++)
        {
            slotBackgrounds[i].color = (i == selectedIndex) ? selectedColor : normalColor;
        }
    }

    // Swap item between inventory and hotbar slot by their indices
    public void SwapItem(int inventoryIndex, int hotbarIndex)
    {
        if (inventory == null) return;
        if (inventoryIndex < 0 || inventoryIndex >= inventory.items.Count) return;
        if (hotbarIndex < 0 || hotbarIndex >= hotbarItems.Count) return;

        // Swap the items
        Item temp = hotbarItems[hotbarIndex];
        hotbarItems[hotbarIndex] = inventory.items[inventoryIndex];
        inventory.items[inventoryIndex] = temp;

        UpdateHotbarUI();
    }
}