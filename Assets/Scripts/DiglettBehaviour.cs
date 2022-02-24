using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DiglettBehaviour : Hazard
{
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

    public List<Transform> players = new List<Transform>();

    private NewTimer stayTimer;
    private NewTimer hideTimer;
    //private NewTimer warnTimer;

    private enum diglettStates {Hide, Warn, Stay};
    private int state;

    public ParticleSystem dust;
    private NavMeshAgent navMeshAgent;
    public Transform digletModel;

    private int targetedPlayerIndex;
    private bool keepTrack;
    private float coveredPathLength = 0f;

    // Start is called before the first frame update
    void Start()
    {
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
        digletModel.position = new Vector3(pos.x, -1.5f, pos.z);

        navMeshAgent = GetComponent<NavMeshAgent>();
        targetedPlayerIndex = Random.Range(0, players.Count);
    }

    void Update()
    {
        if (state == (int)diglettStates.Hide)
        {
            if (hideTimer.TimerStart == false)
            {
                state = (int)diglettStates.Warn;
                //warnTimer.ResetTimer();
                //warnTimer.TimerStart = true;

                // target players in turn
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
                dust.Play();
            }
        }
        else if (state == (int)diglettStates.Warn)
        {
            Vector3 playerPos = players[targetedPlayerIndex].position;

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

                // Emerging
                Vector3 pos = transform.position;
                digletModel.position = new Vector3(pos.x, 0f, pos.z);
                dust.Stop();
            }
        }
        else
        {
            if (stayTimer.TimerStart == false)
            {
                state = (int)diglettStates.Hide;
                hideTimer.ResetTimer();
                hideTimer.TimerStart = true;

                Vector3 pos = transform.position;
                digletModel.position = new Vector3(pos.x, -1.5f, pos.z);
            }
        }
    }
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
    //    Gizmos.DrawSphere(transform.localPosition + playerPos, initialDistance);
    //}
}
