using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [Header("Button Event")]
    [SerializeField] UnityEvent OnClicked;
    [SerializeField] public bool playerOver;

    [Header("Selection Confirmation Parameters")]
    [SerializeField] Image selectionConfirmation;
    [SerializeField] bool multiPlayerAccessible;
    [SerializeField] float completitionMultiplier;

    NewTimer selectionTimer;
    public List<UISelector> UISelectors = new List<UISelector>();
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
            selectionTimer.ReverseTimer = false;
            if (!selectionTimer.TimerStart)
			{
                selectionTimer.TimerStart=true;
			}

            if (multiPlayerAccessible)
            {
                selectionConfirmation.fillAmount = selectionTimer.TimerCompletionRate + (UISelectors.Count * completitionMultiplier);
            }
            else
			{
                selectionConfirmation.fillAmount = selectionTimer.TimerCompletionRate;
			}
        }
        else
		{
            selectionTimer.ReverseTimer = true;
            selectionConfirmation.fillAmount = selectionTimer.TimerCompletionRate;
        }

        if(selectionConfirmation.fillAmount>=1)
		{
            OnClicked.Invoke();
		}

        
    }
}
