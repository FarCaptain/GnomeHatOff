using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement ;
using TMPro;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuickStartButtonClicked()
	{
        levelManager.LoadScene(1);

    }
    IEnumerator WaitAndLoad(int sceneIndex)
	{
        //confirmationText.text = "LEVEL SELECT LOADING";
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(1);
	}
}
