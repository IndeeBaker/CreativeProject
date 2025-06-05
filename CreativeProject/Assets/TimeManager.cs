using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("Time Display")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dayText;

    [Header("Time Settings")]
    public int startHour = 8;
    public int endHour = 24;
    public float realSecondsPerGameHour = 18.75f;

    private int currentHour;
    private float hourTimer;

    [Header("Day Settings")]
    public int currentDay = 1;

    public delegate void OnNewDay();
    public static event OnNewDay NewDayStarted;

    private bool isSleeping = false;  // Track if player is sleeping

    void Start()
    {
        currentHour = startHour;
        hourTimer = 0f;
        UpdateTimeUI();
        UpdateDayUI();
    }

    void Update()
    {
        if (isSleeping) return; // Pause time while sleeping

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
            PassOut();
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
        if (isSleeping) return;

        isSleeping = true;
        Debug.Log("Sleeping... Fast forwarding to 8 AM next day");

        // Fast forward to 8 AM next day
        currentDay++;
        currentHour = startHour;
        hourTimer = 0f;

        UpdateTimeUI();
        UpdateDayUI();

        // Notify other systems of new day if needed
        NewDayStarted?.Invoke();

        // End sleep after short delay (simulate sleep duration)
        StartCoroutine(EndSleepAfterDelay(2f));
    }

    System.Collections.IEnumerator EndSleepAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSleeping = false;
        Debug.Log("Woke up! Good morning!");
    }

    void PassOut()
    {
        Debug.Log("You passed out! Waking up tomorrow morning...");
        // You can handle player passing out here if they don't sleep
        // For example, reset time and day or force sleep
        Sleep();
    }
}
