using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [Header("Points Display Objects")]
    [SerializeField] GameObject player1HatDropDisplay;
    [SerializeField] GameObject player2HatDropDisplay;
    [SerializeField] GameObject bonusPointsPopupDisplay;

    [Header("Player Specific Points Colors")]
    [SerializeField] Color player1HatDropDisplayTextColor;
    [SerializeField] Color player1HatDropDisplayTextTransparent;
    [SerializeField] Color player2HatDropDisplayTextColor;
    [SerializeField] Color player2HatDropDisplayTextTransparent;

    [Header("")]
    [SerializeField] float simultaneousDisplayHeightOffsetValue = 1f;
    public GameObject scoreText0;
    public GameObject scoreText1;
    bool player1HatDropped = false;
    bool player2HatDropped = false;
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

    private NewTimer hatDropDisplayScaleTimer;
    private NewTimer hatDropDisplayFadeTimer;


    private Vector3 initialMessageScale;
    private Vector3 finalMessageScale;

    private enum fadeStates {FadeIn, FadeOut};
    fadeStates currentFade=fadeStates.FadeIn;

    private BonusPointsIndicator[] bonusPointIndicatorsInScene;
    private bool simultaneousBonusesBeingDisplayed = false;
    // Start is called before the first frame update
    void Start()
	{
		initialMessageScale = player1HatDropDisplay.GetComponent<RectTransform>().localScale;
		finalMessageScale = initialMessageScale + Vector3.one*1.5f;

		for (int id = 0; id < 2; id++)
			for (int i = 0; i < fires[id].transform.childCount; i++)
				initFireScale[id, i] = fires[id].transform.GetChild(i).transform.localScale;

		InitializeFadeAndScaleTimer();
	}

	private void InitializeFadeAndScaleTimer()
	{
		hatDropDisplayScaleTimer = gameObject.AddComponent<NewTimer>();
		hatDropDisplayFadeTimer = gameObject.AddComponent<NewTimer>();
		hatDropDisplayScaleTimer.MaxTime = 2f;
		hatDropDisplayFadeTimer.MaxTime = 1f;
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
            BonusPointsIndicator bonusPointsIndicatorObject = Instantiate(bonusPointsPopupDisplay, player.transform.position, Quaternion.identity).GetComponent<BonusPointsIndicator>();
            if(simultaneousBonusesBeingDisplayed==false)
            {
                int numberOfSameSpawnTimeDisplays = 0;
                simultaneousBonusesBeingDisplayed = HandleSimultaneousBonusDisplaySpawns(simultaneousBonusesBeingDisplayed, numberOfSameSpawnTimeDisplays);
            }
            bonusPointsIndicatorObject.SetPointsText(bonusPoints, player.GetComponent<PlayerMovement>().playerIndex);  
        }
		return bonusPoints;
    }

    private bool HandleSimultaneousBonusDisplaySpawns(bool simultaneousCheck, int displayCounter)
	{
        bonusPointIndicatorsInScene = FindObjectsOfType<BonusPointsIndicator>();
        if (bonusPointIndicatorsInScene.Length <= 1)
		{
            return simultaneousCheck = false; ;
		}
        for (int i = 0; i < bonusPointIndicatorsInScene.Length-1; i++)
        {
         
            for (int j = i + 1; j < bonusPointIndicatorsInScene.Length; j++)
            {
                if (bonusPointIndicatorsInScene[i].startTime == bonusPointIndicatorsInScene[j].startTime)
                {
                    print("aSDJKasbDUIVASIDU");
                    simultaneousCheck = true;
                    displayCounter++;
                    bonusPointIndicatorsInScene[j].initialPos += new Vector3(0, simultaneousDisplayHeightOffsetValue * displayCounter, 0);
                }
            }
        }
        return simultaneousCheck;
        //if (bonusPointIndicatorsInScene[0].startTime == bonusPointIndicatorsInScene[1].startTime)
        //{
        //    bonusPointIndicatorsInScene[1].initialPos += new Vector3(0, simultaneousDisplayHeightOffsetValue, 0);
        //}
    }

    private bool DisplayHatDropFeedback(GameObject hatDropDisplay, bool hatDropped)
	{
        if (currentFade == fadeStates.FadeIn)
        {
            if(hatDropDisplay.name == "Player1HatDropDisplay")
			{
                hatDropDisplay.GetComponent<TextMeshProUGUI>().color = Color.Lerp(player1HatDropDisplayTextTransparent, player1HatDropDisplayTextColor, hatDropDisplayFadeTimer.CurrentTime / hatDropDisplayFadeTimer.MaxTime);
            }
            else if(hatDropDisplay.name == "Player2HatDropDisplay")
			{
                hatDropDisplay.GetComponent<TextMeshProUGUI>().color = Color.Lerp(player2HatDropDisplayTextTransparent, player2HatDropDisplayTextColor, hatDropDisplayFadeTimer.CurrentTime / hatDropDisplayFadeTimer.MaxTime);
			}
            
            if (hatDropDisplayFadeTimer.TimerStart == false)
            {
                currentFade = fadeStates.FadeOut;
                hatDropDisplayFadeTimer.TimerStart = true;
            }
        }
        else if (currentFade == fadeStates.FadeOut)
        {
            if (hatDropDisplay.name == "Player1HatDropDisplay")
            {
                hatDropDisplay.GetComponent<TextMeshProUGUI>().color = Color.Lerp(player1HatDropDisplayTextColor, player1HatDropDisplayTextTransparent, hatDropDisplayFadeTimer.CurrentTime / hatDropDisplayFadeTimer.MaxTime);
            }
            else if (hatDropDisplay.name == "Player2HatDropDisplay")
            {
                hatDropDisplay.GetComponent<TextMeshProUGUI>().color = Color.Lerp(player2HatDropDisplayTextColor, player2HatDropDisplayTextTransparent, hatDropDisplayFadeTimer.CurrentTime / hatDropDisplayFadeTimer.MaxTime);
            }
           
            if (hatDropDisplayFadeTimer.TimerStart == false)
            {
                currentFade = fadeStates.FadeIn;
                hatDropDisplay.GetComponent<TextMeshProUGUI>().color = Color.clear;
                hatDropped = false;
            }
        }
        hatDropDisplay.GetComponent<RectTransform>().localScale = Vector3.Lerp(initialMessageScale, finalMessageScale, hatDropDisplayScaleTimer.CurrentTime / hatDropDisplayScaleTimer.MaxTime);
        return hatDropped;
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
                hatcollecter.hatdrop = false;
                // TODO: Perhpas adjust how we can reference different players (not an issue now since we only have 2)
                int bonusPoints = hatcollecter.hatCount + getBonusPoints(player.gameObject);
                int player_id = 0;

                if(hatDropDisplayFadeTimer.TimerRunning==true || hatDropDisplayScaleTimer.TimerRunning==true)
				{
                    hatDropDisplayFadeTimer.ResetTimer();
                    hatDropDisplayScaleTimer.ResetTimer();
                } 
                else if(hatDropDisplayFadeTimer.TimerRunning == false || hatDropDisplayScaleTimer.TimerRunning == false)
				{
                    hatDropDisplayScaleTimer.TimerStart = true;
                    hatDropDisplayFadeTimer.TimerStart = true;
                }
               
                if (player.name == "Gnome_0")
                {
                    playerScore0 += bonusPoints;
                    scoreText0.GetComponent<TMPro.TextMeshProUGUI>().text = playerScore0.ToString();
                    player1HatDropDisplay.GetComponent<TextMeshProUGUI>().text = "+" + hatcollecter.hatCount;
                    player1HatDropped = true;
                    player_id = 0;
                }
                else
                {
                    playerScore1 += bonusPoints;
                    scoreText1.GetComponent<TMPro.TextMeshProUGUI>().text = playerScore1.ToString();
                    player2HatDropDisplay.GetComponent<TextMeshProUGUI>().text = "+" + hatcollecter.hatCount;
                    player2HatDropped  = true;
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
        if (player1HatDropped)
        {
            player1HatDropped=DisplayHatDropFeedback(player1HatDropDisplay,player1HatDropped);
        }
        if (player2HatDropped )
		{
            player2HatDropped =DisplayHatDropFeedback(player2HatDropDisplay,player2HatDropped );
        }
	}
}
