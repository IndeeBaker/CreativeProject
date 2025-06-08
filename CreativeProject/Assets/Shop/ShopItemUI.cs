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

    private float lastBuyTime = 0f;
    private float buyCooldown = 0.5f; // Half a second cooldown to prevent double buy

    private void Awake()
    {
        itemData = itemDatabase.GetItemById(IDNumber);

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyClicked);
        }
    }

    private void Start()
    {
        if (itemData != null)
            Setup(itemData);
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
        if (Time.time - lastBuyTime < buyCooldown)
            return; // Ignore if clicked too soon after last click

        lastBuyTime = Time.time;

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
