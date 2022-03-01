using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [Header("Points Display Objects")]
  
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

    //New
    #region more than 2 players
    static Dictionary<int, int> playerScores;
    private Dictionary<int, GameObject> go_playerScores;
    public int PlayerAmount;
    public Transform scoreBoard;
    public MainGameController gameManager;
    #endregion

    public ParticleSystem[] fires = new ParticleSystem[2];
    private Vector3[,] initFireScale = new Vector3[2,4];

    private NewTimer hatDropDisplayScaleTimer;
    private NewTimer hatDropDisplayFadeTimer;
    


    private BonusPointsIndicator[] bonusPointIndicatorsInScene;
    private bool simultaneousBonusesBeingDisplayed = false;
    // Start is called before the first frame update
    void Start()
	{
        PlayerAmount = gameManager.COM.Length;
        for (int id = 0; id < 2; id++)
			for (int i = 0; i < fires[id].transform.childCount; i++)
				initFireScale[id, i] = fires[id].transform.GetChild(i).transform.localScale;

		InitializeFadeAndScaleTimer();

        #region more than 2 players
        playerScores = new Dictionary<int, int>();
        go_playerScores = new Dictionary<int, GameObject>();
        GameObject scorePrefab = Resources.Load<GameObject>("Prefabs/Score");
        
        //Initiate scoreboard
        for(int i=0; i < PlayerAmount; i++)
        {
            GameObject go_score = Instantiate(scorePrefab, scoreBoard);
            Sprite s = Resources.Load<Sprite>("Texture/Score_" + i);
            go_score.GetComponent<Image>().sprite = s;
            go_playerScores.Add(i, go_score);
            playerScores.Add(i, 0);
        }
        #endregion
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
        AudioManager.PlayGeneralGameAudioClip(GameGeneralAudioStates.RoundEnd);
        int win_Index = 0;
        int maxScore = -1;
        for(int i = 0; i < PlayerAmount; i++)
        {
            if (playerScores[i] > maxScore)
            {
                maxScore = playerScores[i];
                win_Index = i;
            }
        }
        winText.text = "Player "+ win_Index+" Wins!!!";
        winnerScoreText.text = "Score: " + maxScore;

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

 //   private bool DisplayHatDropFeedback(GameObject hatDropDisplay, bool hatDropped)
	//{
 //       if (currentFade == fadeStates.FadeIn)
 //       {
 //           if(hatDropDisplay.name == "Player1HatDropDisplay")
	//		{
 //               hatDropDisplay.GetComponent<TextMeshProUGUI>().color = Color.Lerp(player1HatDropDisplayTextTransparent, player1HatDropDisplayTextColor, hatDropDisplayFadeTimer.CurrentTime / hatDropDisplayFadeTimer.MaxTime);
 //           }
 //           else if(hatDropDisplay.name == "Player2HatDropDisplay")
	//		{
 //               hatDropDisplay.GetComponent<TextMeshProUGUI>().color = Color.Lerp(player2HatDropDisplayTextTransparent, player2HatDropDisplayTextColor, hatDropDisplayFadeTimer.CurrentTime / hatDropDisplayFadeTimer.MaxTime);
	//		}
            
 //           if (hatDropDisplayFadeTimer.TimerStart == false)
 //           {
 //               currentFade = fadeStates.FadeOut;
 //               hatDropDisplayFadeTimer.TimerStart = true;
 //           }
 //       }
 //       else if (currentFade == fadeStates.FadeOut)
 //       {
 //           if (hatDropDisplay.name == "Player1HatDropDisplay")
 //           {
 //               hatDropDisplay.GetComponent<TextMeshProUGUI>().color = Color.Lerp(player1HatDropDisplayTextColor, player1HatDropDisplayTextTransparent, hatDropDisplayFadeTimer.CurrentTime / hatDropDisplayFadeTimer.MaxTime);
 //           }
 //           else if (hatDropDisplay.name == "Player2HatDropDisplay")
 //           {
 //               hatDropDisplay.GetComponent<TextMeshProUGUI>().color = Color.Lerp(player2HatDropDisplayTextColor, player2HatDropDisplayTextTransparent, hatDropDisplayFadeTimer.CurrentTime / hatDropDisplayFadeTimer.MaxTime);
 //           }
           
 //           if (hatDropDisplayFadeTimer.TimerStart == false)
 //           {
 //               currentFade = fadeStates.FadeIn;
 //               hatDropDisplay.GetComponent<TextMeshProUGUI>().color = Color.clear;
 //               hatDropped = false;
 //           }
 //       }
 //       hatDropDisplay.GetComponent<RectTransform>().localScale = Vector3.Lerp(initialMessageScale, finalMessageScale, hatDropDisplayScaleTimer.CurrentTime / hatDropDisplayScaleTimer.MaxTime);
 //       return hatDropped;
 //   }

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

                #region More than 2 players
                AddScore(player.gameObject, bonusPoints, hatcollecter.hatCount);
                #endregion



                //if (player.name == "Gnome_0")
                //{
                //    playerScore0 += bonusPoints;
                //    scoreText0.GetComponent<TMPro.TextMeshProUGUI>().text = playerScore0.ToString();
                //    player1HatDropDisplay.GetComponent<TextMeshProUGUI>().text = "+" + hatcollecter.hatCount;
                //    player1HatDropped = true;
                //    player_id = 0;
                //}
                //else
                //{
                //    playerScore1 += bonusPoints;
                //    scoreText1.GetComponent<TMPro.TextMeshProUGUI>().text = playerScore1.ToString();
                //    player2HatDropDisplay.GetComponent<TextMeshProUGUI>().text = "+" + hatcollecter.hatCount;
                //    player2HatDropped  = true;
                //    player_id = 1;
                //}

                hatcollecter.hatCount = 0;
                
                //revertChangesOnFire(player_id);
                //magnifyFire(player_id, getFireSize(player.gameObject));
                //fires[player_id].Play();

                player.GetComponentInChildren<PlayerMovement>().hatBurden = 0f;

                while (hatcollecter.hatStack.Count != 0)
                {
                    Destroy(hatcollecter.hatStack.Pop());
                }
                

                //reset the collision on the gnome
                if (hatcollecter.hatTop.transform.position.y != hatcollecter.initHatHeight)
                {
                    hatcollecter.updateCollecter();
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

	}

 
    
    private void AddScore(GameObject player, int score, int hatCount)
    {
        int player_index = player.GetComponent<Player>().playerIndex;

        int newScore = playerScores[player_index] + score;
        Debug.Log(newScore);
        playerScores.Remove(player_index);
        playerScores.Add(player_index, newScore);

        go_playerScores[player_index].transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = newScore.ToString();
        go_playerScores[player_index].transform.Find("HatDropDisplay").GetComponent<TMPro.TextMeshProUGUI>().text = "+" + hatCount.ToString();
        go_playerScores[player_index].transform.Find("HatDropDisplay").GetComponent<Animation>().Play();
        //player1HatDropped = true;

        //revertChangesOnFire(player_index);
        //magnifyFire(player_index, getFireSize(player.gameObject));
        //fires[player_index].Play();
    }
}
