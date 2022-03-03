using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTimer : MonoBehaviour
{
    private float maxTime;
    private float currentTime=0f;
    private bool timerRunning=false;
    private bool timerStart;
    private float timerCompletionRate;

    public float MaxTime
	{
		get
		{
            return maxTime;
		}
        set
		{
            maxTime = value;
		}
	}

    public float CurrentTime
    {
        get
        {
            return currentTime;
        }
		set
		{
            currentTime = value;
		}
    }

    public bool TimerStart
	{
		set
		{
            timerStart = value;
		}
        get
		{
            return timerStart;
		}
	}

    public bool TimerRunning
	{
        set
		{
            timerRunning = value;
		}
        get
		{
            return timerRunning;
		}
	}

    public float TimerCompletionRate
	{
		get
		{
            return timerCompletionRate;
		}
	}

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timerStart)
		{
            timerRunning = true;
            currentTime += Time.deltaTime;
            timerCompletionRate = Mathf.Clamp01(currentTime / maxTime);
            if(currentTime>=maxTime)
			{
                timerRunning = false;
                timerStart = false;
                currentTime = 0;
                timerCompletionRate = 0;
			}
		}
    }

    public void ResetTimer()
	{
        currentTime = 0f;
	}
}
