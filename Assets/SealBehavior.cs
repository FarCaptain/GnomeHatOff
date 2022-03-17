using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SealBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject CustomNavMesh;
    private NavMeshAgent NavAgent;
    int counter = 1;
    Transform[] points;
    bool destinationset = false;
    bool collected = false;
    void Start()
    {
        points = CustomNavMesh.GetComponentsInChildren<Transform>();
        NavAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!collected)
        { 
            float RemainingDistance = NavAgent.remainingDistance;
            if (!destinationset)
            {
                destinationset = true;
                Transform Destination = points[counter].transform;
                NavAgent.destination = Destination.position;
                if (counter >= points.Length-1)
                {
                    counter = 1;
                }
                else
                {
                    counter++;
                }
            }
            else if (destinationset && RemainingDistance != Mathf.Infinity && RemainingDistance < 0.15f)
            {
                destinationset = false;
            }
        }
    }
}
