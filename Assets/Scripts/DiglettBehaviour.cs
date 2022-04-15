using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class DiglettBehaviour : Hazard
{

    AudioSource audio;
    [Header("")]
    public float minHideTime;
    public float maxHideTime;

    public float stayTime;
    //public float warnTime;

    public float diglettSpeed;
    //public float initialDistance;
    //public float stopTrackingDistance;
    public float trackingPathLength;
    public List<Transform> spawners = new List<Transform>();

    private List<Transform> players = new List<Transform>();

    private NewTimer stayTimer;
    private NewTimer hideTimer;
    //private NewTimer warnTimer;

    private enum diglettStates {Hide, Warn, Stay};
    private int state;

    public ParticleSystem dustPrefab;
    private ParticleSystem dust;
    public VisualEffect poof;
    private NavMeshAgent navMeshAgent;
    public Transform digletModel;


    private int targetedPlayerIndex;
    private bool keepTrack;
    private float coveredPathLength = 0f;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponentInParent<AudioSource>();
        //hide - warn - stay
        stayTimer =gameObject.AddComponent<NewTimer>();
        hideTimer = gameObject.AddComponent<NewTimer>();
        //warnTimer =gameObject.AddComponent<NewTimer>();

        // config the timers
        stayTimer.MaxTime = stayTime;
        hideTimer.MaxTime = Random.Range(minHideTime, maxHideTime);
        //warnTimer.MaxTime = warnTime;

        // set the Timer to Hide
        state = (int)diglettStates.Hide;
        hideTimer.TimerStart = true;

        Vector3 pos = new Vector3(-2f, 0f, 2f);
        transform.position = pos;
        digletModel.position = new Vector3(pos.x, -2f, pos.z);

        navMeshAgent = GetComponent<NavMeshAgent>();
        UpdatePlayers();
        targetedPlayerIndex = Random.Range(0, players.Count);
    }

    void Update()
    {
        if (state == (int)diglettStates.Hide)
        {
            if (hideTimer.TimerStart == false)
            {
                state = (int)diglettStates.Warn;

                // target players in turn
                if (players.Count == 0)
                {
                    UpdatePlayers();
                    targetedPlayerIndex = Random.Range(0, players.Count);

                    if (players.Count == 0)
                    {
                        Debug.LogError("[Diglett]Something Went wrong. Can't find the Players!");
                        return;
                    }
                }
                targetedPlayerIndex = (targetedPlayerIndex + 1) % players.Count;
                Vector3 playerPos = players[targetedPlayerIndex].position;
                // ToDo. might need bias to avoid bug

                // find the farthest spawnPoint
                Vector3 spawnPos = transform.position;
                float maxDis = 0f;
                for(int i = 0; i < spawners.Count; i++)
                {
                    float dis = Vector3.Distance(spawners[i].position, playerPos);
                    if(dis > maxDis)
                    {
                        maxDis = dis;
                        spawnPos = spawners[i].position;
                    }
                }
                transform.position = spawnPos;

                navMeshAgent.destination = playerPos;
                navMeshAgent.speed = diglettSpeed;
                keepTrack = true;
                dust = GameObject.Instantiate(dustPrefab, transform.position, Quaternion.identity);
            }
        }
        else if (state == (int)diglettStates.Warn)
        {
            
            Vector3 playerPos = players[targetedPlayerIndex].position;

            dust.transform.position = new Vector3(transform.position.x, 0.01f, transform.position.z);
            if (dust.isPlaying == false)
                dust.Play();

            // keep following the player if it is not close enough
            //if (keepTrack && Vector3.Distance(playerPos, transform.position) < stopTrackingDistance)
            if (keepTrack && coveredPathLength >= trackingPathLength)
            {
                coveredPathLength = 0f;
                keepTrack = false;
            }

            if (keepTrack)
            {
                navMeshAgent.destination = playerPos;
                coveredPathLength += Time.deltaTime * navMeshAgent.speed;
            }

            float dist = navMeshAgent.remainingDistance;
            //&& navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete
            if (dist != Mathf.Infinity  && dist < 0.15f)
            {
                state = (int)diglettStates.Stay;
                stayTimer.ResetTimer();
                stayTimer.TimerStart = true;

                poof.Play();
                AudioManager.PlayDigletAudioClip(DigletAudioStates.Raise, audio);  // emereging audio
                // Emerging
                Vector3 pos = transform.position;
                digletModel.position = new Vector3(pos.x, 0f, pos.z);
                Destroy(dust);
                dust.Stop();
            }
        }
        else
        {
            if (stayTimer.TimerStart == false)
            {
                state = (int)diglettStates.Hide;
                AudioManager.PlayDigletAudioClip(DigletAudioStates.Despawn, audio);
                hideTimer.ResetTimer();
                hideTimer.TimerStart = true;

                Vector3 pos = transform.position;
                digletModel.position = new Vector3(pos.x, -2f, pos.z);
            }
        }
    }

    private void UpdatePlayers()
    {
        players.Clear();
        foreach ( GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player.transform);
        }
    }
}
