using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSpawner : MonoBehaviour
{

    [HideInInspector]
    public bool hasLogSpawned = false;

    [SerializeField] GameObject logToSpawn;
    [SerializeField] GameObject spawnLocation;
    private Animator logPileAnimator;
 

    void OnEnable()
	{
        logPileAnimator = gameObject.GetComponent<Animator>();
        logPileAnimator.SetBool("spawnWarningPlaying", true);
        hasLogSpawned = false;
    }


    // Update is called once per frame
    void Update()
    {
		if (IsAnimationIsPlaying(logPileAnimator, "LogRumble") == false && hasLogSpawned == false)
		{
			logPileAnimator.SetBool("spawnWarningPlaying", false);
            SpawnLog();
		}
	}

    public void SpawnLog()
	{
        hasLogSpawned = true;
        GameObject logSpawned = Instantiate(logToSpawn, spawnLocation.transform.position, logToSpawn.transform.rotation);
        logSpawned.GetComponent<Log>().GetOriginLog(spawnLocation);
        spawnLocation.SetActive(false);
        gameObject.GetComponent<LogSpawner>().enabled= false;
    }

    bool IsAnimationIsPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
}
