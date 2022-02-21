using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevModeDebug : MonoBehaviour
{
	[SerializeField] int numberOfHatsToSpawn = 5;

    HatCollecter[] hatCollecters;
	HatSpawning hatSpawner;
	Timer timer;

	private void Awake()
	{
		if(!Debug.isDebugBuild)
		{
			Destroy(gameObject);
		}
	}
	void Start()
    {
        hatCollecters = FindObjectsOfType<HatCollecter>();
		timer = FindObjectOfType<Timer>();
		hatSpawner = FindObjectOfType<HatSpawning>();

	}

    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild)
		{
			UseMKeyForDrop();
			PauseAndUnpauseTime();
			SpawnAlotOfHats();
			//TestSomeAudio();
		}
	}

	private static void TestSomeAudio()
	{
		if (Input.GetKeyDown(KeyCode.O))
		{
			AudioManager.PlayGeneralGameAudioClip(GameGeneralAudioStates.HatRushBegin);
		}
		if (Input.GetKeyDown(KeyCode.I))
		{
			AudioManager.PlayGeneralGameAudioClip(GameGeneralAudioStates.RoundEnd);
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

	private void SpawnAlotOfHats()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			for(int i=0;i<numberOfHatsToSpawn;i++)
			{
				hatSpawner.SpawnHat();
			}
		}
	}
}
