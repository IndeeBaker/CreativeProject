//this code was developed using ai for assisstance

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite icon;
}

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
}