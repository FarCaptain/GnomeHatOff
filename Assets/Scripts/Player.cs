using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Knockback Variables")]
    [SerializeField] int knockBackForceAmount = 10;
    [SerializeField] float knockBackForceDistance = 1;
    private float waitTimeBeforeMoving = 0.5f;

    //[SerializeField] float feedbackTime = 2f;
    //private bool isDamaged = false;


    //Cached Player Components
    Rigidbody playerRigidBody;
    PlayerMovement playerMovement;
    HatCollecter playerHatCollecter;

    NewTimer stealHatIFrame;
    void Start()
    {
        playerRigidBody = gameObject.GetComponent<Rigidbody>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerHatCollecter = gameObject.GetComponentInChildren<HatCollecter>();

        stealHatIFrame = gameObject.AddComponent<NewTimer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Knockback" && playerMovement.canMove == true)
        {
            StartCoroutine(KnockbackPlayer(collision.gameObject));
            OnDamageEnable();
            
        }
        if (collision.gameObject.tag == "Damage")
        {
            OnDamageEnable();
        }

        if (collision.gameObject.tag == "StealHat" && playerMovement.canMove == true && stealHatIFrame.TimerStart == false)
        {
            StartCoroutine(KnockbackPlayer(collision.gameObject));
            OnDamageEnable();

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                if (gameObject.transform.GetChild(i).name == "HatPrefab(Clone)")
                {
                    Destroy(playerHatCollecter.hatStack.Pop());
                    playerHatCollecter.hatCount--;
                    playerHatCollecter.updateCollecter();
                    //stealHatIFrame.MaxTime = 2f;
                    //stealHatIFrame.TimerStart = true;
                    break;
                }
            }
            
        }
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

    //void ShowDamageFeedback()
    //   {

    //       if (isDamaged == true)
    //       {

    //       }
    //       else
    //       {
    //           NewTimer iFramesTimer = gameObject.AddComponent<NewTimer>();
    //           iFramesTimer.MaxTime = feedbackTime;
    //           iFramesTimer.TimerStart = true;
    //       }
    //       isDamaged = true;
    //   }






    public float FlashingTime = .2f;
    public float TimeInterval = .1f;

    void OnDamageEnable()
    { 
        StartCoroutine(Flash(FlashingTime, TimeInterval));
    }

    IEnumerator Flash(float time, float intervalTime)
    {
        //initialize timer
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            //Get all renderer in child also
            Renderer[] RendererArray = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in RendererArray)
            r.enabled = false;
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(intervalTime);
            foreach (Renderer r in RendererArray)
            r.enabled = true;
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(intervalTime);
            Debug.Log(elapsedTime);
        }
    }
}
