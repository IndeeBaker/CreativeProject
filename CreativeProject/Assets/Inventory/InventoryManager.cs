using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<ItemSlotUI> allSlots = new List<ItemSlotUI>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public ItemSlotUI GetNearestSlot(Vector2 screenPos)
    {
        float closest = float.MaxValue;
        ItemSlotUI nearest = null;

        foreach (var slot in allSlots)
        {
            float dist = Vector2.Distance(screenPos, RectTransformUtility.WorldToScreenPoint(null, slot.transform.position));
            if (dist < closest)
            {
                closest = dist;
                nearest = slot;
            }
        }

        return nearest;
    }
}
