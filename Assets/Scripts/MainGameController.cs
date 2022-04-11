using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GameStatus
{
    Ready,
    Playing,
}
public class MainGameController : MonoBehaviour
{
    // this is a Singleton
    public static MainGameController instance;

    public int level;
    private GameStatus status = GameStatus.Ready;
    [HideInInspector]
    // grab from port Manager
    public List<GameObject> players = new List<GameObject>();
    private List<PlayerMovement> playerMovements = new List<PlayerMovement>();
    private int playerAmount;

    public GameObject Arduino;
    public List<string> COM = new List<string>();
    GameObject[] playerfetch;

    public bool manualMode = false;

    // Start is called before the first frame update

    public void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        players.Clear();
        

        // when we need prepared players, rather then active them automatically
        if ( manualMode )
        {
            playerfetch = GameObject.FindGameObjectsWithTag("Player");
            Debug.Log("PlayerCount " + playerfetch.Length);
            for (int i = 0; i < playerfetch.Length; i++)
            {
                players.Add(playerfetch[i]);
                RegisterPlayerController(i);
            }
        }
        else
            Arduino = GameObject.Find("Arduino");       // We have to have a Arduino Prefab there, when game Controller is intantiated
    }

    void OnSceneLoaded(Scene scene)
    {
        if (manualMode)
            return;

        // get the level on Game Controller according to scene Name
        Debug.Log("OnSceneLoaded: " + scene.name);

        // LevelSelect set the players in PortManager
        if (level == -1)
            return;

        //instantiate all players
        foreach( GameObject gnome in players )
        {
            //gnome.reset();
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {

            status = GameStatus.Playing;

        }
    }

    public void RegisterPlayerController(int playerIndex)
    {
        players[playerIndex].GetComponent<PlayerMovement>().playerIndex = playerIndex;
        //players[playerIndex].GetComponent<Player>().playerIndex = playerIndex;

        playerMovements.Add(players[playerIndex].GetComponent<PlayerMovement>());
        GameObject arduino = Instantiate(Arduino, players[playerIndex].gameObject.transform);
        arduino.GetComponent<SerialController>().portName = COM[playerIndex];
        arduino.GetComponent<SerialController>().enabled = true;
        arduino.GetComponentInChildren<SampleMessageListener>().Playerindex = playerIndex;
        arduino.GetComponentInChildren<SampleMessageListener>().Game = this;
    }

    public void RecieveSignal(int playerIndex, float x, float y, float z)
    {
        //        Debug.Log(playerIndex + x + z + RFID);
        switch (status)
        {
            case GameStatus.Ready:
                playerMovements[playerIndex].SetOffset(new Vector3(x, y, z));
                break;
            case GameStatus.Playing:
                playerMovements[playerIndex].Move(x, y, z);
                break;
        }
    }
}
