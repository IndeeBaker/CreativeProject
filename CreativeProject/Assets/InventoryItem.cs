[System.Serializable]
public class InventoryItem
{
    public int itemId;
    public int quantity;

    public InventoryItem(int id, int qty = 1)
    {
        itemId = id;
        quantity = qty;
    }
}
