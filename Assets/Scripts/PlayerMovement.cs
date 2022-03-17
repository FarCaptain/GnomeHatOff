//#define KEYBOARD
//else use alt controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public bool canSlide;
    public float MinthresholdFB = 2f;
    public float MinthresholdLR = 2f;

    public float MaxthresholdFB = 4f;
    public float MaxthresholdLR = 4f;

    public float slideFactor;
    public float maxSpeed;
    public float minSpeed;
    [Header("The decrease of max speed each hat gives you")]
    public float hatBurden = 0;
    public float speedDecreaseEachHat;

    public int playerIndex = 0;
    
    // to filter the value we get from the gyroscope
    public int delayedFrames;
    private int remainingFrames;
  
    private Rigidbody rigidBody;

    public ParticleSystem runDust;

    public Vector3 shownSpeed;


    public float keyboardSpeed;

    public float xval;



    Vector3 initPos = new Vector3(0,0,0);       // new default position for controller when calibrated
    
    [HideInInspector]
    public bool canMove = true;

    bool isDrop;
    Vector3 speed;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        isDrop = false;



    }

    void FixedUpdate()
    {

        // used for calibration

        float vel_y = rigidBody.velocity.y;

        if (vel_y < -0.001&&!isDrop)
        {
            isDrop = true;
           
        }
        Vector3 move = Vector3.zero;
        if (playerIndex == 0)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {


                float x = Input.GetAxis("Horizontal1");
                float z = Input.GetAxis("Vertical1");

                move = new Vector3(x, 0f, z);
                speed = move * keyboardSpeed;

            }
        }

        else if (playerIndex == 1)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");
                move = new Vector3(x, 0f, z);
                speed = move * keyboardSpeed;
            }
        }


        if (isDrop)
        {
            speed.y = -200f;
        }
        
        if (speed != Vector3.zero && canMove == true)
        {
            if(speed.x!=0||speed.y!=0)
            {
                gameObject.transform.forward = new Vector3(speed.x, 0, speed.z);
               
            }
            
            Move(speed * Time.deltaTime);
            drawRunDust();
        }



        // keeps object from flying off (might be removable)
        Vector3 pos = gameObject.transform.position;
      
    }

    private void Move(Vector3 motion)
    {
        if (canSlide)
            rigidBody.AddForce(motion * slideFactor);
        else
            rigidBody.velocity = motion;
    }

    public void Move(float xaxis, float yaxis,float zaxis)
    {
        if (canMove == false)
        {
            return;
        }
        float xval = xaxis - initPos.x;
        float yval = yaxis - initPos.y;
        float zval = zaxis - initPos.y;

        Vector3 move = new Vector3(xval, 0f, zval);

        float currentMaxSpeed = maxSpeed;
        if (maxSpeed - hatBurden >= minSpeed)
            currentMaxSpeed = maxSpeed - hatBurden;

        // adjusts speed according to our thresholds for an acceleration effect
        float speed_x, speed_z;
        if (Mathf.Abs(move.x) <= MinthresholdLR)
            speed_x = 0f;
        else if (Mathf.Abs(move.x) >= MaxthresholdLR)
            speed_x = currentMaxSpeed;
        else
            speed_x = map(Mathf.Abs(move.x), MinthresholdLR, MaxthresholdLR, minSpeed, currentMaxSpeed);

        if (Mathf.Abs(move.z) <= MinthresholdFB)
            speed_z = 0f;
        else if (Mathf.Abs(move.z) >= MaxthresholdFB)
            speed_z = currentMaxSpeed;
        else
            speed_z = map(Mathf.Abs(move.z), MinthresholdFB, MaxthresholdFB, minSpeed, currentMaxSpeed);

        // assigns direction
        speed_x *= Mathf.Sign(move.x);
        speed_z *= Mathf.Sign(move.z);
        speed = new Vector3(speed_x, 0f, speed_z);
        //Debug.Log(speed);


      
       // Vector3 pos = gameObject.transform.position;
        //Debug.Log(pos);
       // gameObject.transform.position = new Vector3(pos.x, 0.1f, pos.z);
    }

    private void drawRunDust()
    {
        if (!runDust.isPlaying)
        {
            runDust.Play();
        }
    }

    //map changes the range of a1,a2 to b1,b1
    private float map(float s, float a1, float a2, float b1, float b2)
    {
        return (s - a1) * (b2 - b1) / (a2 - a1) + b1;
    }

    public void SetOffset(Vector3 i_offset)
    {
        initPos = i_offset;
    }

    public void jumpset(bool value)
    {

        //GetComponentInChildren<HatCollecter>().hatdrop = value;
    }

}
