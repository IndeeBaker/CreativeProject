using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<int> inventory = new List<int>();
    public List<int> hotbar = new List<int>();

    public List<ItemSlotUI> inventorySlots;
    public List<ItemSlotUI> hotbarSlots;

    public ItemDatabase itemDatabase;

    void Start()
    {
        inventory = new List<int> { -1, -1, -1, -1, -1, -1, -1, 1, -1, -1 };
        hotbar = new List<int> { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            int itemId = (i < inventory.Count) ? inventory[i] : -1;
            inventorySlots[i].SetItem(itemId);
        }

        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            int itemId = (i < hotbar.Count) ? hotbar[i] : -1;
            hotbarSlots[i].SetItem(itemId);
        }
    }
}
