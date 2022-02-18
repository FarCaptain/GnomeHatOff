using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Knockback Variables")]
    [SerializeField] int knockBackForceAmount = 10;
    private float waitTimeBeforeMoving = 0.5f;

    [Header("Damage Variables")]
    [SerializeField] float iFrameMaxTime = 2f;

    enum TypesOfHatSteal {None, Some, All};
    [Header("Hat Steal Variables")]
    [SerializeField] TypesOfHatSteal typeOfHatStealChosen = TypesOfHatSteal.Some;
    [SerializeField] int maxHatsToSteal = 0;

    //Cached Player Components
    Rigidbody playerRigidBody;
    PlayerMovement playerMovement;
    HatCollecter playerHatCollecter;
    NewTimer stealHatIFrame;
    AudioSource playerAudioSource;
    void Start()
    {
        playerRigidBody = gameObject.GetComponent<Rigidbody>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerHatCollecter = gameObject.GetComponentInChildren<HatCollecter>();
        playerAudioSource = GetComponent<AudioSource>();
 
        stealHatIFrame = gameObject.AddComponent<NewTimer>();
        stealHatIFrame.MaxTime = iFrameMaxTime;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        /// Guard Statment
        /// Always make sure player can move before applying any debuff on them
        if(playerMovement.canMove == false)
		{
            return;
		}
        if (collision.gameObject.tag.Contains("Knockback"))
        {
            StartCoroutine(KnockbackPlayer(collision.gameObject));
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
				SetMaxHatsToStealBasedOnType();
			}
		}

    
    }

	private void SetMaxHatsToStealBasedOnType()
	{
        switch(typeOfHatStealChosen)
		{
            case TypesOfHatSteal.None:
                break;

			case TypesOfHatSteal.Some:
                StealHat(maxHatsToSteal, typeOfHatStealChosen);
                break;

            case TypesOfHatSteal.All:
                maxHatsToSteal = playerHatCollecter.hatCount;
                StealHat(maxHatsToSteal, typeOfHatStealChosen);
                break;
        }
		
	}

	private void StealHat(int numberOfHats,TypesOfHatSteal typeOfHatStealChosen)
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

	IEnumerator KnockbackPlayer(GameObject objectCausingKnockback)
	{
        playerMovement.canMove = false;

        Vector3 directionOfKnockback = -transform.forward;
		directionOfKnockback = SelectRandomHorizontalDirection(directionOfKnockback);

        playerRigidBody.AddForce(directionOfKnockback*knockBackForceAmount, ForceMode.Impulse);

        yield return new WaitForSecondsRealtime(waitTimeBeforeMoving); // Stops player from overriding the force by moving
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
        while (elapsedTime < time)
        {
            Physics.IgnoreLayerCollision(9, 10);
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
        Physics.IgnoreLayerCollision(9, 10,false);
        GetComponentInChildren<HatCollecter>().isdamaged = false;
    }
}
