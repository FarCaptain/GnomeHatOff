using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatCollecter : MonoBehaviour
{
    public GameObject gnome;
    public int hatCount;
    public ScoreSystem scoreSystem;
    public ParticleSystem sparks;
    public GameObject hatPrefab;
    public float gap = 0.1f; // the height difference between hats
    
    public GameObject hatTop;
    public float initHatHeight;
    public float headTop;
    
    public Vector3 initColliderSize;
    public Vector3 initColliderCenter;
    public bool hatdrop = false;
    public bool isTouchingWell = false;

    // stay for some time to drop the hat
    public NewTimer hatDropTimer;
    public float hatDropDuration;

    public Stack<GameObject> hatStack = new Stack<GameObject>();
    public bool isdamaged = false;
    private Image barImage;

    Color[] hatColors = new Color[4];
    AudioSource playerAudioSource;
    Player playerScript;

    private void Start()
    {
        playerAudioSource = GetComponentInParent<AudioSource>();
        
        playerScript = GetComponentInParent<Player>();
        ColorUtility.TryParseHtmlString("#3768A7", out hatColors[0]);
        ColorUtility.TryParseHtmlString("#7637A7", out hatColors[1]);
        ColorUtility.TryParseHtmlString("#A7A037", out hatColors[2]);
        ColorUtility.TryParseHtmlString("#FFFFFF", out hatColors[3]); //original

        hatdrop = false;
        for (int i = 0; i < gnome.transform.childCount; i++)
        {
            if (gnome.transform.GetChild(i).name == "HatTop")
                hatTop = gnome.transform.GetChild(i).gameObject;
            else if (gnome.transform.GetChild(i).name == "HeadTop")
                headTop = gnome.transform.GetChild(i).position.y;
        }
        initHatHeight = hatTop.transform.position.y;
        initColliderSize = GetComponent<BoxCollider>().size;
        initColliderCenter = GetComponent<BoxCollider>().center;

        //init the hatdrop timer
        hatDropTimer = gameObject.AddComponent<NewTimer>();
        hatDropTimer.MaxTime = hatDropDuration;

        
    }

    private void Update()
    {
        Image barImage = GetComponentInChildren<Image>();
        //Debug.Log(hatdrop);
        if (hatDropTimer.TimerRunning)
        {
            barImage.fillAmount = hatDropTimer.TimerCompletionRate;
        }
        else
        {
            barImage.fillAmount = 0;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hat" && isdamaged == false && other.gameObject.GetComponent<HatFade>().hatCollectedByPlayer==false)
        {
            if (other.gameObject.transform.position.y > (headTop + 0.01f))
            {
                if(hatCount < 9)
                {
                    if (playerAudioSource.isPlaying == false)
                    {
                        AudioManager.PlayHatAudioClip(HatAudioStates.Collected, playerAudioSource);
                    }
                    AddHat(other.gameObject);
                }
                else
                {
                    Destroy(other.gameObject);
                }
                //print("Yeay! Hat Collected!" + (++count));
               
                
                other.gameObject.GetComponent<HatFade>().hatShadowDestroy();
            }
        }
        if (other.tag == "Mushroom")
        {
            if (playerAudioSource.isPlaying == false)
            {
                AudioManager.PlayMushroomManAudioClip(MushroomManAudioStates.Collected, playerAudioSource);
            }
            
            for (int i=hatCount; i < 9; i++)
            {
                GameObject hat = Instantiate(hatPrefab, hatTop.transform.position, Quaternion.identity);
                hat.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = hatColors[Random.Range(0, hatColors.Length - 1)];
                AddHat(hat);
            }
            playerScript.SuperBounce();
            other.GetComponent<MushroomController>().Caught();
            other.GetComponent<MushroomController>().hatShadowDestroy();
            
            updateCollecter();
            
        }
    }
    private void drawSparks()
    {
        sparks.Play();
    }

    public void updateCollecter()
    {
        BoxCollider bc = GetComponent<BoxCollider>();
        Vector3 size = bc.size;
        Vector3 center = bc.center;
        int hatCount = hatStack.Count;
        float param = 0.2f;
        Vector3 hatPos = hatTop.transform.position;
        hatTop.transform.position = new Vector3(hatPos.x, initHatHeight+hatCount*gap, hatPos.z);
       
        // the scale of the collider is different from the hat model
        
        bc.size = new Vector3(size.x, initColliderSize.y + gap * param * hatCount, size.z);
        bc.center = new Vector3(center.x, initColliderCenter.y + 0.5f * gap * param * hatCount, center.z);
    }

    private void AddHat(GameObject hat)
    {
        
        hatCount += 1;
        hat.GetComponent<HatFade>().hatCollectedByPlayer = true;
        Vector3 hatPos = hatTop.transform.position;
        hatPos = new Vector3(hatPos.x, hatPos.y + gap, hatPos.z);
        Destroy(hat.GetComponent<Rigidbody>());
        //other.gameObject.GetComponentInChildren<MeshRenderer>().material

       
        //hat.gameObject.GetInstanceID
        hatStack.Push(hat);

        hat.transform.parent = gnome.transform;
        hat.gameObject.transform.position = hatPos;
        hatTop.transform.position = hatPos;

        sparks.gameObject.transform.position = hatPos;
        drawSparks();

        //decrease speed of Gnome
        PlayerMovement movement = GetComponentInParent<PlayerMovement>();
        movement.hatBurden = movement.speedDecreaseEachHat * hatCount;
        updateCollecter();
    }
}
