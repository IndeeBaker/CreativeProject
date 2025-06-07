using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework.Interfaces;

public class ShopItemUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Button buyButton;
    public int IDNumber;

    public ItemDatabase itemDatabase;
    public ItemData itemData;

    private bool hasBeenPurchased = false; // Prevent multiple purchases

    private void Awake()
    {
        // Corrected: Assign to the field, not a local variable
        itemData = itemDatabase.GetItemById(IDNumber);
    }

    private void Start()
    {
        if (itemData != null)
        {
            Setup(itemData);
        }

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners(); // Prevent duplicates
            buyButton.onClick.AddListener(OnBuyClicked);
        }
        else
        {
            Debug.LogError("Buy Button is not assigned in ShopItemUI!");
        }
    }

    public void Setup(ItemData data)
    {
        itemData = data;

        if (data.icon != null)
        {
            icon.sprite = data.icon;
        }
        else
        {
            Debug.LogWarning($"Item {data.itemName} has no icon assigned!");
        }

        nameText.text = data.itemName;
        priceText.text = $"${data.price}";

        Debug.Log($"Set up {data.itemName}: Price = {data.price}, Icon = {(data.icon != null ? data.icon.name : "NULL")}");
    }

    public void OnBuyClicked()
    {
        if (hasBeenPurchased)
        {
            Debug.LogWarning($"Item {itemData.itemName} has already been purchased.");
            return;
        }

        if (itemData != null)
        {
            Debug.Log($"Buy clicked for {itemData.itemName} for {itemData.price} at {Time.time}");
            buyLogic();
            hasBeenPurchased = true; // Mark as purchased
            // Optionally disable button to prevent further clicks
            // buyButton.interactable = false;
        }
    }

    public void buyLogic()
    {
        // buy logic - find nearest slot with -1 (empty), assign item ID
        // Use item ID found in itemdata.id 
        // send item data to logic to enter hotbar/inventory in nearest empty slot.
        // Logic from end drag function in ItemSlotUI:
        // var nearest = InventoryManager.Instance.GetNearestSlot(eventData.position);
        // finds nearest empty slot and puts item there

        // Make sure the item data is valid
        if (itemData == null)
        {
            Debug.LogWarning("Attempted to buy null item!");
            return;
        }

        // Search for the first empty inventory slot
        foreach (var slot in InventoryManager.Instance.allSlots)
        {
            if (slot.itemId == -1) // Slot is empty
            {
                slot.SetItem(itemData.id); // Assign the item
                Debug.Log($"Added {itemData.itemName} to inventory slot.");
                return;
            }
        }

        // No empty slots found
        Debug.LogWarning("Inventory is full. Could not add item.");
    }
}

// Example purchase check logic to include later:
// int playerMoney = PlayerInventory.Instance.coins;
// if (playerMoney >= itemData.price)
// {
//     PlayerInventory.Instance.coins -= itemData.price;
//     buyLogic();
// }
// else
// {
//     Debug.Log("Not enough money!");
// }
