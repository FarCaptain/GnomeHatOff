using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Quit)");
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Map1Level Layout");

    }
}
