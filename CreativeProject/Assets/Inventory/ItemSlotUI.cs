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

        if (icon == null)
        {
            Debug.LogError($"Icon Image is not assigned on {gameObject.name}");
            return;
        }
        if (stack == null)
        {
            Debug.LogError($"Stack Text is not assigned on {gameObject.name}");
            return;
        }
        if (itemDatabase == null)
        {
            Debug.LogError($"ItemDatabase is not assigned on {gameObject.name}");
            return;
        }

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
                Debug.LogError($"ItemData not found for ID {itemId} on {gameObject.name}");
                icon.sprite = null;
                icon.color = new Color(1, 1, 1, 0);
                stack.text = "";
            }
        }
    }
}
