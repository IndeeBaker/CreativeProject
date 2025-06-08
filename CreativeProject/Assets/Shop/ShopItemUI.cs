using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Button buyButton;
    public int IDNumber;

    public ItemDatabase itemDatabase;
    public ItemDatabase.ItemData itemData;

    private void Awake()
    {
        itemData = itemDatabase.GetItemById(IDNumber);
    }

    private void Start()
    {
        if (itemData != null)
            Setup(itemData);

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyClicked);
        }
    }

    public void Setup(ItemDatabase.ItemData data)
    {
        itemData = data;
        if (data.icon != null)
            icon.sprite = data.icon;

        nameText.text = data.itemName;
        priceText.text = $"${data.price}";
    }

    public void OnBuyClicked()
    {
        if (itemData != null)
        {
            bool added = InventorySystem.Instance.AddItem(itemData.id, 1);
            if (added)
            {
                Debug.Log($"Bought 1 {itemData.itemName}");
                // TODO: Deduct player money here
            }
            else
            {
                Debug.LogWarning("Inventory full, cannot buy item.");
            }
        }
    }
}
