using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [HideInInspector] public int lockedInPlayers = 0;
    [SerializeField] Animator faderAnimator;
    [SerializeField] UnityEvent OnPlayersLockedIn;
    [SerializeField] UnityEvent OnPlayersNotLockedIn;
    bool allLockedIn = false;

    private int indexOfLevelToLoad = -1;
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

        if(IsAnimationStateOver(faderAnimator,"FadeOut",1f))
		{
            SceneManager.LoadScene(indexOfLevelToLoad);
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
        indexOfLevelToLoad = levelIndex;
        faderAnimator.SetTrigger("faderFadeOut");
    }

    private bool IsAnimationStateOver(Animator animator, string animationStateName, float time)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName)
               && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= time;
    }
}
