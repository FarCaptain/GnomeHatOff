using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HatSpawning : MonoBehaviour
{
    public GameObject hatPrefab;
    public GameObject hatShadowPrefab;
    public GameObject mushroomManPrefab;
    public GameObject hatRushMessage;
    public TextMeshProUGUI timerText;
    public Animator hatRushAnim;
    public float hatshadowDestroyTime=4;

    public Vector3 center;
    public Vector3 size;
    public float minGap = 0.3f;
    public float maxGap = 2.0f;
    public float hatRushTime = 30f;
    public float hatRushMinGap = 0.2f;

    private int mushroomCount = 0;
    public int mushroomMaxCount;
    public int mushroomOneTime = 0;
    float timeInterval = 0f;
    bool isHatRush = false;

    float timeGap;
    float deltaDashChange;
    public float accumulatedSpeed;
    Color[] hatColors = new Color[4];

    // Start is called before the first frame update
    void Start()
    {
        timeGap = Random.Range( 0.6f, 3.5f);

        ColorUtility.TryParseHtmlString("#3768A7",out hatColors[0]);
        ColorUtility.TryParseHtmlString("#7637A7", out hatColors[1]);
        ColorUtility.TryParseHtmlString("#A7A037", out hatColors[2]);
        ColorUtility.TryParseHtmlString("#FFFFFF", out hatColors[3]); //original

        float dashStartSpeed = minGap;
        hatRushMinGap = Mathf.Min(dashStartSpeed, hatRushMinGap);

        deltaDashChange = (dashStartSpeed - hatRushMinGap) / hatRushTime;
        accumulatedSpeed = dashStartSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // HAT RUSH!!!!
        if (!isHatRush && Timer.timeRemaining < hatRushTime)
        {
            isHatRush = true;
            timerText.color = Color.red;
            hatRushMessage.SetActive(true);
            AudioManager.PlayGeneralGameAudioClip(GameGeneralAudioStates.HatRushBegin);
            hatRushAnim.Play("FadeAnimation");
        }
            
        timeInterval += Time.deltaTime;

        
        if (timeInterval > timeGap)
        {
            int ifMushroom = Random.Range(0, 100);
            if(ifMushroom < 30 && !isHatRush)
            {
                timeGap = Random.Range(minGap, maxGap);
                if (mushroomOneTime < 1&& mushroomCount<mushroomMaxCount)
                {
                    SpawnMushroomMan();
                    mushroomOneTime++;
                    mushroomCount++;
                }
                    
            }
            else
            {
                timeGap = isHatRush ? accumulatedSpeed = Mathf.Max(accumulatedSpeed - (deltaDashChange * timeGap), hatRushMinGap) : Random.Range(minGap, maxGap);
                SpawnHat();
            }
            
            timeInterval = 0f;
        }

        //test
        if (Input.GetKeyDown(KeyCode.K))
        {
                SpawnMushroomMan();
               
            
        }
        
    }

    public void SpawnHat()
    {
        Vector3 pos = transform.localPosition + center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
        GameObject hat = Instantiate(hatPrefab, pos, Quaternion.identity);
        hat.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = hatColors[Random.Range(0, hatColors.Length - 1)];
        generateShadow(pos, hat, "HatFade");
    }

    public void generateShadow(Vector3 pos, GameObject hat, string type)
    {
        GameObject hatShadow = Instantiate(hatShadowPrefab, pos - (new Vector3(0, pos.y, 0)), Quaternion.identity);
        if(type.Equals("HatFade"))
            hat.transform.GetComponent<HatFade>().shadowPrefab = hatShadow;
        if (type.Equals("MushroomController"))
        {
            hat.transform.GetComponent<MushroomController>().shadowPrefab = hatShadow;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawCube(transform.localPosition + center, size);
    }

    void SpawnMushroomMan()
    {
        Vector3 pos = transform.localPosition + center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
        GameObject hat = Instantiate(mushroomManPrefab, pos, Quaternion.identity);
        hat.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.black;
        generateShadow(pos, hat, "MushroomController");
       
    }
}
