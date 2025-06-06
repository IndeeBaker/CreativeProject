using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int id;
    public string itemName;
    public Sprite icon;
    public int price;  // Add price here
}

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public ItemData[] items;

    // Returns the item with matching id or null if not found
    public ItemData GetItemById(int id)
    {
        foreach (var item in items)
        {
            if (item.id == id)
                return item;
        }
        return null;
    }
}
