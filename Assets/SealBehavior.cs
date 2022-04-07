using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SealBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject CustomNavMesh;
    public GameObject CustomIceAnchors;
    private NavMeshAgent NavAgent;
    public float SealReturnToIslandSpeed = 2;
    int counter = 1;
    Transform[] points;
    Transform[] IceAnchors;
    Transform des;
    public float CirclingTimer = 8;
    public float PowerupTimer = 8;
    bool destinationset = false;
    bool collected = false;
    private float ResetCirclingTimer;
    private float ResetPowerupTimer;
    private bool ApproachIce = false;
    private bool ReachedIsland = false;
    private GameObject Sealsocket;
    public GameObject fence;
    public float AdjustYPosition = 0;
    void Start()
    {
        
        points = CustomNavMesh.GetComponentsInChildren<Transform>();
        IceAnchors = CustomIceAnchors.GetComponentsInChildren<Transform>();
        NavAgent = GetComponent<NavMeshAgent>();
        ResetCirclingTimer = CirclingTimer;
        ResetPowerupTimer = PowerupTimer;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, AdjustYPosition, gameObject.transform.position.z);
        if (!collected)
        {
            if (!ApproachIce)
            {
                float RemainingDistance = NavAgent.remainingDistance;
                if (!destinationset)
                {
                    destinationset = true;
                    Transform Destination = points[counter].transform;
                    NavAgent.ResetPath();
                    Debug.Log(NavAgent.SetDestination(Destination.position));

                    //NavAgent.destination = ;
                    if (counter >= points.Length - 1)
                    {
                        counter = 1;
                    }
                    else
                    {
                        counter++;
                    }
                }
                else if (destinationset && RemainingDistance != Mathf.Infinity && RemainingDistance < 0.05f)
                {
                    destinationset = false;
                }
            }
            else if(ApproachIce)
            {
                if (!destinationset)
                {
                    Transform SealTransform = gameObject.transform;
                    NavAgent.SetDestination(SealTransform.position);
                    Vector3 NearestIceAnchorPosition = new Vector3(10, 10, 10);
                    Vector3 FinalDestination = new Vector3(0,0,0);
                    foreach (Transform point in IceAnchors)
                    {
                        if ((SealTransform.position - point.position).magnitude < NearestIceAnchorPosition.magnitude)
                        {
                            NearestIceAnchorPosition = SealTransform.position - point.position;
                            FinalDestination = point.position;
                        }
                    }
                    NavAgent.SetDestination(FinalDestination) ;
                    destinationset = true;
                }
                else if(destinationset && NavAgent.remainingDistance != Mathf.Infinity && NavAgent.remainingDistance < 0.15f)
                {
                    NavAgent.SetDestination(gameObject.transform.position);
                }
            }
        }
        else if(collected && PowerupTimer<=0)
        {
            gameObject.transform.GetComponentInParent<PlayerMovement>().hasSeal = false;

            gameObject.transform.GetComponentInParent<PlayerMovement>().isMoving = true;
            if (!ReachedIsland)
            {
                Vector3 NearestIceAnchorPosition = new Vector3(10, 10, 10);
                Vector3 FinalDestination = new Vector3(0, 0, 0);
                foreach (Transform point in IceAnchors)
                {
                    if ((gameObject.transform.parent.position - point.position).magnitude < NearestIceAnchorPosition.magnitude)
                    {
                        NearestIceAnchorPosition = gameObject.transform.parent.position - point.position;
                        FinalDestination = point.position;
                        des = point;
                    }
                }
                gameObject.transform.parent.position = Vector3.MoveTowards(gameObject.transform.parent.position, FinalDestination, SealReturnToIslandSpeed * Time.deltaTime);
                if((gameObject.transform.parent.position - FinalDestination).magnitude < 0.5f)
                {
                    ReachedIsland = true;
                }
            }
            else if(ReachedIsland)
            {
                
                PlayerMovement player = gameObject.transform.GetComponentInParent<PlayerMovement>();
                player.isMoving = false;
                gameObject.transform.parent = null;
                ReachedIsland = false;
                collected = false;
                ApproachIce = false;
                NavAgent.enabled = true;
                PowerupTimer = ResetPowerupTimer;
                player.transform.position = des.Find("Point").transform.position;
                player.collisionTime = 1;
                gameObject.GetComponent<BoxCollider>().enabled = true;
                player.moveInWater = false;
            }
        }
    }
    void EnableCollider()
    {
        
    }
    private void FixedUpdate()
    {
        if(!collected)
        { 
            CirclingTimer -= Time.deltaTime;
            if(CirclingTimer<=0)
            {
                ApproachIce = true;
                destinationset = false;
            }
        }
        else if (collected)
        {
            CirclingTimer = ResetCirclingTimer;
            PowerupTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && CirclingTimer <= 0)
        {
            //fence.SetActive(false);
            collected = true;
            other.gameObject.GetComponent<PlayerMovement>().hasSeal = true;
            NavAgent.enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.transform.position = this.transform.position;
            Sealsocket = other.gameObject.GetComponent<PlayerMovement>().SealSocket;
            gameObject.transform.parent = other.gameObject.transform;
            gameObject.transform.localPosition = Sealsocket.transform.localPosition;
            gameObject.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
    }
}
