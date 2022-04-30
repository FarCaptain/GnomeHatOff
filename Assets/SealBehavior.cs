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
    private bool once = false;
    private bool FindNearestPointCheck = false;
    private GameObject Sealsocket;
    private Vector3 NearestPosition;
    private Vector3 FinalDestination;
    public GameObject fence;
    public float AdjustYPosition = 0;
    public Animator sealanimator;
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
        if (!collected)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, AdjustYPosition, gameObject.transform.position.z);
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
                    sealanimator.SetBool("moveentry", false);
                    sealanimator.SetBool("clapentry", true);
                    NavAgent.SetDestination(gameObject.transform.position);
                }
            }
        }
        else if(collected && PowerupTimer<=0)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, AdjustYPosition, gameObject.transform.position.z);

            if (!ReachedIsland && !FindNearestPointCheck)
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
                gameObject.transform.GetComponentInParent<PlayerMovement>().enabled = false;
/*                while((gameObject.transform.parent.position - FinalDestination).magnitude > 0.5f)
                {*/
                gameObject.transform.parent.position = Vector3.MoveTowards(gameObject.transform.parent.position, FinalDestination, SealReturnToIslandSpeed * Time.deltaTime);
/*                }*/
                
                
                if ((gameObject.transform.parent.position - FinalDestination).magnitude < 0.5f)
                {
                    ReachedIsland = true;
                    if (gameObject.transform.GetComponentInParent<PlayerMovement>())
                    {
                        gameObject.transform.GetComponentInParent<PlayerMovement>().hasSeal = false;
                        //gameObject.transform.GetComponentInParent<PlayerMovement>().canMove = true;
                        NavAgent.enabled = true;
                    }
                }
            }
            else if(ReachedIsland && !FindNearestPointCheck)
            {
                
                PlayerMovement player = gameObject.transform.GetComponentInParent<PlayerMovement>();
                
                gameObject.transform.parent = null;
                ReachedIsland = false;
                ApproachIce = false;

                player.enabled = true;
                player.transform.position = des.Find("Point").transform.position;
                player.collisionTime = 1;
                gameObject.GetComponent<BoxCollider>().enabled = true;
                player.moveInWater = false;
                FindNearestPoint();
                FindNearestPointCheck = true;
                //player.isMoving = true;
            }
            else if(FindNearestPointCheck)
            {
                FindNearestPoint();
            }
        }
    }

    void FindNearestPoint()
    {
        NavAgent.enabled = true;
        sealanimator.SetBool("moveentry", true);
        sealanimator.SetBool("clapentry", false);
        if (!once)
        {
        NearestPosition = new Vector3(10, 10, 10);
        FinalDestination = new Vector3(0, 0, 0);
        foreach (Transform point in points)
        {
            if ((gameObject.transform.position - point.position).magnitude < NearestPosition.magnitude)
            {
                NearestPosition = gameObject.transform.position - point.position;
                FinalDestination = point.position;
            }
        }
            NavAgent.SetDestination(FinalDestination);
            once = true;
        }
        if (Mathf.Abs(gameObject.transform.position.x - FinalDestination.x) <= 1 && Mathf.Abs(gameObject.transform.position.z - FinalDestination.z) <= 1)
        {
            collected = false;
            ReachedIsland = false;
            once = false;
            FindNearestPointCheck = false;
            PowerupTimer = ResetPowerupTimer;
            for(int i = 0; i < points.Length; i++)
            {
                if(points[i].position == FinalDestination)
                {
                    counter = i;
                    break;
                }
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
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            CirclingTimer = ResetCirclingTimer;
            PowerupTimer -= Time.deltaTime;
            sealanimator.SetBool("moveentry", true);
            sealanimator.SetBool("clapentry", false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && CirclingTimer <= 0)
        {
            //fence.SetActive(false);
            other.gameObject.GetComponent<PlayerMovement>().canSlide = false;
            collected = true;
            other.gameObject.GetComponent<PlayerMovement>().hasSeal = true;
            NavAgent.enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.transform.position = this.transform.position;
            Sealsocket = other.gameObject.GetComponent<PlayerMovement>().SealSocket;
            gameObject.transform.parent = other.gameObject.transform;
            gameObject.transform.localPosition = Sealsocket.transform.localPosition;

            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
