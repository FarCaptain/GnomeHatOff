using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameStatus
{
    Ready,
    Playing,
}
public class MainGameController : GenericSingleton<MainGameController>
{
    public int level;
    private GameStatus status = GameStatus.Ready;
    [HideInInspector]
    public List<GameObject> players = new List<GameObject>();
    private List<PlayerMovement> playerMovements = new List<PlayerMovement>();
    private int playerAmount;

    public GameObject Arduino;
    public List<string> COM = new List<string>();
    GameObject[] playerfetch;

    // Start is called before the first frame update

    private void Awake()
    {
        players.Clear();

        // We have to have a Arduino Prefab there, when game Controller is intantiated
        Arduino = GameObject.Find("Arduino");

        // when we need prepared players, rather then active them automatically
        if (COM.Count > 0)
        {
            playerfetch = GameObject.FindGameObjectsWithTag("Player");
            Debug.Log("PlayerCount " + playerfetch.Length);
            for (int i = 0; i < playerfetch.Length; i++)
            {
                players.Add(playerfetch[i]);
                RegisterPlayerController(i);
            }
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
        playerMovements.Add(players[playerIndex].GetComponent<PlayerMovement>());
        GameObject arduino = Instantiate(Arduino, players[playerIndex].gameObject.transform);
        arduino.transform.parent = transform;
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
