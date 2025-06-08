using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text stack;  // The UI text showing quantity
    public int itemId = -1;
    public int quantity = 0;

    public ItemDatabase itemDatabase;

    public void SetItem(int newItemId, int newQuantity)
    {
        itemId = newItemId;
        quantity = newQuantity;

        if (itemId == -1 || quantity <= 0)
        {
            // Clear slot visuals
            icon.sprite = null;
            icon.color = new Color(1, 1, 1, 0);  // Transparent
            stack.text = "";
        }
        else
        {
            var data = itemDatabase.GetItemById(itemId);
            if (data != null)
            {
                icon.sprite = data.icon;
                icon.color = Color.white;
                stack.text = quantity > 1 ? quantity.ToString() : "";
            }
            else
            {
                // Item data missing, clear visuals
                icon.sprite = null;
                icon.color = new Color(1, 1, 1, 0);
                stack.text = "";
            }
        }
    }
}
