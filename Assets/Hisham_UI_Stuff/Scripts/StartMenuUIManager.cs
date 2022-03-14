using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuUIManager : MonoBehaviour
{
    [SerializeField] float buttonInitialXPos;
    [SerializeField] float buttonFinalXPos;
    [SerializeField] GameObject fader;
    [SerializeField] Animator responsiveModelAnimator;
    void Start()
    {
        fader.GetComponent<Animator>().SetBool("isFadingIn", true);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void ButtonHoverTransform()
	{
        GameObject buttonHoveredOver = EventSystem.current.currentSelectedGameObject;
		if (buttonHoveredOver.GetComponent<Button>())
		{
            buttonHoveredOver.GetComponent<RectTransform>().localPosition = new Vector2(buttonFinalXPos, buttonHoveredOver.GetComponent<RectTransform>().localPosition.y);
		}
	}

    public void ButtonIdleTransform()
	{
        GameObject buttonHoveredOver = EventSystem.current.currentSelectedGameObject;
        if (buttonHoveredOver.GetComponent<Button>())
        {
            buttonHoveredOver.GetComponent<RectTransform>().localPosition = new Vector2(buttonInitialXPos, buttonHoveredOver.GetComponent<RectTransform>().localPosition.y);
        }
    }

    public void GoToLevelSelect()
	{
        SceneManager.LoadScene(2);
	}


}
