using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITestManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI confirmationText;

    [SerializeField] TextMeshProUGUI[] buttonTexts;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Button1Event()
	{
        confirmationText.text = buttonTexts[0].text;
	}
    public void Button2Event()
    {
        confirmationText.text = buttonTexts[1].text;
    }
    public void Button3Event()
    {
        confirmationText.text = buttonTexts[2].text;
    }
}
