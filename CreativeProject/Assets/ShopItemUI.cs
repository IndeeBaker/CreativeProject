using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    private ItemData itemData;

    public void Setup(ItemData data)
    {
        itemData = data;
        icon.sprite = data.icon;
        nameText.text = data.itemName;
        priceText.text = data.price + " G"; // G for Gold
        buyButton.onClick.AddListener(BuyItem);
    }

    void BuyItem()
    {
        if (InventorySystemHasRoom())
        {
            InventorySystem inventory = FindObjectOfType<InventorySystem>();
            inventory.inventory.Add(itemData.id);
            inventory.inventorySlots[inventory.inventory.Count - 1].SetItem(itemData.id);

            Debug.Log($"Bought {itemData.itemName} for {itemData.price}G");
        }
        else
        {
            Debug.Log("Inventory full!");
        }
    }

    bool InventorySystemHasRoom()
    {
        InventorySystem inventory = FindObjectOfType<InventorySystem>();
        return inventory.inventory.Contains(-1);
    }
}
