using System.Collections;
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

    public List<float> fireSizeModifiers;

    static public int hatCount0 = 0;
    static public int hatCount1 = 0;
    
    static public int playerScore0 = 0;
    static public int playerScore1 = 0;

    public ParticleSystem[] fires = new ParticleSystem[2];


    private Vector3[] initFireScale = new Vector3[8];

    // Start is called before the first frame update
    void Start()
    {
        for(int id = 0; id < 2; id ++)
            for (int i = 0; i < fires[id].transform.childCount; i++)
                initFireScale[id*4 + i] = fires[id].transform.GetChild(i).transform.localScale;
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

    private float getFireSize(GameObject player)
    {
        float size = 1;
        int currentHats = player.GetComponentInChildren<HatCollecter>().hatCount;

        for (int i = 0; i < hatThresholds.Count; i++)
        {
            if (currentHats >= hatThresholds[0])
            {
                if (currentHats >= hatThresholds[i])
                    size = fireSizeModifiers[i];
            }
            else
                break;
        }
        return size;
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

                    revertChangesOnFire(0);
                    magnifyFire(0, getFireSize(player.gameObject));
                    fires[0].Play();
                }
                else
                {
                    int bonusPoints = hatCount1 + getBonusPoints(player.gameObject);
                    playerScore1 += bonusPoints;
                    hatCount1 = 0;
                    scoreText1.GetComponent<TMPro.TextMeshProUGUI>().text = playerScore1.ToString();

                    revertChangesOnFire(1);
                    magnifyFire(1, getFireSize(player.gameObject));
                    fires[1].Play();
                    // revert the change in stop call back
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

    private void magnifyFire(int fireId, float bonus)
    {
        for(int i = 0; i < fires[fireId].transform.childCount; i ++)
            fires[fireId].transform.GetChild(i).transform.localScale *= bonus;
    }

    private void revertChangesOnFire(int fireId)
    {
        for (int i = 0; i < fires[fireId].transform.childCount; i++)
            fires[fireId].transform.GetChild(i).transform.localScale = initFireScale[fireId * 4 + i];
    }
}
