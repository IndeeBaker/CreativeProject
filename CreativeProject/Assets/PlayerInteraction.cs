using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactLayerMask;

    public Tilemap soilTilemap;
    public Tilemap nontillableTilemap;
    public TileBase tilledSoilTile;

    public GameObject flowerPlantPrefab;
    public GameObject carrotPlantPrefab;
    public GameObject wheatPlantPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
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
                    UseHoe(hit.point);
                    break;

                case ItemType.WateringCan:
                    UseWateringCan(hit.collider.gameObject);
                    break;

                case ItemType.Seeds:
                    PlantSeed(hit.point, selectedItem);
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
    }

    void UseAxe(GameObject target)
    {
        Debug.Log("Chopping tree: " + target.name);
    }

    void UseHoe(Vector2 hitPosition)
    {
        Vector3Int tilePosition = soilTilemap.WorldToCell(hitPosition);

        if (nontillableTilemap.HasTile(tilePosition))
        {
            Debug.Log("Cannot till here — area is protected or non-tillable.");
            return;
        }

        soilTilemap.SetTile(tilePosition, tilledSoilTile);
        Debug.Log("Soil tilled at " + tilePosition);
    }

    void UseWateringCan(GameObject target)
    {
        Debug.Log("Watering plant: " + target.name);
    }

    void PlantSeed(Vector2 hitPosition, ItemDatabase.ItemData seedItem)
    {
        Vector3Int tilePos = soilTilemap.WorldToCell(hitPosition);
        TileBase tile = soilTilemap.GetTile(tilePos);

        if (tile == null)
        {
            Debug.Log("No tile here.");
            return;
        }

        // Directly compare tile instance to the tilled soil tile
        if (tile != tilledSoilTile)
        {
            Debug.Log("You can only plant on tilled soil.");
            return;
        }

        // Check for existing plant on "Crops" layer
        Collider2D existingPlant = Physics2D.OverlapCircle(
            soilTilemap.GetCellCenterWorld(tilePos),
            0.1f,
            LayerMask.GetMask("Crops")
        );

        if (existingPlant != null)
        {
            Debug.Log("Already a plant here.");
            return;
        }

        // Choose plant prefab
        GameObject prefabToPlant = null;
        string seedName = seedItem.itemName.ToLower().Trim();

        switch (seedName)
        {
            case "flower seed":
            case "flower seeds":
                prefabToPlant = flowerPlantPrefab;
                break;

            case "carrot seed":
            case "carrot seeds":
                prefabToPlant = carrotPlantPrefab;
                break;

            case "wheat seed":
            case "wheat seeds":
                prefabToPlant = wheatPlantPrefab;
                break;

            default:
                Debug.Log("Unknown seed type: " + seedItem.itemName);
                return;
        }

        // Plant the seed
        if (prefabToPlant != null)
        {
            Vector3 spawnPos = soilTilemap.GetCellCenterWorld(tilePos);
            Instantiate(prefabToPlant, spawnPos, Quaternion.identity);
            Debug.Log($"Planted {seedItem.itemName} at {tilePos}");

            InventorySystem.Instance.ConsumeSelectedHotbarItem(1);
            // InventorySystem.Instance.RemoveItem(seedItem.id, 1);
        }
    }
}
