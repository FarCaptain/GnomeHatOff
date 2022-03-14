using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelectionManager : MonoBehaviour
{
    [Header("Map Gif Parameters")]
    [SerializeField] RenderTexture[] mapGifs;
    [SerializeField] RawImage mapDisplay;

    [Header("Map Name Parameters")]
    [SerializeField] string[] mapNames;
    [SerializeField] TextMeshProUGUI mapTitle;

    [Header("VideoPlayers")]
    [SerializeField] GameObject[] videoPlayers;

    [Header("Fader Animator")]
    [SerializeField] Animator faderAnimator;

    enum Buttons { None, Play, Back}
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
        TransitionToScene();
    }

    private void TransitionToScene()
	{
        if(faderAnimator.GetCurrentAnimatorStateInfo(0).IsName("FadeOut") && faderAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime>=1f)
		{
            switch (buttonClicked)
            {
                case Buttons.Play:
                    SelectLevel();
                    break;
                case Buttons.Back:
                    SceneManager.LoadScene(1);
                    break;
            }
        }
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
		if(mapTitle.text==mapNames[0])
		{
            SceneManager.LoadScene(3);
		}
        else if(mapTitle.text == mapNames[1])
		{
            SceneManager.LoadScene(4);
        }
        else if(mapTitle.text == mapNames[2])
		{
            SceneManager.LoadScene(5);
        }
	}

    public void PlayButtonClicked()
	{
        faderAnimator.SetBool("isFadingIn", false);
        buttonClicked = Buttons.Play;
    }

    public void BackButtonClicked()
	{
        faderAnimator.SetBool("isFadingIn", false);
        buttonClicked = Buttons.Back;
    }

}
