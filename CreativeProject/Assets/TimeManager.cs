// Built using AI for assistance
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("Time Display")]
    public TextMeshProUGUI timeText;    // Drag your TMP Time Text here
    public TextMeshProUGUI dayText;     // Drag your TMP Day Text here

    [Header("Time Settings")]
    public int startHour = 8;           // Start of day (8 AM)
    public int endHour = 24;            // End of day (12 AM)
    public float realSecondsPerGameHour = 18.75f; // 5 minutes per full day

    private int currentHour;
    private float hourTimer;

    [Header("Day Settings")]
    public int currentDay = 1;

    // Optional event for other systems (e.g., crop growth)
    public delegate void OnNewDay();
    public static event OnNewDay NewDayStarted;

    void Start()
    {
        currentHour = startHour;
        hourTimer = 0f;
        UpdateTimeUI();
        UpdateDayUI();
    }

    void Update()
    {
        hourTimer += Time.deltaTime;

        if (hourTimer >= realSecondsPerGameHour)
        {
            hourTimer -= realSecondsPerGameHour;
            AdvanceHour();
        }
    }

    void AdvanceHour()
    {
        currentHour++;

        if (currentHour >= endHour)
        {
            PassOut(); // Player didn’t sleep — force next day
            return;
        }

        UpdateTimeUI();
    }

    void UpdateTimeUI()
    {
        string suffix = (currentHour >= 12 && currentHour < 24) ? "PM" : "AM";
        int displayHour = currentHour % 12;
        if (displayHour == 0) displayHour = 12;

        timeText.text = displayHour.ToString("00") + ":00 " + suffix;
    }

    void UpdateDayUI()
    {
        if (dayText != null)
            dayText.text = "Day " + currentDay;
    }

    public void Sleep()
    {
        Debug.Log("You slept. New day begins.");
        StartNewDay();
    }

    void PassOut()
    {
        Debug.Log("You passed out! Waking up tomorrow morning...");
        // TODO: Show message popup or screen fade here
        StartNewDay();
    }

    public void StartNewDay()
    {
        currentDay++;
        currentHour = startHour;
        hourTimer = 0f;

        UpdateTimeUI();
        UpdateDayUI();

        NewDayStarted?.Invoke(); // Let crops, animals, NPCs know it's a new day
    }
}
