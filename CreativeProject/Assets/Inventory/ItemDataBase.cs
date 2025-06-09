using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Pickaxe,
    Axe,
    Hoe,
    WateringCan,
    Seeds,
    Consumable,
    Tool,
    Furniture,
    NPC,
    Resource
}

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    [System.Serializable]
    public class ItemData
    {
        public int id;
        public string itemName;
        public Sprite icon;
        public int price;
        public int maxStack;
        public ItemType itemType; // <-- NEW: item type for gameplay logic
    }

    public List<ItemData> items = new List<ItemData>();

    public ItemData GetItemById(int id)
    {
        return items.Find(item => item.id == id);
    }

    public ItemData GetItemByName(string name)
    {
        return items.Find(item => item.itemName == name);
    }
}
