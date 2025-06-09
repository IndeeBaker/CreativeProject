using UnityEngine;
using UnityEngine.Tilemaps;  // Needed for Tilemap and TileBase

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactLayerMask;

    public Tilemap soilTilemap;           // Your base soil tilemap
    public Tilemap nontillableTilemap;    // Tilemap where tilling is blocked
    public TileBase tilledSoilTile;       // The tile to set when tilling soil

    public GameObject flowerPlantPrefab;
    public GameObject carrotPlantPrefab;
    public GameObject wheatPlantPrefab;

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
        // Get the position on the tilemap where the player is interacting
        Vector3 hitPosition = target.transform.position;
        Vector3Int tilePos = soilTilemap.WorldToCell(hitPosition);

        // Check if there’s already a plant here (optional)
        Collider2D existingPlant = Physics2D.OverlapCircle(soilTilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f, 0), 0.1f);
        if (existingPlant != null)
        {
            Debug.Log("Already a plant here.");
            return;
        }

        GameObject prefabToPlant = null;

        switch (seedItem.itemName.ToLower())
        {
            case "flower seed":
                prefabToPlant = flowerPlantPrefab;
                break;
            case "carrot seed":
                prefabToPlant = carrotPlantPrefab;
                break;
            case "wheat seed":
                prefabToPlant = wheatPlantPrefab;
                break;
            default:
                Debug.Log("Unknown seed type.");
                return;
        }

        if (prefabToPlant != null)
        {
            Vector3 spawnPos = soilTilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f, 0); // Center the plant in tile
            Instantiate(prefabToPlant, spawnPos, Quaternion.identity);
            Debug.Log($"Planted {seedItem.itemName} at {tilePos}");

            // TODO: remove one seed from inventory here
        }
    }
}

