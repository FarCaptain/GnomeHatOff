using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSpawnManager : MonoBehaviour
{
    [SerializeField] float minTimeBetweenSpawns;
    [SerializeField] float maxTimeBetweenSpawns;
    [SerializeField] int numberOfActiveSpawners = 2;

    [HideInInspector]
    public NewTimer logSpawnTimer;
    private List<GameObject> logPiles = new List<GameObject>();
    void Start()
    {
        PopulateLogPileList();
        InitializeLogSpawnTimer();
        ActivateSpawner();
    }

    // Update is called once per frame
    void Update()
    {
        if (logSpawnTimer.TimerStart == false && logSpawnTimer != null)
        {
            ActivateSpawner();
        }
    }
    private void PopulateLogPileList()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            logPiles.Add(gameObject.transform.GetChild(i).gameObject);
        }
    }
    private void InitializeLogSpawnTimer()
    {
        logSpawnTimer = gameObject.AddComponent<NewTimer>();
    }
   
    public void ActivateSpawner()
	{
        logSpawnTimer.MaxTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
        logSpawnTimer.TimerStart = true;
        int oldNumber = 0;
        for(int i=0;i<numberOfActiveSpawners;i++)
		{
            int randomSpawnerIndex = Random.Range(0, gameObject.transform.childCount);

            while(oldNumber == randomSpawnerIndex)
			{
                randomSpawnerIndex = Random.Range(0, gameObject.transform.childCount);
            }

            logPiles[randomSpawnerIndex].GetComponent<LogSpawner>().enabled = true;

            oldNumber = randomSpawnerIndex;
		}
	}

}
