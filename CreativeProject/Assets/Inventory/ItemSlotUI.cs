//This code was built using AI for assistance
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image icon;
    public TMP_Text stack;  // Quantity text
    public int itemId = -1;
    public int quantity = 0;

    public ItemDatabase itemDatabase;

    // New fields for slot identity
    public int slotIndex = -1;
    public bool isHotbarSlot = false;

    private Canvas canvas;
    private GameObject dragIconObj;
    private RectTransform dragIconRect;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    // Set item and quantity, update visuals
    public void SetItem(int newItemId, int newQuantity)
    {
        itemId = newItemId;
        quantity = newQuantity;

        if (itemId >= 0 && quantity > 0)
        {
            var data = itemDatabase.GetItemById(itemId);
            if (data != null && data.icon != null)
            {
                icon.sprite = data.icon;
                icon.color = Color.white;
                stack.text = quantity > 1 ? quantity.ToString() : "";
            }
            else
            {
                ClearSlot();
            }
        }
        else
        {
            ClearSlot();
        }
    }

    void ClearSlot()
    {
        icon.sprite = null;
        icon.color = new Color(1, 1, 1, 0);  // transparent
        stack.text = "";
        itemId = -1;
        quantity = 0;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemId < 0) return;

        dragIconObj = new GameObject("DraggingItemIcon");
        dragIconObj.transform.SetParent(canvas.transform, false);

        dragIconRect = dragIconObj.AddComponent<RectTransform>();
        dragIconRect.sizeDelta = icon.rectTransform.sizeDelta;

        Image dragImage = dragIconObj.AddComponent<Image>();
        dragImage.sprite = icon.sprite;
        dragImage.raycastTarget = false;

        UpdateDragPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIconObj != null)
            UpdateDragPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIconObj != null)
            Destroy(dragIconObj);

        var nearest = InventoryManager.Instance.GetNearestSlot(eventData.position);
        Debug.Log($"OnEndDrag nearest slot: {nearest}, current slot: {this}");

        if (nearest != null && nearest != this)
        {
            Debug.Log($"Swapping items between slot {slotIndex} (hotbar={isHotbarSlot}) and slot {nearest.slotIndex} (hotbar={nearest.isHotbarSlot})");
            InventorySystem.Instance.SwapItems(this, nearest);
        }
        else
        {
            Debug.Log("Dropped outside or on the same slot - no swap.");
        }
    }


    void UpdateDragPosition(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint);

        dragIconRect.localPosition = localPoint;
    }
}
