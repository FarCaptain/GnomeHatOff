using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] Animator[] levelSelectUIAnimators;

    [Header("Map Gif Parameters")]
    [SerializeField] RenderTexture[] mapGifs;
    [SerializeField] RawImage mapDisplay;

    [Header("Map Name Parameters")]
    [SerializeField] string[] mapNames;
    [SerializeField] TextMeshProUGUI mapTitle;

    [Header("Map Description Parameters")]
    [SerializeField] string[] mapDescriptions;
    [SerializeField] TextMeshProUGUI selectedMapDescription;

    [Header("VideoPlayers")]
    [SerializeField] GameObject[] videoPlayers;

    [Header("Fader Animator")]
    [SerializeField] Animator faderAnimator;

    [Header("Map Info Displays")]
    [SerializeField] GameObject[] mapInfoDisplays;
    enum Buttons { None, Play, Back }
    Buttons buttonClicked = Buttons.None;
    private int mapCount;
    private int currentIndex = 0;
    void Start()
    {
        mapCount = mapGifs.Length;
    }


    // Update is called once per frame
    void Update()
    {
        if(IsAnimationStateOver(faderAnimator,"FadeIn"))
		{
            levelSelectUIAnimators[0].SetTrigger("curtainOpen");

        }

        if(IsAnimationStateOver(levelSelectUIAnimators[0],"CurtainOpen"))
		{
            levelSelectUIAnimators[1].SetTrigger("RemoteRiseUp");
            foreach (GameObject display in mapInfoDisplays)
            {
                display.SetActive(true);
            }
        }

        if(IsAnimationStateOver(faderAnimator, "FadeOut"))
        {
            switch (buttonClicked)
            {
                case Buttons.Play:
                    SelectLevel();
                    break;
                case Buttons.Back:
                    SceneManager.LoadScene(0);
                    break;
            }
        }
    }

    private bool IsAnimationStateOver(Animator animator, string animationStateName)
	{
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName) 
               && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
	}

    private void ChangeMapDisplayComponents(int alterationNumber)
    {
        int index = CustomModulo(currentIndex + (alterationNumber), mapCount);
        foreach (GameObject videoPlayer in videoPlayers)
        {
            videoPlayer.SetActive(false);
        }
        mapDisplay.texture = mapGifs[index];
        mapTitle.text = mapNames[index];
        selectedMapDescription.text = mapDescriptions[index];
        videoPlayers[index].SetActive(true);
        currentIndex = currentIndex + (alterationNumber);
    }

    private int CustomModulo(int currentIndex, int totalCount)
    {
        int modulo = 0;
        modulo = currentIndex % totalCount;
        if (modulo < 0)
        {
            while (modulo < 0)
            {
                modulo = modulo + totalCount;
            }
            return modulo;

        }
        else
        {
            return modulo;
        }

    }

    public void IncrementMapDisplay()
    {
        ChangeMapDisplayComponents(1);
    }

    public void DecrementMapDisplay()
    {
        ChangeMapDisplayComponents(-1);
    }

    public void SelectLevel()
    {
        if (mapTitle.text == mapNames[0])
        {
            SceneManager.LoadScene(2);
        }
        else if (mapTitle.text == mapNames[1])
        {
            SceneManager.LoadScene(3);
        }
        else if (mapTitle.text == mapNames[2])
        {
            SceneManager.LoadScene(4);
        }
    }

    public void PlayButtonClicked()
    {
        faderAnimator.SetTrigger("faderFadeOut");
        buttonClicked = Buttons.Play;
    }

    public void BackButtonClicked()
    {
        faderAnimator.SetTrigger("faderFadeOut");
        buttonClicked = Buttons.Back;
    }


}
