﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public GameObject scoreText0;
    public GameObject scoreText1;
    public GameObject player0;
    public GameObject player1;
    public GameObject winPanel;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI winnerScoreText;

    public List<int> hatThresholds;

    [Header("Size must match HatThresholds")]
    public List<int> bonusModifiers;

    static public int hatCount0 = 0;
    static public int hatCount1 = 0;
    
    static public int playerScore0 = 0;
    static public int playerScore1 = 0;

    public ParticleSystem fire0;
    public ParticleSystem fire1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void displayWinner()
    {
        winPanel.SetActive(true);
        if (playerScore0 > playerScore1)
        {
            winText.text = "Player 1 Wins!!!";
            winnerScoreText.text = "Score: " + playerScore0;
        }
        else if (playerScore0 == playerScore1)
        {
            winText.text = "DRAW";
            winnerScoreText.text = "";
        }
        else
        {
            winText.text = "Player 2 Wins!!!";
            winnerScoreText.text = "Score: " + playerScore1;
        }
    }

    private int getBonusPoints(GameObject player)
    {
        int points = 0;
        int currentHats = player.GetComponentInChildren<HatCollecter>().hatCount;

        for (int i = 0; i < hatThresholds.Count; i++)
        {
            if (currentHats >= hatThresholds[0])
            {
                if (currentHats >= hatThresholds[i])
                    points = bonusModifiers[i];
            }
            else
                break;
        }
        return points;
    }

    public void OnTriggerEnter(Collider player)
    {
        if(player.tag == "Player")
        {
            if (player.GetComponentInChildren<HatCollecter>().hatCount > 0)
            {
                if (player.name == "Gnome")
                {
                    int bonusPoints = hatCount0 + getBonusPoints(player.gameObject);
                    playerScore0 += bonusPoints;
                    hatCount0 = 0;
                    scoreText0.GetComponent<TMPro.TextMeshProUGUI>().text = playerScore0.ToString();
                    fire0.Play();
                }
                else
                {
                    int bonusPoints = hatCount1 + getBonusPoints(player.gameObject);
                    playerScore1 += bonusPoints;
                    hatCount1 = 0;
                    scoreText1.GetComponent<TMPro.TextMeshProUGUI>().text = playerScore1.ToString();
                    fire1.Play();
                }

                player.GetComponentInChildren<HatCollecter>().hatCount = 0;

                for (int i = 0; i < player.transform.childCount; i++)
                {
                    if (player.transform.GetChild(i).name == "HatPrefab(Clone)")
                    {
                        print("YYYYY");
                        print("ddd" + playerScore0);
                        Destroy(player.transform.GetChild(i).gameObject);
                    }

                }
            }
        }
    }
}
