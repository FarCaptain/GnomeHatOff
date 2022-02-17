using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevModeDebug : MonoBehaviour
{
    HatCollecter[] hatCollecters;
	Timer timer;
    void Start()
    {
        hatCollecters = FindObjectsOfType<HatCollecter>();
		timer = FindObjectOfType<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild)
		{
			UseMKeyForDrop();
			PauseAndUnpauseTime();
		}
	}

	private void PauseAndUnpauseTime()
	{
		if (Input.GetKeyDown(KeyCode.X))
		{
			if (timer.timerPaused == false)
			{
				timer.timerPaused = true;
			}
			else
			{
				timer.timerPaused = false;
			}
		}
	}

	private void UseMKeyForDrop()
	{
		if (Input.GetKeyDown(KeyCode.M))
		{
			for (int i = 0; i < hatCollecters.Length; i++)
			{
				hatCollecters[i].hatdrop = true;
			}
		}
	}
}
