using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleUI : MonoBehaviour
{
    public void LoadForest1()
    {
        SceneManager.LoadScene("Map1Level Layout");
    }

    public void LoadForest2()
    {
        SceneManager.LoadScene("Map1Level2 Layout");
    }

    public void LoadIce1()
    {
        SceneManager.LoadScene("IceMapLayout");
    }

    public void LoadIce2()
    {
        SceneManager.LoadScene("IceMap2Layout");
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("Normal Level UI");
    }
}
