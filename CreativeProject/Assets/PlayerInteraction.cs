using UnityEngine;
using UnityEngine.Tilemaps;  // Needed for Tilemap and TileBase

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactLayerMask;

    // Step 1: Add references for soil tilemap and tilled soil tile
    public Tilemap soilTilemap;
    public TileBase tilledSoilTile;
    public GameObject tilledSoilPrefab;


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

        // Overlap circle centered at player (adjust offset if needed)
        Vector2 center = (Vector2)transform.position + (Vector2)transform.right;
        Collider2D hit = Physics2D.OverlapCircle(center, interactRange, interactLayerMask);

        if (hit != null)
        {
            Debug.Log($"Interacting with {hit.name} using {selectedItem.itemName} ({selectedItem.itemType})");

            switch (selectedItem.itemType)
            {
                case ItemType.Pickaxe:
                    UsePickaxe(hit.gameObject);
                    break;
                case ItemType.Axe:
                    UseAxe(hit.gameObject);
                    break;
                case ItemType.Hoe:
                    UseHoe(hit.gameObject);
                    break;
                case ItemType.WateringCan:
                    UseWateringCan(hit.gameObject);
                    break;
                case ItemType.Seeds:
                    PlantSeed(hit.gameObject, selectedItem);
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
        Debug.Log("Tilling soil with hoe");

        Vector3 worldPos = transform.position + transform.right * 0.5f; // In front of player
        Vector3Int cellPos = soilTilemap.WorldToCell(worldPos);        // Snap to tile grid
        TileBase existingTile = soilTilemap.GetTile(cellPos);

        if (existingTile != null && existingTile != tilledSoilTile)
        {
            soilTilemap.SetTile(cellPos, tilledSoilTile);
            Debug.Log($"Tilled soil at {cellPos}");
        }
        else
        {
            Debug.Log("Tile already tilled or no tile present.");
        }
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
