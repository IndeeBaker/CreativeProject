using UnityEngine;
using UnityEngine.Tilemaps;  // Needed for Tilemap and TileBase

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactLayerMask;

    public Tilemap soilTilemap;           // Your base soil tilemap
    public Tilemap nontillableTilemap;    // Tilemap where tilling is blocked
    public TileBase tilledSoilTile;       // The tile to set when tilling soil

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
                    UseHoe(hit.point);  // Pass hit position for tile check
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

    // Updated UseHoe to accept Vector2 point instead of GameObject target
    void UseHoe(Vector2 hitPosition)
    {
        Vector3Int tilePosition = soilTilemap.WorldToCell(hitPosition);

        // Check if nontillable tile exists at this position
        if (nontillableTilemap.HasTile(tilePosition))
        {
            Debug.Log("Cannot till here — area is protected or non-tillable.");
            return;
        }

        // Otherwise, set the tilled soil tile
        soilTilemap.SetTile(tilePosition, tilledSoilTile);
        Debug.Log("Soil tilled at " + tilePosition);
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
