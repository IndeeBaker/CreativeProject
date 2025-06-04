//this code was built using AI for assistance

using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public int inventoryIndex; // Set this in the UI
    public Image iconImage;    // Drag the icon child image here

    private Inventory inventory;
    private Hotbar hotbar;

    [System.Obsolete]
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        hotbar = FindObjectOfType<Hotbar>();

        UpdateSlot();
    }

    public void OnClick()
    {
        if (hotbar != null)
        {
            hotbar.SwapItem(inventoryIndex, hotbar.selectedIndex); // Sends this item to the selected hotbar slot
        }
    }

    public void UpdateSlot()
    {
        if (inventory != null && inventoryIndex < inventory.items.Count)
        {
            Item item = inventory.items[inventoryIndex];
            if (item != null)
            {
                iconImage.sprite = item.icon;
                iconImage.color = Color.white;
            }
        }
        else
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1, 1, 1, 0);
        }
    }
}
