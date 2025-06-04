using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image icon;
    public int itemId = -1;
    public ItemDatabase itemDatabase;

    private Canvas canvas;
    private GameObject dragIconObj;
    private RectTransform dragIconRect;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void SetItem(int id)
    {
        itemId = id;

        if (id >= 0)
        {
            ItemData data = itemDatabase.GetItemById(id);
            icon.sprite = data.icon;
            icon.color = Color.white;
        }
        else
        {
            icon.sprite = null;
            icon.color = new Color(1, 1, 1, 0);
        }
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
        if (nearest != null && nearest != this)
        {
            int temp = nearest.itemId;
            nearest.SetItem(this.itemId);
            this.SetItem(temp);
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
