using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public int id;
        public string itemName;
        public Sprite icon;
        public int price;
        public int maxStack = 99;
    }

    public List<ItemData> items = new List<ItemData>();

    public ItemData GetItemById(int id)
    {
        return items.Find(item => item.id == id);
    }
}
