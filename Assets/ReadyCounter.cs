using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ReadyCounter : MonoBehaviour
{

    [SerializeField] CounterStates currentCounterState;
    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] UnityEvent OnCounterFinished;
    enum CounterStates { Started, Stopped, Finished }
    private int maxTime = 4;
    private NewTimer counterIntervalTimer;
    void Start()
    {
        counterIntervalTimer = gameObject.AddComponent<NewTimer>();
        counterIntervalTimer.MaxTime = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentCounterState==CounterStates.Started)
		{
            if (maxTime <= 0)
            {
                Invoke("WaitBeforeFinishing", 1f);
            }
            if (!counterIntervalTimer.TimerStart)
			{
                maxTime = Mathf.Clamp(maxTime-1, 0, 3);
                counterIntervalTimer.TimerStart = true;
                counterText.text = maxTime.ToString();
                counterText.color = Color.yellow;
            }

        
        }
        else if (currentCounterState == CounterStates.Stopped)
		{
            maxTime = 5;
            counterText.text = "READY UP";
            counterText.color = Color.red;
        }
        else if (currentCounterState == CounterStates.Finished)
        {
            counterText.text = "PLAY";
            counterText.color = Color.green;
            OnCounterFinished?.Invoke();
        }
    }

    private void WaitBeforeFinishing()
	{
        currentCounterState = CounterStates.Finished;
    }

    public void StartReadyCounter()
	{
        currentCounterState = CounterStates.Started;
    }

    public void StopReadyCounter()
	{
        currentCounterState = CounterStates.Stopped;
	}
}
