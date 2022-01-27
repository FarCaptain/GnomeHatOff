using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Functions as a countdown timer
/// </summary>
public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private float timeRemaining = 120;      // measured in seconds
    private bool isTimerRunning = false;

    void Start()
    {
        timerText = gameObject.GetComponent<TextMeshProUGUI>();
        isTimerRunning = true;
    }

    /// <summary>
    /// Converts time in seconds to Minute:Seconds
    /// </summary>
    private void DisplayTime(float time)
    {
        time += 1;      // offset for flooring
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Debuglog - Game End");
                timeRemaining = 0;
                isTimerRunning = false;
            }
        }
    }
}
