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
    public GameObject[] players;
    private List<PlayerMovement> playerMovements = new List<PlayerMovement>();
    private int playerAmount;
    // Start is called before the first frame update
    void Start()
    {
        playerAmount = players.Length;
        for(int i=0; i < playerAmount; i++)
        {
            playerMovements.Add(players[i].GetComponent<PlayerMovement>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {

            status = GameStatus.Playing;

        }
    }

    public void RecieveSignal(int playerIndex, float x, float y, float z, string jump)
    {
        Debug.Log(jump);
        switch (status)
        {
            case GameStatus.Ready:
                playerMovements[playerIndex].SetOffset(new Vector3(x,y,z));
                break;
            case GameStatus.Playing:
                playerMovements[playerIndex].Move(x, y, z);
                if (jump == "False")
                {
                    playerMovements[playerIndex].jumpset(true);
                }

                break;
        }
    }
}
