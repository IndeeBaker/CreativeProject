//this script was made using AI for assistance

using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerDepthMovement : MonoBehaviour
{
    public int orderOffset = 0;
    public string sortingLayerName = "Player";

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingLayerName = sortingLayerName;
    }

    void LateUpdate()
    {
        sr.sortingOrder = Mathf.RoundToInt(transform.position.y * -100f) + orderOffset;

        // Force correct sorting layer if needed
        if (sr.sortingLayerName != sortingLayerName)
            sr.sortingLayerName = sortingLayerName;
    }
}