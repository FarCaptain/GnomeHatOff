using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    public GameObject scoreText0;
    public GameObject scoreText1;
    [SerializeField] GameObject player1BonusMessage;
    [SerializeField] GameObject player2BonusMessage;
    [SerializeField] GameObject pointsPopupDisplay;
    bool player1BonusRecieved = false;
    bool player2BonusRecieved= false;
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
    private Vector3[,] initFireScale = new Vector3[2,4];

    private NewTimer bonusMessageScaleTimer;
    private NewTimer bonusMessageFadeTimer;

    private Color opaque = new Color(1, 1, 1, 1);
    private Color transparent = new Color(1, 1, 1, 0);

    private Vector3 initialMessageScale;
    private Vector3 finalMessageScale;

    private enum fadeStates {FadeIn, FadeOut};
    fadeStates currentFade=fadeStates.FadeIn;

    
    // Start is called before the first frame update
    void Start()
    {
        initialMessageScale = player1BonusMessage.GetComponent<RectTransform>().localScale;
        finalMessageScale = initialMessageScale + Vector3.one;
     
        for(int id = 0; id < 2; id ++)
            for (int i = 0; i < fires[id].transform.childCount; i++)
                initFireScale[id, i] = fires[id].transform.GetChild(i).transform.localScale;
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
        int bonusPoints = 0;
        int currentHats = player.GetComponentInChildren<HatCollecter>().hatCount;

        for (int i = 0; i < hatThresholds.Count; i++)
        {
            // greater than the lowest threshold
            if (currentHats >= hatThresholds[i])
            {
                bonusPoints = bonusModifiers[i];
            }
            else
                break;
        }
        if (bonusPoints > 0)
        {
            bonusMessageScaleTimer = gameObject.AddComponent<NewTimer>();
            bonusMessageFadeTimer = gameObject.AddComponent<NewTimer>();
            bonusMessageScaleTimer.MaxTime = 2f;
            bonusMessageFadeTimer.MaxTime = 1f;
            bonusMessageScaleTimer.TimerStart = true;
            bonusMessageFadeTimer.TimerStart = true;

            if (player.name=="Gnome_0")
			{
                player1BonusMessage.GetComponent<TextMeshProUGUI>().text = "+" + bonusPoints + " BONUS!";
                player1BonusRecieved = true;
            }
            else if(player.name=="Gnome_1")
			{
                player2BonusMessage.GetComponent<TextMeshProUGUI>().text = "+" + bonusPoints + " BONUS!";
                player2BonusRecieved = true;
			}
            
        }
		return bonusPoints;
    }

	private bool DisplayBonusMessage(GameObject bonusMessageDisplayObject, bool bonusActivated)
	{
        if (currentFade == fadeStates.FadeIn)
        {
            bonusMessageDisplayObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(transparent, opaque, bonusMessageFadeTimer.CurrentTime / bonusMessageFadeTimer.MaxTime);
            if (bonusMessageFadeTimer.TimerStart == false)
            {
                currentFade = fadeStates.FadeOut;
                bonusMessageFadeTimer.TimerStart = true;
            }
        }
        else if (currentFade == fadeStates.FadeOut)
        {
            bonusMessageDisplayObject.GetComponent<TextMeshProUGUI>().color = Color.Lerp(opaque, transparent, bonusMessageFadeTimer.CurrentTime / bonusMessageFadeTimer.MaxTime);
            if (bonusMessageFadeTimer.TimerStart == false)
            {
                currentFade = fadeStates.FadeIn;
                bonusMessageDisplayObject.GetComponent<TextMeshProUGUI>().color = transparent;
                bonusActivated = false;
            }
        }
        bonusMessageDisplayObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(initialMessageScale, finalMessageScale, bonusMessageScaleTimer.CurrentTime / bonusMessageScaleTimer.MaxTime);
        return bonusActivated;
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
    
    public void OnTriggerStay(Collider player)
    {
        if(player.tag == "Player")
        {
            HatCollecter hatcollecter = player.GetComponentInChildren<HatCollecter>();
            if (hatcollecter.hatCount > 0 && hatcollecter.hatdrop == true)
            {
                // TODO: Perhpas adjust how we can reference different players (not an issue now since we only have 2)
                int bonusPoints = hatcollecter.hatCount + getBonusPoints(player.gameObject);

                int player_id = 0;
                if (player.name == "Gnome_0")
                {
                    playerScore0 += bonusPoints;
                    scoreText0.GetComponent<TMPro.TextMeshProUGUI>().text = playerScore0.ToString();
                    HatDropIndicator gnome0HatDropIndicator = Instantiate(pointsPopupDisplay, player.transform.position, Quaternion.identity).GetComponent<HatDropIndicator>();
                    gnome0HatDropIndicator.SetPoints(bonusPoints);
                    player_id = 0;
                }
                else
                {
                    playerScore1 += bonusPoints;
                    scoreText1.GetComponent<TMPro.TextMeshProUGUI>().text = playerScore1.ToString();
                    HatDropIndicator gnome1HatDropIndicator = Instantiate(pointsPopupDisplay, player.transform.position, Quaternion.identity).GetComponent<HatDropIndicator>();
                    gnome1HatDropIndicator.SetPoints(bonusPoints);
                    player_id = 1;
                }

                hatcollecter.hatCount = 0;
                
                revertChangesOnFire(player_id);
                magnifyFire(player_id, getFireSize(player.gameObject));
                fires[player_id].Play();

                hatcollecter.hatCount = 0;

                for (int i = 0; i < player.transform.childCount; i++)
                {
                    if (player.transform.GetChild(i).name == "HatPrefab(Clone)")
                        Destroy(player.transform.GetChild(i).gameObject);
                }
                

                //reset the collision on the gnome
                if (hatcollecter.hatTop.transform.position.y != hatcollecter.initHatHeight)
                {
                    Vector3 hatPos = hatcollecter.hatTop.transform.position;
                    hatcollecter.hatTop.transform.position = new Vector3(hatPos.x, hatcollecter.initHatHeight, hatPos.z);

                    hatcollecter.GetComponent<BoxCollider>().size = hatcollecter.initColliderSize;
                    hatcollecter.GetComponent<BoxCollider>().center = hatcollecter.initColliderCenter;
                }
            }
            hatcollecter.hatdrop = false;
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
            fires[fireId].transform.GetChild(i).transform.localScale = initFireScale[fireId, i];
    }

	private void Update()
	{
        if (player1BonusRecieved)
        {
            player1BonusRecieved=DisplayBonusMessage(player1BonusMessage,player1BonusRecieved);
        }
        if (player2BonusRecieved)
		{
            player2BonusRecieved=DisplayBonusMessage(player2BonusMessage,player2BonusRecieved);
        }
	}
}
