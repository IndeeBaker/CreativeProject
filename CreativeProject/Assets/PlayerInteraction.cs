//This code was built using AI for assistance
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 2f;
    public LayerMask interactLayerMask;

    [Header("Tilemaps and Tiles")]
    public Tilemap soilTilemap;
    public Tilemap nontillableTilemap;
    public TileBase tilledSoilTile;

    [Header("Plant Prefabs")]
    public GameObject flowerPlantPrefab;
    public GameObject carrotPlantPrefab;
    public GameObject wheatPlantPrefab;

    [Header("Pickup Settings")]
    public float pickupRange = 2f;
    public LayerMask pickupLayerMask; // Layer for dropped items

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryInteract();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryHarvest();
            TryPickupNearbyItems();
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

        if (hit.collider == null)
        {
            Debug.Log("Nothing interactable in range.");
            return;
        }

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
                UseWateringCan(hit.point);
                break;

            case ItemType.Seeds:
                PlantSeed(hit.point, selectedItem);
                break;

            default:
                Debug.Log("Item type not usable for interaction.");
                break;
        }
    }

    void TryHarvest()
    {
        int cropsLayerMask = LayerMask.GetMask("Crops");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, interactRange, cropsLayerMask);

        if (hit.collider == null)
        {
            Debug.Log("Nothing to harvest in range.");
            return;
        }

        CropGrowth crop = hit.collider.GetComponent<CropGrowth>();

        if (crop == null)
        {
            Debug.Log("No crop to harvest in front.");
            return;
        }

        if (crop.currentStage < crop.growthStages.Length - 1)
        {
            Debug.Log("Crop is not fully grown yet.");
            return;
        }

        Debug.Log("Harvesting crop: " + crop.name);

        int cropItemID = crop.harvestItemID;

        if (cropItemID == -1)
        {
            Debug.LogWarning("Crop item ID not set in CropGrowth.");
            return;
        }

        bool added = InventorySystem.Instance.AddItemToHotbar(cropItemID, 1);

        if (added)
        {
            Destroy(crop.gameObject);
            Debug.Log("Crop harvested and added to hotbar!");
        }
        else
        {
            Debug.Log("Hotbar full, cannot add crop.");
        }
    }

    void TryPickupNearbyItems()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRange, pickupLayerMask);

        if (hits.Length == 0)
        {
            Debug.Log("No dropped items to pick up nearby.");
            return;
        }

        foreach (Collider2D hit in hits)
        {
            DroppedItem droppedItem = hit.GetComponent<DroppedItem>();
            if (droppedItem == null)
            {
                Debug.LogWarning("Dropped item missing DroppedItem component.");
                continue;
            }

            int itemId = droppedItem.itemId;
            int quantity = droppedItem.quantity;

            // Try to add to hotbar first, then inventory if hotbar full
            bool addedToHotbar = InventorySystem.Instance.AddItemToHotbar(itemId, quantity);
            if (!addedToHotbar)
            {
                bool addedToInventory = InventorySystem.Instance.AddItem(itemId, quantity);
                if (!addedToInventory)
                {
                    Debug.Log("Inventory and hotbar are full! Cannot pick up item.");
                    continue;
                }
            }

            Destroy(droppedItem.gameObject);
            Debug.Log($"Picked up {quantity} of item ID {itemId}");
        }
    }

    void UsePickaxe(GameObject target)
    {
        // TODO: Implement breaking rock logic here
        Debug.Log("Breaking rock: " + target.name);
    }

    void UseAxe(GameObject target)
    {
        // TODO: Implement chopping tree logic here
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

    void UseWateringCan(Vector2 hitPoint)
    {
        Collider2D cropCollider = Physics2D.OverlapCircle(hitPoint, 0.2f, LayerMask.GetMask("Crops"));
        if (cropCollider == null)
        {
            Debug.Log("No crop to water.");
            return;
        }

        CropGrowth crop = cropCollider.GetComponent<CropGrowth>();
        if (crop == null)
        {
            Debug.Log("Crop found but missing CropGrowth script.");
            return;
        }

        crop.WaterCrop();
        Debug.Log("Watered crop: " + crop.name);
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

        if (tile != tilledSoilTile)
        {
            Debug.Log("You can only plant on tilled soil.");
            return;
        }

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

        GameObject prefabToPlant = seedItem.itemName.ToLower() switch
        {
            "flower seed" or "flower seeds" => flowerPlantPrefab,
            "carrot seed" or "carrot seeds" => carrotPlantPrefab,
            "wheat seed" or "wheat seeds" => wheatPlantPrefab,
            _ => null
        };

        if (prefabToPlant == null)
        {
            Debug.Log("Unknown seed type: " + seedItem.itemName);
            return;
        }

        Vector3 spawnPos = soilTilemap.GetCellCenterWorld(tilePos);
        Instantiate(prefabToPlant, spawnPos, Quaternion.identity);
        Debug.Log($"Planted {seedItem.itemName} at {tilePos}");

        InventorySystem.Instance.ConsumeSelectedHotbarItem(1);
    }
}
