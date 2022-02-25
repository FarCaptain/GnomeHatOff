using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Collision Layers")]
    [SerializeField] string defaultLayerName;
    [SerializeField] string iFramesLayerName;

    public int playerIndex;
    [Header("Damage Variables")]
    [SerializeField] float iFrameMaxTime = 2f;

    [Header("Character Bumping Variables")]
    [SerializeField] PhysicMaterial knockbackMaterial;

    [Header("Super Bump Variables")]
    [SerializeField] float bumpForce = 10f;
    [SerializeField] float superBounceMass = 10f;
    [HideInInspector] public bool superBump = false;
    [SerializeField] float superBumpMaxTime = 3f;

    //Cached Player Components
    Rigidbody playerRigidBody;
    PlayerMovement playerMovement;
    HatCollecter playerHatCollecter;
    NewTimer stealHatIFrame;
    NewTimer superBumpTimer;
    AudioSource playerAudioSource;
    [HideInInspector]
    public BoxCollider playerBoxCollider;

    //REMOVE IF NEEDED
    static List<GameObject> players = new List<GameObject>();
    void Start()
    { 
        playerRigidBody = gameObject.GetComponent<Rigidbody>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerHatCollecter = gameObject.GetComponentInChildren<HatCollecter>();
        playerAudioSource = GetComponent<AudioSource>();
        playerBoxCollider = GetComponent<BoxCollider>();

        stealHatIFrame = gameObject.AddComponent<NewTimer>();
        stealHatIFrame.MaxTime = iFrameMaxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(superBumpTimer!=null && superBumpTimer.TimerStart==false)
		{
            superBump = false;
            playerRigidBody.mass = 1f;
            Destroy(superBumpTimer);
        }
    }

	private void FixedUpdate()
	{
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag=="Player")
		{
            if (!players.Contains(other.gameObject))
            {
                players.Add(other.gameObject);
                players[players.Count - 1].GetComponent<BoxCollider>().material = knockbackMaterial;
            }
        }
	}

	private void OnTriggerExit(Collider other)
	{
        playerMovement.canMove = true;
        if (other.gameObject.tag == "Player")
        {
            if (players.Contains(other.gameObject))
            {
                players[players.Count - 1].GetComponent<BoxCollider>().material = null;
                players.Remove(other.gameObject);
 
            }
        }
    }

	private void OnCollisionEnter(Collision collision)
    {
        /// Guard Statment
        /// Always make sure player can move before applying any debuff on them
        if(playerMovement.canMove == false)
		{
            return;
		}

        if(collision.gameObject.tag=="Player")
		{
            playerMovement.canMove = false;
            if(collision.gameObject.GetComponent<Player>().superBump == true)
			{
                playerRigidBody.AddForce(-transform.forward * bumpForce, ForceMode.Impulse);
            }
        }

		if (collision.gameObject.tag.Contains("Knockback"))
        {
            StartCoroutine(KnockbackPlayer(collision.gameObject.GetComponentInParent<Hazard>()));
        }

        if (collision.gameObject.tag.Contains("Damage") && stealHatIFrame.TimerStart == false)
        {
            OnDamageEnable();
        }

        if (collision.gameObject.tag.Contains("StealHat"))
        {
            if(playerHatCollecter.hatCount==0)
			{
                return;
			}
            else
			{
                if(collision.gameObject.GetComponent<Hazard>()==null)
				{
                    SetMaxHatsToStealBasedOnType(collision.gameObject.GetComponentInParent<Hazard>());
                }
                else
				{
                    SetMaxHatsToStealBasedOnType(collision.gameObject.GetComponent<Hazard>());
                }
			}
		}
	}

	private void SetMaxHatsToStealBasedOnType(Hazard hazardObject)
	{
        switch(hazardObject.typeOfHatStealChosen)
		{
            case Hazard.TypesOfHatSteal.None:
                break;

			case Hazard.TypesOfHatSteal.Some:
                StealHat(hazardObject.maxHatsToSteal, hazardObject.typeOfHatStealChosen);
                break;

            case Hazard.TypesOfHatSteal.All:
                hazardObject.maxHatsToSteal = playerHatCollecter.hatCount;
                StealHat(hazardObject.maxHatsToSteal, hazardObject.typeOfHatStealChosen);
                break;
        }
		
	}

	private void StealHat(int numberOfHats, Hazard.TypesOfHatSteal typeOfHatStealChosen)
	{
        for(int i=0;i<numberOfHats; i++)
		{
            if(playerHatCollecter.hatStack.Count == 0)
			{
                return;
			}
            Destroy(playerHatCollecter.hatStack.Pop());
            playerHatCollecter.hatCount--;
            
        }
        playerHatCollecter.updateCollecter();
    }

	IEnumerator KnockbackPlayer(Hazard hazardObject)
	{
        playerMovement.canMove = false;
        Vector3 directionOfKnockback = -transform.forward;
        directionOfKnockback = SelectRandomHorizontalDirection(directionOfKnockback);
        playerRigidBody.AddForce(directionOfKnockback * hazardObject.knockBackForceAmount, ForceMode.Impulse);
        yield return new WaitForSecondsRealtime(hazardObject.KnockBackTime);
        playerMovement.canMove = true;
    }


    //This function is there to deal diagonal knockback even if the player is moving in a straight direction
	private Vector3 SelectRandomHorizontalDirection(Vector3 directionOfKnockback)
	{
		if (transform.forward.x == 0)
		{
			int randDirectionSelection = Random.Range(0, 2); //Chooses either 0 or 1 to represent left and right respectively
			if (randDirectionSelection == 0)
			{
				directionOfKnockback = new Vector3(-1, 0, -transform.forward.z);
			}
			else if (randDirectionSelection == 1)
			{
				directionOfKnockback = new Vector3(1, 0, -transform.forward.z);
			}
		}
		return directionOfKnockback;
	}

    [Header("IFrames variables")] 
    public float FlashingTime = .2f;
    public float TimeInterval = .1f;

    void OnDamageEnable()
    {
        AudioManager.PlayPlayerAudioClip(PlayerAudioStates.Damaged, playerAudioSource);
        stealHatIFrame.TimerStart = true;
        StartCoroutine(Flash(FlashingTime, TimeInterval));
    }

    IEnumerator Flash(float time, float intervalTime)
    {
        //initialize timer
        float elapsedTime = 0f;
        GetComponentInChildren<HatCollecter>().isdamaged = true;
        gameObject.layer = LayerMask.NameToLayer(iFramesLayerName);
        while (elapsedTime < time)
        {
            Renderer[] RendererArray = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in RendererArray)
            {
                if (!r) continue;
                r.enabled = false;
            }
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(intervalTime);
            foreach (Renderer r in RendererArray)
            {
                if (!r) continue;
                r.enabled = true;
            }
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(intervalTime);
        }
        gameObject.layer = LayerMask.NameToLayer(defaultLayerName);
        GetComponentInChildren<HatCollecter>().isdamaged = false;
    }

    public void SuperBounce()
	{
        superBump = true;
        playerRigidBody.mass = superBounceMass;

        superBumpTimer = gameObject.AddComponent<NewTimer>();
        superBumpTimer.MaxTime = superBumpMaxTime;
        superBumpTimer.TimerStart = true;
    }
}
