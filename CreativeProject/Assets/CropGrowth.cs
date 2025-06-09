using UnityEngine;

public class CropGrowth : MonoBehaviour
{
    public Sprite[] growthStages;
    public int currentStage = 0;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();

        // Subscribe to day-change event
        TimeManager.NewDayStarted += AdvanceGrowth;
    }

    void OnDestroy()
    {
        TimeManager.NewDayStarted -= AdvanceGrowth;
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
}
