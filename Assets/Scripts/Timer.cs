using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float startMinutes = 1;  // Set this in the Inspector to define how many minutes to count down from
    public float timeRemaining;  // Variable to hold the remaining time in seconds
    public TextMeshPro timerText;  // Link this to a TextMeshPro element in the inspector

    void Start()
    {
        // Initialize the timer with the total time in seconds
        timeRemaining = startMinutes * 60;  // Convert minutes to seconds
        UpdateTimerUI();  // Update the UI at the start
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            // Decrease time remaining by the time that has passed since last frame
            timeRemaining -= Time.deltaTime;

            // Ensure timeRemaining doesn't go below zero
            if (timeRemaining < 0)
            {
                timeRemaining = 0;
            }

            // Update the UI
            UpdateTimerUI();
        }
        else
        {
            // Timer reached zero
            TimerFinished();  // Call a method when the timer finishes
        }
    }

    void UpdateTimerUI()
    {
        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        // Format the time as MM:SS
        timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    void TimerFinished()
    {
        Debug.Log("Timer Finished!");
        // Add any logic for when the timer ends, like ending the game
    }
}
