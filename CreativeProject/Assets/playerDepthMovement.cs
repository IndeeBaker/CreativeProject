//this script was made using AI for assistance

using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]


public class playerDepthMovement : MonoBehaviour
{
    public int baseSortingOrder = 1000;
    [Tooltip("Offset to add to sortingOrder (useful to stack objects in same position)")]
    public int orderOffset = 0;

    [Tooltip("Sorting layer to assign this object to")]
    public string sortingLayerName = "Default";

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!string.IsNullOrEmpty(sortingLayerName))
        {
            spriteRenderer.sortingLayerName = sortingLayerName;
        }
    }

    void LateUpdate()
    {
        // Sorting order is inverted Y position times 100, plus optional offset
        spriteRenderer.sortingOrder = baseSortingOrder + Mathf.RoundToInt(transform.position.y * -100) + orderOffset;
    }
}