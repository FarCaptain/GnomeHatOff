using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuUIManager : MonoBehaviour
{

    [SerializeField] Animator faderAnimator;
    [SerializeField] Animator responsiveModelAnimator;
    [SerializeField] Animator platformAnimator;

    [Header("Scrolling background Parameters")]
    [SerializeField] RawImage backgroundImage;
    [SerializeField] float xSpeed, ySpeed;

    enum Buttons {None, QuickStart, LevelSelect, Options, Quit};
    Buttons buttonClicked = Buttons.None;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ScrollBackgroundInfinitely();
        ActivateFaderToGame();
        TransitionToScene();
    }

    private void ScrollBackgroundInfinitely()
    {
        backgroundImage.uvRect = new Rect(backgroundImage.uvRect.position + new Vector2(xSpeed, ySpeed) * Time.deltaTime, backgroundImage.uvRect.size);
    }

    private void ActivateFaderToGame()
    {
        if(responsiveModelAnimator.GetCurrentAnimatorStateInfo(0).IsName("OnClick(Launched)") && responsiveModelAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime>=1f)
		{
            faderAnimator.SetBool("isFadingIn", false);
            platformAnimator.SetTrigger("platformFadeOut");
            buttonClicked = Buttons.QuickStart;
        }
    }

    private void TransitionToScene()
	{
        if (faderAnimator.GetCurrentAnimatorStateInfo(0).IsName("FadeOut") && faderAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
			switch (buttonClicked)
			{
                case Buttons.QuickStart:
                    SceneManager.LoadScene(3);
                    break;
                case Buttons.LevelSelect:
                    SceneManager.LoadScene(2);
                    break;
			}
        }
    }

    public void ActivateOnHoverAnimation()
	{
        responsiveModelAnimator.SetBool("hoverOverStart", true);
    }

    public void ActivateOffHoverAnimation()
    {
        responsiveModelAnimator.SetBool("hoverOverStart", false);
    }

    public void ActivateOnClickAnimation()
	{
        responsiveModelAnimator.SetTrigger("startButtonClicked");
	}

    public void GoToLevelSelect()
	{
        faderAnimator.SetBool("isFadingIn", false);
        platformAnimator.SetTrigger("platformFadeOut");
        responsiveModelAnimator.SetTrigger("modelFadeOut");
        buttonClicked = Buttons.LevelSelect;
    }

    public void QuitGame()
	{
        Application.Quit();
        if (Debug.isDebugBuild)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
	}


}
