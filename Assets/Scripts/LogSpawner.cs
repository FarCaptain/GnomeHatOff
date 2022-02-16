using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSpawner : MonoBehaviour
{
    [SerializeField] float minTimeBetweenSpawns;
    [SerializeField] float maxTimeBetweenSpawns;
    [HideInInspector]
    public bool logAlive = false;

    [SerializeField] GameObject logToSpawn;
    [HideInInspector]
    public NewTimer logSpawnTimer;

    void Start()
	{
        InitializeLogSpawnTimer();
        SpawnLog();
    }

	private void InitializeLogSpawnTimer()
	{
		logSpawnTimer = gameObject.AddComponent<NewTimer>();
	}

	// Update is called once per frame
	void Update()
    {
        if (logSpawnTimer.TimerStart == false && logAlive == false && logSpawnTimer!=null)
        {
            SpawnLog();
        }
    }

    public void SpawnLog()
	{
        logAlive = true;
        logSpawnTimer.MaxTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
        GameObject logSpawned = Instantiate(logToSpawn, transform.position, logToSpawn.transform.rotation);
        logSpawned.transform.SetParent(transform);
	}
}
