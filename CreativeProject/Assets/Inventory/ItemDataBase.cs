//This code was built using AI for assistance
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
        public int maxStack = 1;
        public ItemType itemType;

        [Header("Inventory Settings")]
        [Tooltip("If true, this item is added to the inventory when bought.")]
        public bool goesToInventory = true;

        [Header("Prefab Settings")]
        [Tooltip("Prefab to instantiate when buying this item, if it doesn't go to inventory.")]
        public GameObject spawnPrefab;

        [Tooltip("Prefab to instantiate when planting this seed (optional, used for farming).")]
        public GameObject plantPrefab;
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
