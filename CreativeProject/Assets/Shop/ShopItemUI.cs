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

    private bool buyInProgress = false;
    private bool sellInProgress = false;

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
        if (buyInProgress) return;
        buyInProgress = true;
        StartCoroutine(ResetBuyFlag());

        if (itemData == null)
            return;

        int price = itemData.price;

        if (PlayerWallet.Instance.TrySpendMoney(price))
        {
            bool added = InventorySystem.Instance.AddItem(itemData.id, 1);
            if (added)
            {
                Debug.Log($"Bought 1 {itemData.itemName} for ${price}");
            }
            else
            {
                Debug.LogWarning("Inventory full, refunding money.");
                PlayerWallet.Instance.AddMoney(price);
            }
        }
        else
        {
            Debug.LogWarning("Not enough money to buy this item.");
        }
    }

    public void OnSellClicked()
    {
        if (sellInProgress) return;
        sellInProgress = true;
        StartCoroutine(ResetSellFlag());

        if (itemData == null)
            return;

        int sellPrice = itemData.price;

        bool removed = InventorySystem.Instance.RemoveItem(itemData.id, 1);

        if (removed)
        {
            PlayerWallet.Instance.AddMoney(sellPrice);
            Debug.Log($"Sold 1 {itemData.itemName} for ${sellPrice}");
        }
        else
        {
            Debug.LogWarning("You don't have this item to sell.");
        }
    }

    private System.Collections.IEnumerator ResetBuyFlag()
    {
        yield return null;
        buyInProgress = false;
    }

    private System.Collections.IEnumerator ResetSellFlag()
    {
        yield return null;
        sellInProgress = false;
    }
}
