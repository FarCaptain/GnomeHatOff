using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Functions as a countdown timer
/// </summary>
public class Timer : MonoBehaviour
{
    private Scene currentScene;
    private TextMeshProUGUI timerText;
    public static float timeRemaining = 60;      // measured in seconds
    private bool isTimerRunning = false;
    [HideInInspector] public bool timerPaused = false;

    public ScoreSystem scoreSystem;

    void Start()
    {
        AudioManager.PlayGeneralGameAudioClip(GameGeneralAudioStates.RoundBegin);
        currentScene = SceneManager.GetActiveScene();
        timerText = gameObject.GetComponent<TextMeshProUGUI>();
        isTimerRunning = true;
    }

    public void restartGame()
    {
        Debug.Log("Reload Game");
        SceneManager.LoadScene(currentScene.name);
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
            if (timeRemaining > 0 && timerPaused == false)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else if(timeRemaining<=0)
            {
                scoreSystem.displayWinner();
                timeRemaining = 0;
                isTimerRunning = false;
            }
        }
    }
}
