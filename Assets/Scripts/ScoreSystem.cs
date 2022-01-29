using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public GameObject scoreText0;
    public GameObject scoreText1;
    public GameObject player0;
    public GameObject player1;

    static public int hatCount0 = 0;
    static public int hatCount1 = 0;
    
    static public int playerScore0 = 0;
    static public int playerScore1 = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider player)
    {
        if(player.tag == "Player")
        {
            if (player.name == "Gnome")
            {
                playerScore0 += hatCount0;
                hatCount0 = 0;
                scoreText0.GetComponent<TMPro.TextMeshProUGUI>().text = "P1 Score: " + playerScore0;
            }
            else
            {
                playerScore1 += hatCount1;
                hatCount1 = 0;
                scoreText1.GetComponent<TMPro.TextMeshProUGUI>().text = "P1 Score: " + playerScore1;
            }


            for (int i = 0; i < player.transform.childCount; i++)
            {
                if (player.transform.GetChild(i).name == "HatPrefab(Clone)")
                {
                    print("YYYYY");
                    print("ddd"+playerScore0);
                    Destroy(player.transform.GetChild(i).gameObject);
                }
                    
            }
        }
    }
}
