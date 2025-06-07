using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;
    public GameObject[] ShopItems;

    private void Start()
    {
        // Optional: Validate buttons or reassign itemDatabase if needed
        foreach (var shopItemObj in ShopItems)
        {
            ShopItemUI shopItemUI = shopItemObj.GetComponent<ShopItemUI>();
            if (shopItemUI != null)
            {
                shopItemUI.itemDatabase = itemDatabase;
            }
        }
    }
}
