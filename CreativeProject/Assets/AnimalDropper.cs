using UnityEngine;

public class AnimalDropper : MonoBehaviour
{
    public GameObject dropItemPrefab;      // The item prefab to drop
    public Transform dropPoint;             // Where the item will spawn (assign in inspector)
    public int dropIntervalDays = 3;        // Drop every N days

    private int lastDropDay = 0;

    private void OnEnable()
    {
        TimeManager.NewDayStarted += OnNewDayStarted;
    }

    private void OnDisable()
    {
        TimeManager.NewDayStarted -= OnNewDayStarted;
    }

    private void Start()
    {
        lastDropDay = TimeManagerInstance.currentDay; // Initialize with current day
    }

    private void OnNewDayStarted()
    {
        int currentDay = TimeManagerInstance.currentDay;
        if (currentDay - lastDropDay >= dropIntervalDays)
        {
            DropItem();
            lastDropDay = currentDay;
        }
    }

    private void DropItem()
    {
        if (dropItemPrefab != null && dropPoint != null)
        {
            Instantiate(dropItemPrefab, dropPoint.position, Quaternion.identity);
            Debug.Log($"{gameObject.name} dropped an item on day {TimeManagerInstance.currentDay}");
        }
        else
        {
            Debug.LogWarning("DropItemPrefab or DropPoint not assigned!");
        }
    }

    private TimeManager TimeManagerInstance
    {
        get
        {
            if (_timeManager == null)
                _timeManager = FindObjectOfType<TimeManager>();
            return _timeManager;
        }
    }
    private TimeManager _timeManager;
}
