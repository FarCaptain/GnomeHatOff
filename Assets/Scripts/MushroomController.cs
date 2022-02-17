using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{

    public float defaultTransparency = 1f;
    public float fadeDuration = 3f;
    public GameObject player;
    public GameObject shadowPrefab;
    public HatSpawning hatSpawn;
    public ParticleSystem circleDust;

    public float accelerationTime = 1f;
    public float maxSpeed = 3f;
    private Vector3 movement;
    private float timeLeft;
    [SerializeField] public Rigidbody rb;

    bool isOnGround = false;
    float headTop;

    public bool hatFadeEnabled = true;
    private MeshCollider collider;
    void Start()
    {
        
        hatSpawn = GameObject.Find("HatSpawner").GetComponent<HatSpawning>();
        player = GameObject.Find("Gnome_0");

        // TODO: Adjust for loop code
        for (int i = 0; i < player.transform.childCount; i++)
        {
            // inefficient, way to do on resources?
            if (player.transform.GetChild(i).name == "HeadTop")
            {
                headTop = player.transform.GetChild(i).transform.position.y;
                break;
            }
        }
    }

    void FixedUpdate()
    {
     

        if (hatFadeEnabled && transform.position.y < headTop)
        {

            Destroy(shadowPrefab);
            circleDust.Play();
            Destroy(circleDust, 1f);
            hatFadeEnabled = false;
            Invoke("SetOnGround", 0.1f);
        }

        //On ground
        if (isOnGround)
        {
            Move();
            rb.velocity = (movement * maxSpeed);
        }
        
        
        
    }

    void SetOnGround()
    {
        isOnGround = true;
    }



    public void hatShadowDestroy()
    {
        Destroy(shadowPrefab);
        hatSpawn.mushroomCount--;
    }
    public void drawCircleDust()
    {
        circleDust.Play();
    }


    public void Move()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            movement = new Vector3(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f));
            timeLeft = accelerationTime;
        }
        


    }

    public void OnCollisionEnter(Collision s)
    {
        if (isOnGround)
        {
            Debug.Log("sss");
            movement = -movement;
            
        }
    }
}