using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameStatus
{
    Ready,
    Playing,
}
public class MainGameController : MonoBehaviour
{
    private GameStatus status = GameStatus.Ready;
    [HideInInspector]
    public GameObject[] players;
    private List<PlayerMovement> playerMovements = new List<PlayerMovement>();
    private int playerAmount;
    public GameObject Arduino;
    public string[] COM;
    GameObject[] playerfetch;

    // Start is called before the first frame update

    private void Awake()
    {
        playerfetch = GameObject.FindGameObjectsWithTag("Player");
        players = new GameObject[playerfetch.Length];
        Debug.Log("PlayerCount " + playerfetch.Length);
        for (int i = 0; i < playerfetch.Length; i++)
        {
            players[i] = playerfetch[i];
            players[i].GetComponent<PlayerMovement>().playerIndex = i;
            playerMovements.Add(players[i].GetComponent<PlayerMovement>());
            GameObject arduino =  Instantiate(Arduino, players[i].gameObject.transform);
            arduino.GetComponent<SerialController>().portName = COM[i];
            arduino.GetComponent<SerialController>().enabled = true;
            arduino.GetComponentInChildren<SampleMessageListener>().Playerindex = i;
            arduino.GetComponentInChildren<SampleMessageListener>().Game = this;
         
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

    public void RecieveSignal(int playerIndex, float x, float y, float z, string RFID)
    {
        //        Debug.Log(playerIndex + x + z + RFID);
        switch (status)
        {
            case GameStatus.Ready:
                playerMovements[playerIndex].SetOffset(new Vector3(x, y, z));
                break;
            case GameStatus.Playing:
                playerMovements[playerIndex].Move(x, y, z);
                if (RFID == "False")
                {
                    playerMovements[playerIndex].jumpset(true);
                }

                break;
        }
    }
}
