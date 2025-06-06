using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

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

    public CanvasGroup fadePanel; // Assign in Inspector

    private bool isSleeping = false;

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
        if (isSleeping) return;

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
        StartCoroutine(SleepRoutine());
    }

    IEnumerator SleepRoutine()
    {
        isSleeping = true;

        // Fade to black
        yield return StartCoroutine(FadeScreen(1f));

        // Advance time and day
        currentDay++;
        currentHour = startHour;
        hourTimer = 0f;
        UpdateTimeUI();
        UpdateDayUI();
        NewDayStarted?.Invoke();

        // Simulate showing daily summary (later you'll build earnings UI here)
        Debug.Log("Show daily summary...");
        yield return new WaitForSeconds(2f); // Placeholder for summary

        // Fade back in
        yield return StartCoroutine(FadeScreen(0f));

        isSleeping = false;
    }

    IEnumerator FadeScreen(float targetAlpha)
    {
        float duration = 1f;
        float startAlpha = fadePanel.alpha;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            yield return null;
        }

        fadePanel.alpha = targetAlpha;
    }

    void PassOut()
    {
        Debug.Log("You passed out! Waking up tomorrow...");
        Sleep();
    }
}
