using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [Header("Button Event")]
    [SerializeField] UnityEvent OnClicked;
    [SerializeField] UnityEvent OnExit;
    [SerializeField] public bool playerOver;

    [Header("Selection Confirmation Parameters")]
    [SerializeField] Image selectionConfirmation;
    [SerializeField] ButtonAccessibilityStates buttonAccessibilityLevel;
    [SerializeField] GnomeColors acceptedGnomeColor;
    [SerializeField] float completitionMultiplier;

    NewTimer selectionTimer;
    public List<UISelector> UISelectors = new List<UISelector>();
    private bool optionSelected=false;
    private bool exitFunctionInvoked = true;
    private enum ButtonAccessibilityStates { multiPlayerAccessible, singlePlayerAccessible, gnomeSpecificSinglePlayerAccessible };
    private enum GnomeColors { Blue, Yellow, Red, Purple, None };

    void Start()
    {
        selectionTimer = gameObject.AddComponent<NewTimer>();
        selectionTimer.MaxTime = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerOver || UISelectors.Count>0)
		{
            if (buttonAccessibilityLevel == ButtonAccessibilityStates.multiPlayerAccessible)
            {
                selectionTimer.ReverseTimer = false;
                if (!selectionTimer.TimerStart)
                {
                    selectionTimer.TimerStart = true;
                }
                selectionConfirmation.fillAmount = selectionTimer.TimerCompletionRate + (UISelectors.Count * completitionMultiplier);
            }
            else if(buttonAccessibilityLevel == ButtonAccessibilityStates.singlePlayerAccessible)
			{
                selectionTimer.ReverseTimer = false;
                if (!selectionTimer.TimerStart)
                {
                    selectionTimer.TimerStart = true;
                }
                selectionConfirmation.fillAmount = selectionTimer.TimerCompletionRate;
			}
            else if (buttonAccessibilityLevel == ButtonAccessibilityStates.gnomeSpecificSinglePlayerAccessible)
            {
                if (UISelectors[0].gameObject.name.ToLower().Contains(acceptedGnomeColor.ToString().ToLower()) && !optionSelected)
				{
                    selectionTimer.ReverseTimer = false;
                    if (!selectionTimer.TimerStart)
                    {
                        selectionTimer.TimerStart = true;
                    }
                    selectionConfirmation.fillAmount = selectionTimer.TimerCompletionRate + completitionMultiplier;
                }
            }
        }
        else
		{
            if(exitFunctionInvoked==false)
			{
                OnExit?.Invoke();
                exitFunctionInvoked = true;
            }
            selectionTimer.ReverseTimer = true;
            selectionConfirmation.fillAmount = selectionTimer.TimerCompletionRate;
        }

        if(selectionConfirmation.fillAmount>=1 && !optionSelected)
		{
            OnClicked.Invoke();
            optionSelected = true;
            exitFunctionInvoked = false;
        }

        if(selectionConfirmation.fillAmount<1)
		{
            optionSelected = false;
		}

        
    }
}
