using UnityEngine;

public class CropGrowth : MonoBehaviour
{
    public Sprite[] growthStages;
    public int currentStage = 0;
    public bool isWatered = false;

    public int harvestItemID; // ? Used to determine what item is harvested (e.g. wheat = 11)

    private SpriteRenderer spriteRenderer;
    private GameObject wateredOverlayInstance;

    public GameObject wateredOverlayPrefab;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();

        TimeManager.NewDayStarted += OnNewDay;
    }

    void OnDestroy()
    {
        TimeManager.NewDayStarted -= OnNewDay;
    }

    void OnNewDay()
    {
        if (isWatered)
        {
            AdvanceGrowth();
        }

        isWatered = false;
        RemoveWateredOverlay();
        UpdateWaterVisual();
    }

    public void WaterCrop()
    {
        isWatered = true;
        ShowWateredOverlay();
        UpdateWaterVisual();
    }

    void AdvanceGrowth()
    {
        if (currentStage < growthStages.Length - 1)
        {
            currentStage++;
            UpdateSprite();
        }
    }

    void UpdateSprite()
    {
        if (spriteRenderer != null && growthStages.Length > 0)
        {
            spriteRenderer.sprite = growthStages[currentStage];
        }
    }

    void UpdateWaterVisual()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isWatered ? new Color(0.8f, 0.9f, 1f) : Color.white;
        }
    }

    void ShowWateredOverlay()
    {
        if (wateredOverlayInstance == null && wateredOverlayPrefab != null)
        {
            Vector3 overlayPosition = transform.position;
            wateredOverlayInstance = Instantiate(wateredOverlayPrefab, overlayPosition, Quaternion.identity, transform);
        }
    }

    void RemoveWateredOverlay()
    {
        if (wateredOverlayInstance != null)
        {
            Destroy(wateredOverlayInstance);
            wateredOverlayInstance = null;
        }
    }
}
