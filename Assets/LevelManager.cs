using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [HideInInspector] public int lockedInPlayers = 0;
    [SerializeField] UnityEvent OnPlayersLockedIn;
    [SerializeField] UnityEvent OnPlayersNotLockedIn;
    bool allLockedIn = false;

    private int numberOfPlayers;
    void Start()
    {
        numberOfPlayers = FindObjectsOfType<Player>().Length;
    }

    // Update is called once per frame
    void Update()
    {
        if(lockedInPlayers==numberOfPlayers && !allLockedIn)
		{
            OnPlayersLockedIn?.Invoke();
            allLockedIn = true;
        }
        else
		{
            allLockedIn = false;
        }
    }

    public void IncrementLockedInPlayers()
	{
        lockedInPlayers = Mathf.Clamp(lockedInPlayers + 1, 0, 4);

    }

    public void DecrementLockedInPlayers()
    {
        lockedInPlayers = Mathf.Clamp(lockedInPlayers - 1, 0, 4);
        OnPlayersNotLockedIn?.Invoke();
    }

    public void LoadScene(int levelIndex)
	{
        SceneManager.LoadScene(levelIndex);
	}
}
