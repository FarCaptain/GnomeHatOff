using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{

    GameObject[] players;
    private float moveScaleX = 4.5f;
    public float defaultTransparency = 1f;
    public float fadeDuration = 3f;
    public GameObject player;
    public GameObject shadowPrefab;
    public HatSpawning hatSpawn;
    public ParticleSystem circleDust;

    [SerializeField] public Rigidbody rb;
    [SerializeField] Animator mushroomManAnimator;

    public Vector3 dropSpeed;
    public float closeDistance;
    private float moveSpeed;
    public float highSpeed;
    public float lowSpeed;
    public float catchTime;
    private float accelerationTime = 0.5f;
    
    private float bornTime;
    
    private Vector3 movement;
    private float timeLeft;
    

    bool isOnGround = false;
    float headTop;

    public bool hatFadeEnabled = true;
    private AudioSource mushroomManAudioSource;
    void Start()
    {
        bornTime = 0;
        players = GameObject.FindGameObjectsWithTag("Player");
        mushroomManAudioSource = GetComponent<AudioSource>();
        AudioManager.PlayMushroomManAudioClip(MushroomManAudioStates.Falling, mushroomManAudioSource);

        hatSpawn = GameObject.Find("HatSpawner").GetComponent<HatSpawning>();
        player = GameObject.Find("Gnome_0");
        rb.velocity = -dropSpeed;

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
        bornTime += Time.fixedDeltaTime;
        if(bornTime > catchTime)
        {
            moveSpeed = lowSpeed;
        }
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
            mushroomManAnimator.SetTrigger("mushroomManOnGround");
            Move();
            Debug.Log(moveSpeed);
            rb.velocity = (movement.normalized * moveSpeed);
        }
        
        
        
    }

    void SetOnGround()
    {
        isOnGround = true;
    }



    public void hatShadowDestroy()
    {
        Destroy(shadowPrefab);
        hatSpawn.mushroomOneTime--;
    }
    public void drawCircleDust()
    {
        circleDust.Play();
    }


    public void Move()
    {
        
        int closePlayer = ClosedToGnome();
        timeLeft -= Time.deltaTime;
        if (closePlayer != -1)
        {

            Escape(players[closePlayer]);
            if(bornTime < catchTime)
            moveSpeed = highSpeed+1;
            OutScale(true);
        }
        else
        {
            if (bornTime < catchTime)
                moveSpeed = highSpeed;
            OutScale(false);
            if (movement == Vector3.zero)
            {
                movement = new Vector3(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f));
            }
            timeLeft = accelerationTime;

            if (timeLeft <= 0)
            {

                if (closePlayer == -1)
                {
                    movement = new Vector3(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f));
                }

                timeLeft = accelerationTime;
            }
        }
        Vector3 newDirection = Vector3.Normalize(rb.velocity);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }


    public bool OutScale(bool ifClosed)
    {
        float x = transform.position.x;
        float z = transform.position.z;
        float movementX = movement.x;
        float movementZ = movement.z;
        bool outcome = false;
        if (x < -moveScaleX)
        {
            if (ifClosed)
            {
                movementX = 0;
            }
            else
            {
                movementX = movementX < 0 ? -movementX : movementX;
            }
            
            outcome = true;
        }
        else if ( x > moveScaleX)
        {
            if (ifClosed)
            {
                movementX = 0;
            }
            else
            {
                movementX = movementX < 0 ? movementX : -movementX;
            }
           
            outcome = true;
        }

        if (z < -2.2f)
        {
            if (ifClosed)
            {
                movementZ = 0;
            }
            else
            {
                movementZ = movementZ < 0 ? -movementZ : movementZ;
            }
            outcome = true;
        }
        else if (  z > 2.2f)
        {
            if (ifClosed)
            {
                movementZ = 0;
            }
            else
            {
                movementZ = movementZ < 0 ? movementZ : -movementZ;
            }
            
            outcome = true;
        }
        movement = new Vector3(movementX, 0, movementZ);
        return outcome;
    }
    public void OnCollisionEnter(Collision s)
    {
        Debug.Log(s.gameObject.name);
        if(s.gameObject.tag == "Ground")
		{
            AudioManager.PlayMushroomManAudioClip(MushroomManAudioStates.Landed, mushroomManAudioSource);
            return;
        }
        if(s.gameObject.tag != "wall")
        {
            movement = -movement;
        }
    }
    public void Caught()
    {
        Destroy(gameObject);
    }
    private void Escape(GameObject player)
    {

        Vector3 vec = transform.position - player.transform.position;
        movement = new Vector3(vec.x,0,vec.z).normalized;
       
    }

    private int ClosedToGnome()
    {
        int closest = -1;
        float minDistance = 999;
        for (int i = 0; i < players.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, players[i].transform.position);
            if ( distance < closeDistance && distance < minDistance)
            {
                minDistance = distance;
                closest = i;
            }
        }
        //if (closest != -1)
        //{
        //    Debug.Log(transform.position+" "+player[closest].transform.positio+" "+ minDistance+" "+closeDistance);
            
        //}
        return closest;
    }
}