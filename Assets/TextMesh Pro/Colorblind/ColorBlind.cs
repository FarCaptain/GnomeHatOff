using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ColorBlind : MonoBehaviour
{
    public Toggle toggleNone;
    public Toggle toggleColorblind;

    void Start()
    {
        if (PlayerPrefs.GetInt("ToggleBool") == 1)
        {
            toggleNone.isOn = true;
        }
        else
        {
            toggleNone.isOn = false;
        }

        if (PlayerPrefs.GetInt("ToggleBool2") == 1)
        {
            toggleColorblind.isOn = true;
        }
        else
        {
            toggleColorblind.isOn = false;
        }
    }

    
    void Update()
    {
        if (toggleNone.isOn == true)
        {
            PlayerPrefs.SetInt("ToggleBool", 1);
        }
        else
        {
            PlayerPrefs.SetInt("ToggleBool", 0);
        }

        if (toggleColorblind.isOn == true)
        {
            PlayerPrefs.SetInt("ToggleBool2", 1);
        }
        else
        {
            PlayerPrefs.SetInt("ToggleBool2", 0);
        }
    }
}
