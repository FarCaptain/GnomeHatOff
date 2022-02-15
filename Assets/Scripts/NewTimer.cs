﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTimer : MonoBehaviour
{
    private float maxTime;
    private float currentTime=0f;
    private bool isTimerRunning;
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

    public bool IsTimerRunning
	{
        set
		{
            IsTimerRunning = value;
		}
        get
		{
            return isTimerRunning;
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
            currentTime += Time.deltaTime;
            timerCompletionRate = Mathf.Sin(currentTime / maxTime);
            if(currentTime>=maxTime)
			{
                timerStart = false;
                currentTime = 0;
			}
		}
    }
}