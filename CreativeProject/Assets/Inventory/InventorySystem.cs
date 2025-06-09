using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public List<int> inventory = new List<int>();
    public List<int> inventoryQuantities = new List<int>();

    public List<int> hotbar = new List<int>();
    public List<int> hotbarQuantities = new List<int>();

    public List<ItemSlotUI> inventorySlots;
    public List<ItemSlotUI> hotbarSlots;

    public GameObject[] selected;
    public ItemDatabase itemDatabase;
    public int selectedHotbarIndex = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        int inventorySize = 10;
        int hotbarSize = 10;

        inventory.Clear();
        inventoryQuantities.Clear();
        hotbar.Clear();
        hotbarQuantities.Clear();

        for (int i = 0; i < inventorySize; i++)
        {
            inventory.Add(-1);
            inventoryQuantities.Add(0);
        }

        hotbar = new List<int> { 0, 1, 2, 3, -1, -1, -1, -1, -1, -1 };
        hotbarQuantities = new List<int> { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 };

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].slotIndex = i;
            inventorySlots[i].isHotbarSlot = false;
            inventorySlots[i].itemDatabase = itemDatabase;
        }

        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            hotbarSlots[i].slotIndex = i;
            hotbarSlots[i].isHotbarSlot = true;
            hotbarSlots[i].itemDatabase = itemDatabase;
        }

        UpdateUI();
        SelectHotbar(0);
    }

    void Update()
    {
        HandleHotbarInput();
    }

    void HandleHotbarInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SelectHotbar(0); }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { SelectHotbar(1); }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { SelectHotbar(2); }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) { SelectHotbar(3); }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) { SelectHotbar(4); }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) { SelectHotbar(5); }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) { SelectHotbar(6); }
        else if (Input.GetKeyDown(KeyCode.Alpha8)) { SelectHotbar(7); }
        else if (Input.GetKeyDown(KeyCode.Alpha9)) { SelectHotbar(8); }
        else if (Input.GetKeyDown(KeyCode.Alpha0)) { SelectHotbar(9); }
    }

    public int GetHeldItemId()
    {
        if (selectedHotbarIndex >= 0 && selectedHotbarIndex < hotbar.Count)
            return hotbar[selectedHotbarIndex];
        return -1;
    }

    public ItemDatabase.ItemData GetHeldItemData()
    {
        int itemId = GetHeldItemId();
        return itemId >= 0 ? itemDatabase.GetItemById(itemId) : null;
    }

    public bool AddItem(int itemId, int quantityToAdd)
    {
        if (itemId < 0) return false;

        var itemData = itemDatabase.GetItemById(itemId);
        if (itemData == null) return false;

        int maxStack = itemData.maxStack;

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] == itemId && inventoryQuantities[i] < maxStack)
            {
                int spaceLeft = maxStack - inventoryQuantities[i];
                int addAmount = Mathf.Min(spaceLeft, quantityToAdd);
                inventoryQuantities[i] += addAmount;
                quantityToAdd -= addAmount;
                inventorySlots[i].SetItem(inventory[i], inventoryQuantities[i]);
                if (quantityToAdd <= 0) { UpdateUI(); return true; }
            }
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] == -1)
            {
                int addAmount = Mathf.Min(quantityToAdd, maxStack);
                inventory[i] = itemId;
                inventoryQuantities[i] = addAmount;
                quantityToAdd -= addAmount;
                inventorySlots[i].SetItem(inventory[i], inventoryQuantities[i]);
                if (quantityToAdd <= 0) { UpdateUI(); return true; }
            }
        }

        UpdateUI();
        return false;
    }

    public bool AddItemToHotbar(int itemId, int quantityToAdd)
    {
        if (itemId < 0) return false;

        var itemData = itemDatabase.GetItemById(itemId);
        if (itemData == null) return false;

        int maxStack = itemData.maxStack;

        for (int i = 0; i < hotbar.Count; i++)
        {
            if (hotbar[i] == itemId && hotbarQuantities[i] < maxStack)
            {
                int spaceLeft = maxStack - hotbarQuantities[i];
                int addAmount = Mathf.Min(spaceLeft, quantityToAdd);
                hotbarQuantities[i] += addAmount;
                quantityToAdd -= addAmount;
                hotbarSlots[i].SetItem(hotbar[i], hotbarQuantities[i]);
                if (quantityToAdd <= 0) { UpdateUI(); return true; }
            }
        }

        for (int i = 0; i < hotbar.Count; i++)
        {
            if (hotbar[i] == -1)
            {
                int addAmount = Mathf.Min(quantityToAdd, maxStack);
                hotbar[i] = itemId;
                hotbarQuantities[i] = addAmount;
                quantityToAdd -= addAmount;
                hotbarSlots[i].SetItem(hotbar[i], hotbarQuantities[i]);
                if (quantityToAdd <= 0) { UpdateUI(); return true; }
            }
        }

        UpdateUI();
        return false;
    }

    public void UpdateUI()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            int id = i < inventory.Count ? inventory[i] : -1;
            int qty = i < inventoryQuantities.Count ? inventoryQuantities[i] : 0;
            inventorySlots[i].SetItem(id, qty);
        }

        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            int id = i < hotbar.Count ? hotbar[i] : -1;
            int qty = i < hotbarQuantities.Count ? hotbarQuantities[i] : 0;
            hotbarSlots[i].SetItem(id, qty);
        }
    }

    void SelectHotbar(int index)
    {
        selectedHotbarIndex = index;
        for (int i = 0; i < selected.Length; i++)
            selected[i].SetActive(i == index);
        UpdateUI();
    }

    public void SwapItems(ItemSlotUI slotA, ItemSlotUI slotB)
    {
        int idA = slotA.isHotbarSlot ? hotbar[slotA.slotIndex] : inventory[slotA.slotIndex];
        int qtyA = slotA.isHotbarSlot ? hotbarQuantities[slotA.slotIndex] : inventoryQuantities[slotA.slotIndex];

        int idB = slotB.isHotbarSlot ? hotbar[slotB.slotIndex] : inventory[slotB.slotIndex];
        int qtyB = slotB.isHotbarSlot ? hotbarQuantities[slotB.slotIndex] : inventoryQuantities[slotB.slotIndex];

        if (slotA.isHotbarSlot)
        {
            hotbar[slotA.slotIndex] = idB;
            hotbarQuantities[slotA.slotIndex] = qtyB;
        }
        else
        {
            inventory[slotA.slotIndex] = idB;
            inventoryQuantities[slotA.slotIndex] = qtyB;
        }

        if (slotB.isHotbarSlot)
        {
            hotbar[slotB.slotIndex] = idA;
            hotbarQuantities[slotB.slotIndex] = qtyA;
        }
        else
        {
            inventory[slotB.slotIndex] = idA;
            inventoryQuantities[slotB.slotIndex] = qtyA;
        }

        UpdateUI();
    }

    public void ConsumeSelectedHotbarItem(int amount = 1)
    {
        int index = selectedHotbarIndex;
        if (index < 0 || index >= hotbar.Count) return;
        if (hotbar[index] == -1) return;

        RemoveFromSlot(true, index, amount);
        UpdateUI();
    }

    // === NEW: Main Remove Method ===
    public bool RemoveItem(int itemId, int quantityToRemove)
    {
        Debug.Log($"[RemoveItem] Called for itemId={itemId} qty={quantityToRemove}");

        int quantityLeft = quantityToRemove;

        TryRemoveFromInventory(itemId, ref quantityLeft);
        if (quantityLeft > 0)
            TryRemoveFromHotbar(itemId, ref quantityLeft);

        UpdateUI();
        return quantityLeft <= 0;
    }

    void TryRemoveFromInventory(int itemId, ref int quantityLeft)
    {
        for (int i = 0; i < inventory.Count && quantityLeft > 0; i++)
        {
            if (inventory[i] == itemId && inventoryQuantities[i] > 0)
            {
                int removeAmount = Mathf.Min(quantityLeft, inventoryQuantities[i]);
                RemoveFromSlot(false, i, removeAmount);
                quantityLeft -= removeAmount;
            }
        }
    }

    void TryRemoveFromHotbar(int itemId, ref int quantityLeft)
    {
        for (int i = 0; i < hotbar.Count && quantityLeft > 0; i++)
        {
            if (hotbar[i] == itemId && hotbarQuantities[i] > 0)
            {
                int removeAmount = Mathf.Min(quantityLeft, hotbarQuantities[i]);
                RemoveFromSlot(true, i, removeAmount);
                quantityLeft -= removeAmount;
            }
        }
    }

    void RemoveFromSlot(bool isHotbar, int slotIndex, int removeAmount)
    {
        if (isHotbar)
        {
            hotbarQuantities[slotIndex] -= removeAmount;
            if (hotbarQuantities[slotIndex] <= 0)
            {
                hotbarQuantities[slotIndex] = 0;
                hotbar[slotIndex] = -1;
            }
        }
        else
        {
            inventoryQuantities[slotIndex] -= removeAmount;
            if (inventoryQuantities[slotIndex] <= 0)
            {
                inventoryQuantities[slotIndex] = 0;
                inventory[slotIndex] = -1;
            }
        }
    }
}
