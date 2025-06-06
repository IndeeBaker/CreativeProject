using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;
    public GameObject shopItemPrefab;
    public Transform shopContentParent;

    void Start()
    {
        PopulateShop();
    }

    void PopulateShop()
    {
        foreach (ItemData item in itemDatabase.items)
        {
            GameObject obj = Instantiate(shopItemPrefab, shopContentParent);
            ShopItemUI ui = obj.GetComponent<ShopItemUI>();
            ui.Setup(item);
        }
    }
}

