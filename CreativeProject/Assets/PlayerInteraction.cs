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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryHarvest();
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
        else
        {
            Debug.Log("Nothing interactable in range.");
        }
    }

    void TryHarvest()
    {
        // Use the Crops layer specifically for harvesting
        int cropsLayerMask = LayerMask.GetMask("Crops");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, interactRange, cropsLayerMask);

        if (hit.collider != null)
        {
            CropGrowth crop = hit.collider.GetComponent<CropGrowth>();

            if (crop != null)
            {
                if (crop.currentStage >= crop.growthStages.Length - 1)
                {
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
                else
                {
                    Debug.Log("Crop is not fully grown yet.");
                }
            }
            else
            {
                Debug.Log("No crop to harvest in front.");
            }
        }
        else
        {
            Debug.Log("Nothing to harvest in range.");
        }
    }

    int GetItemIDFromCrop(CropGrowth crop)
    {
        string cropName = crop.name.ToLower();

        if (cropName.Contains("wheat"))
            return 11; // Correct ID for wheat
        if (cropName.Contains("carrot"))
            return 12; // Correct ID for carrot
        if (cropName.Contains("flower"))
            return 13; // Correct ID for flower

        return -1; // Unknown crop
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

    void UseWateringCan(Vector2 hitPoint)
    {
        Collider2D cropCollider = Physics2D.OverlapCircle(hitPoint, 0.2f, LayerMask.GetMask("Crops"));
        if (cropCollider != null)
        {
            CropGrowth crop = cropCollider.GetComponent<CropGrowth>();
            if (crop != null)
            {
                crop.WaterCrop();
                Debug.Log("Watered crop: " + crop.name);
            }
            else
            {
                Debug.Log("Crop found but missing CropGrowth script.");
            }
        }
        else
        {
            Debug.Log("No crop to water.");
        }
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

        if (prefabToPlant != null)
        {
            Vector3 spawnPos = soilTilemap.GetCellCenterWorld(tilePos);
            Instantiate(prefabToPlant, spawnPos, Quaternion.identity);
            Debug.Log($"Planted {seedItem.itemName} at {tilePos}");

            InventorySystem.Instance.ConsumeSelectedHotbarItem(1);
        }
    }
}
