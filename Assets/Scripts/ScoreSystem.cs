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

    /// <summary>
    /// Bonus Points are assigned after a player drops off multiple hats, points are determined by passing thresholds
    /// </summary>
    private int getBonusPoints(GameObject player)
    {
        int points = 0;
        int currentHats = player.GetComponentInChildren<HatCollecter>().hatCount;

        for (int i = 0; i < hatThresholds.Count; i++)
        {
            // greater than the lowest threshold
            if (currentHats >= hatThresholds[0])
            {
                // check for highest threshold
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
                // TODO: Perhpas adjust how we can reference different players (not an issue now since we only have 2)
                int bonusPoints = player.GetComponentInChildren<HatCollecter>().hatCount + getBonusPoints(player.gameObject);

                int player_id = 0;
                if (player.name == "Gnome_0")
                {
                    playerScore0 += bonusPoints;
                    scoreText0.GetComponent<TMPro.TextMeshProUGUI>().text = playerScore0.ToString();
                    player_id = 0;
                }
                else
                {
                    playerScore1 += bonusPoints;
                    scoreText1.GetComponent<TMPro.TextMeshProUGUI>().text = playerScore1.ToString();
                    player_id = 1;
                }

                player.GetComponentInChildren<HatCollecter>().hatCount = 0;
                
                revertChangesOnFire(player_id);
                magnifyFire(player_id, getFireSize(player.gameObject));
                fires[player_id].Play();

                player.GetComponentInChildren<HatCollecter>().hatCount = 0;

                for (int i = 0; i < player.transform.childCount; i++)
                {
                    if (player.transform.GetChild(i).name == "HatPrefab(Clone)")
                        Destroy(player.transform.GetChild(i).gameObject);
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
