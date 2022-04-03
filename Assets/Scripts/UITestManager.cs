using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement ;
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
        StartCoroutine(WaitAndLoad(1));
	}
    IEnumerator WaitAndLoad(int sceneIndex)
	{
        confirmationText.text = "LEVEL SELECT LOADING";
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(1);
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
