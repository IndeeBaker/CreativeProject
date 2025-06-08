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

    public GameObject[] selected; // For hotbar item visuals

    public ItemDatabase itemDatabase;

    public int selectedHotbarIndex = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        int inventorySize = 10;  // Set your inventory size here
        int hotbarSize = 10;

        inventory.Clear();
        inventoryQuantities.Clear();
        hotbar.Clear();
        hotbarQuantities.Clear();

        // Initialize inventory slots with empty (-1) and zero quantity
        for (int i = 0; i < inventorySize; i++)
        {
            inventory.Add(-1);
            inventoryQuantities.Add(0);
        }

        // Initialize hotbar slots - example: first 4 items set, rest empty
        hotbar = new List<int> { 0, 1, 2, 3, -1, -1, -1, -1, -1, -1 };
        hotbarQuantities = new List<int> { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 };

        // Update all UI slots at start
        UpdateUI();

        // Select first hotbar slot by default
        SelectHotbar(0);
    }

    void Update()
    {
        HandleHotbarInput();
    }

    void HandleHotbarInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { selectedHotbarIndex = 0; SelectHotbar(0); }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { selectedHotbarIndex = 1; SelectHotbar(1); }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { selectedHotbarIndex = 2; SelectHotbar(2); }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) { selectedHotbarIndex = 3; SelectHotbar(3); }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) { selectedHotbarIndex = 4; SelectHotbar(4); }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) { selectedHotbarIndex = 5; SelectHotbar(5); }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) { selectedHotbarIndex = 6; SelectHotbar(6); }
        else if (Input.GetKeyDown(KeyCode.Alpha8)) { selectedHotbarIndex = 7; SelectHotbar(7); }
        else if (Input.GetKeyDown(KeyCode.Alpha9)) { selectedHotbarIndex = 8; SelectHotbar(8); }
        else if (Input.GetKeyDown(KeyCode.Alpha0)) { selectedHotbarIndex = 9; SelectHotbar(9); }

        // Debug to confirm selected hotbar index
        Debug.Log("Selected hotbar index: " + selectedHotbarIndex);
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
        if (itemId < 0)
        {
            Debug.LogWarning("AddItem called with invalid itemId: " + itemId);
            return false;
        }

        var itemData = itemDatabase.GetItemById(itemId);
        if (itemData == null)
        {
            Debug.LogWarning("AddItem called with unknown itemId: " + itemId);
            return false;
        }

        int maxStack = itemData.maxStack;

        // Try to stack into existing slots first (inventory)
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] == itemId && inventoryQuantities[i] < maxStack)
            {
                int spaceLeft = maxStack - inventoryQuantities[i];
                int addAmount = Mathf.Min(spaceLeft, quantityToAdd);
                inventoryQuantities[i] += addAmount;
                quantityToAdd -= addAmount;

                inventorySlots[i].SetItem(inventory[i], inventoryQuantities[i]);

                if (quantityToAdd <= 0)
                {
                    UpdateUI();
                    return true;
                }
            }
        }

        // Add to empty slots if any quantity remains
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] == -1)
            {
                int addAmount = Mathf.Min(quantityToAdd, maxStack);
                inventory[i] = itemId;
                inventoryQuantities[i] = addAmount;
                quantityToAdd -= addAmount;

                inventorySlots[i].SetItem(inventory[i], inventoryQuantities[i]);

                if (quantityToAdd <= 0)
                {
                    UpdateUI();
                    return true;
                }
            }
        }

        // No space left
        Debug.LogWarning("Inventory full or not enough space to add all items.");
        UpdateUI();
        return false;
    }

    void UpdateUI()
    {
        // Update inventory UI slots
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            int id = (i < inventory.Count) ? inventory[i] : -1;
            int qty = (i < inventoryQuantities.Count) ? inventoryQuantities[i] : 0;
            inventorySlots[i].SetItem(id, qty);
        }

        // Update hotbar UI slots
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            int id = (i < hotbar.Count) ? hotbar[i] : -1;
            int qty = (i < hotbarQuantities.Count) ? hotbarQuantities[i] : 0;
            hotbarSlots[i].SetItem(id, qty);
        }
    }

    void SelectHotbar(int index)
    {
        selectedHotbarIndex = index;

        for (int i = 0; i < selected.Length; i++)
        {
            selected[i].SetActive(i == index);
        }

        UpdateUI();
    }
}
