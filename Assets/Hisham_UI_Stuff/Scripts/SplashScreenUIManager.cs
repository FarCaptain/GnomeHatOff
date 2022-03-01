using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SplashScreenUIManager : MonoBehaviour
{
    private float currentTime = 0f;
    private bool gameStarted = false;
    enum StartTextGlowStates { Forward, Reverse }
    StartTextGlowStates startTextCurrentGlowState = StartTextGlowStates.Forward;

    [Header("Background Scroll Parameters")]
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;
    [SerializeField] private RawImage backgroundImage;

    [Header("Start Text Glow Parameters")]
    [SerializeField] float maxTime = 1f;
    [SerializeField] GameObject startText;
  
    [Header("UI Animators")]
    [SerializeField] Animator titleTextAnimator;
    Animator startTextAnimator;
    Animator faderAnimator;

    [SerializeField] GameObject fader;
    void Start()
    {
        faderAnimator = fader.GetComponent<Animator>();
        startTextAnimator = startText.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
	{
        if(gameStarted==true && faderAnimator.GetCurrentAnimatorStateInfo(0).IsName("FadeOut") && faderAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime>=1f)
		{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}

        if(Input.GetKeyDown(KeyCode.Return) && startText.activeSelf==true && gameStarted==false)
		{
            startTextAnimator.SetTrigger("gameStarted");
        }

        ActivateStartText();
		StartTextGlowOscillate();
		ScrollBackgroundInfinitely();
        ScaleDownTitleText();

    }

	private void ActivateStartText()
	{
		if (titleTextAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && titleTextAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f && startText.activeSelf==false)
		{
			startText.SetActive(true);
		}
	}

	private void ScrollBackgroundInfinitely()
	{
		backgroundImage.uvRect = new Rect(backgroundImage.uvRect.position + new Vector2(xSpeed, ySpeed) * Time.deltaTime, backgroundImage.uvRect.size);
	}



    private void StartTextGlowOscillate()
	{
		if (startTextCurrentGlowState == StartTextGlowStates.Forward)
		{
			currentTime+=Time.deltaTime;
		}
        else if(startTextCurrentGlowState == StartTextGlowStates.Reverse)
		{
            currentTime -= Time.deltaTime;
        }

        startText.GetComponent<TextMeshProUGUI>().fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowOuter, currentTime/maxTime);

        if (currentTime >= maxTime)
        {
            startTextCurrentGlowState = StartTextGlowStates.Reverse;
        }
        else if (currentTime <= 0)
        {
            startTextCurrentGlowState = StartTextGlowStates.Forward;
        }
    }

    private void ScaleDownTitleText()
	{
        if(startTextAnimator.GetCurrentAnimatorStateInfo(0).IsName("Exit") && startTextAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime>=1f)
		{
            titleTextAnimator.SetTrigger("gameStarted");
            ActivateFader();
        }
	}

    private void ActivateFader()
	{
        if (titleTextAnimator.GetCurrentAnimatorStateInfo(0).IsName("Exit") && titleTextAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && fader.activeSelf==false)
        {
            fader.SetActive(true);
            faderAnimator.SetBool("isFadingIn", false);
            gameStarted = true;
        }
        
	}
}
