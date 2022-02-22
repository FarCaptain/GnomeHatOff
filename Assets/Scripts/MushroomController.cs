﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{

    GameObject[] players;
    public float defaultTransparency = 1f;
    public float fadeDuration = 3f;
    public GameObject player;
    public GameObject shadowPrefab;
    public HatSpawning hatSpawn;
    public ParticleSystem circleDust;
    private float closeDistance = 2;
    public float accelerationTime = 1f;
    public float maxSpeed = 0.5f;
    private Vector3 movement;
    private float timeLeft;
    [SerializeField] public Rigidbody rb;

    bool isOnGround = false;
    float headTop;

    public bool hatFadeEnabled = true;
    private MeshCollider collider;
    private AudioSource mushroomManAudioSource;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        mushroomManAudioSource = GetComponent<AudioSource>();
        AudioManager.PlayMushroomManAudioClip(MushroomManAudioStates.Falling, mushroomManAudioSource);

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
            rb.velocity = (movement.normalized * maxSpeed);
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
        int closePlayer = ClosedToGnome();
        if (closePlayer != -1)
        {
           
            Escape(players[closePlayer]);
        }

        if (timeLeft <= 0 )
        {
            
            if(closePlayer == -1)
            {
                movement = new Vector3(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f));
            }
                
            timeLeft = accelerationTime;
        }
        


    }
    public void OnCollisionStay(Collision collision)
    {
        if (isOnGround && collision.gameObject.tag != "Ground")
        {
            timeLeft = 0;

        }
    }
    public void OnCollisionEnter(Collision s)
    {
        if (isOnGround)
        {
            movement = -movement;
           
        }
        if(s.gameObject.tag == "Ground")
		{
            AudioManager.PlayMushroomManAudioClip(MushroomManAudioStates.Landed, mushroomManAudioSource);
        }
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
            float distance = Vector2.Distance(transform.position, players[i].transform.position);
            if ( distance < closeDistance && distance < minDistance)
            {
                minDistance = distance;
                closest = i;
            }
        }
        return closest;
    }
}