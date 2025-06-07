using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<int> inventory = new List<int>();
    public List<int> hotbar = new List<int>();

    public List<ItemSlotUI> inventorySlots;
    public List<ItemSlotUI> hotbarSlots;

    public GameObject[] selected;
    public ItemDatabase itemDatabase;

    public int selectedHotbarIndex = 1;

    void Start()
    {
        inventory = new List<int> { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        hotbar = new List<int> { 0, 1, 2, 3, -1, -1, -1, -1, -1, -1 };

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            int itemId = (i < inventory.Count) ? inventory[i] : -1;
            inventorySlots[i].SetItem(itemId);
        }

        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            int itemId = (i < hotbar.Count) ? hotbar[i] : -1;
            hotbarSlots[i].SetItem(itemId);
        }
        selectedHotbar(0);
    }

    void Update()
    {
        HandleHotbarInput();
    }

    void HandleHotbarInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { selectedHotbarIndex = 0; selectedHotbar(0); }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {selectedHotbarIndex = 1; selectedHotbar(1);}
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {selectedHotbarIndex = 2; selectedHotbar(2);}
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {selectedHotbarIndex = 3; selectedHotbar(3);}
        else if (Input.GetKeyDown(KeyCode.Alpha5)) {selectedHotbarIndex = 4; selectedHotbar(4);}
        else if (Input.GetKeyDown(KeyCode.Alpha6)) {selectedHotbarIndex = 5; selectedHotbar(5);}
        else if (Input.GetKeyDown(KeyCode.Alpha7)) {selectedHotbarIndex = 6; selectedHotbar(6);}
        else if (Input.GetKeyDown(KeyCode.Alpha8)) {selectedHotbarIndex = 7; selectedHotbar(7);}
        else if (Input.GetKeyDown(KeyCode.Alpha9)) {selectedHotbarIndex = 8; selectedHotbar(8);}
        else if (Input.GetKeyDown(KeyCode.Alpha0)) {selectedHotbarIndex = 9; selectedHotbar(9);}

        // Optional: Debug or highlight selected slot
        Debug.Log("Selected hotbar index: " + selectedHotbarIndex);
        //CheckItem();
        //attach all item scripts to player
        //in here activate and deactivate, have all deactivated and then if hoe is selected, activate hoe script
        //add scrips to array then activate them based on the item number from the database ID - see start
    }

    public int GetHeldItemId()
    {
        if (selectedHotbarIndex >= 0 && selectedHotbarIndex < hotbar.Count)
            return hotbar[selectedHotbarIndex];
        return -1;
    }

    public ItemData GetHeldItemData()
    {
        int itemId = GetHeldItemId();
        return itemId >= 0 ? itemDatabase.GetItemById(itemId) : null;
    }


    void selectedHotbar(int selection)
    {
        for (int i = 0; i < selected.Length; i++) 
        {
            if (i != selection)
            {
                selected[i].SetActive(false);
            }
            else
            {
                selected[i].SetActive(true);
            }
        }
    }
}

//void TryChopTree()
//{
//    var heldItem = FindObjectOfType<InventorySystem>().GetHeldItemData();

//    if (heldItem != null && heldItem.itemName == "Axe")
//    {
//        Debug.Log("Tree chopped!");
//        // Do chopping logic
//    }
//    else
//    {
//        Debug.Log("You need an axe to chop this tree.");
//    }
//}
