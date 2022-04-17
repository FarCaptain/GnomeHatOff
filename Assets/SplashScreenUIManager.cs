using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SplashScreenUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gnomeReadyText;
    [SerializeField] LevelManager levelManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
	{
        gnomeReadyText.text = "Gnomes Ready: " + levelManager.lockedInPlayers;
    }
}
