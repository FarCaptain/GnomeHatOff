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

    public float DiglettSpeed;

    public List<Transform> players = new List<Transform>();

    private NewTimer stayTimer;
    private NewTimer hideTimer;
    //private NewTimer warnTimer;

    private enum diglettStates {Hide, Warn, Stay};
    private int state;

    public ParticleSystem dust;
    private NavMeshAgent navMeshAgent;
    public Transform digletModel;

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

                // ToDo. might need bias to avoid bug
                int index = Random.Range(0, players.Count);
                Vector3 playerPos = players[index].position;
                navMeshAgent.destination = playerPos;
                navMeshAgent.speed = DiglettSpeed;
                dust.Play();
            }
        }
        else if (state == (int)diglettStates.Warn)
        {
            float dist = navMeshAgent.remainingDistance;
            if (dist != Mathf.Infinity && navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && navMeshAgent.remainingDistance == 0)
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
}
