using UnityEngine;
using UnityEngine.Tilemaps;  // Needed for Tilemap and TileBase

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactLayerMask;

    // Step 1: Add references for soil tilemap and tilled soil tile
    public Tilemap soilTilemap;
    public TileBase tilledSoilTile;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))  // Interaction key
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        var selectedItem = InventorySystem.Instance.GetHeldItemData();

        if (selectedItem == null)
        {
            Debug.Log("No item selected.");
            return;
        }

        // Raycast forward (adjust direction if needed)
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, interactRange, interactLayerMask);

        if (hit.collider != null)
        {
            Debug.Log($"Interacting with {hit.collider.name} using {selectedItem.itemName} ({selectedItem.itemType})");

            switch (selectedItem.itemType)
            {
                case ItemType.Pickaxe:
                    UsePickaxe(hit.collider.gameObject);
                    break;

                case ItemType.Axe:
                    UseAxe(hit.collider.gameObject);
                    break;

                case ItemType.Hoe:
                    UseHoe(hit.collider.gameObject);
                    break;

                case ItemType.WateringCan:
                    UseWateringCan(hit.collider.gameObject);
                    break;

                case ItemType.Seeds:
                    PlantSeed(hit.collider.gameObject, selectedItem);
                    break;

                default:
                    Debug.Log("Item type not usable for interaction.");
                    break;
            }
        }
        else
        {
            Debug.Log("Nothing interactable in range.");
        }
    }

    void UsePickaxe(GameObject target)
    {
        Debug.Log("Breaking rock: " + target.name);
        // TODO: rock breaking logic here
    }

    void UseAxe(GameObject target)
    {
        Debug.Log("Chopping tree: " + target.name);
        // TODO: tree chopping logic here
    }

    void UseHoe(GameObject target)
    {
        Debug.Log("Tilling soil: " + target.name);
        // TODO: tilling logic will go here in next step
    }

    void UseWateringCan(GameObject target)
    {
        Debug.Log("Watering plant: " + target.name);
        // TODO: watering logic here
    }

    void PlantSeed(GameObject target, ItemDatabase.ItemData seedItem)
    {
        Debug.Log($"Planting {seedItem.itemName} on {target.name}");
        // TODO: planting logic here
    }
}
