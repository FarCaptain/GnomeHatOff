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
    public float constantSpeed = 100;
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



    Vector3 initPos = new Vector3(0, 0, 0);       // new default position for controller when calibrated

    [HideInInspector]
    public bool canMove = true;
    public bool isBumping = false;
    int level;
    //for map2
    public GameObject SealSocket;
    public bool isDrop;
    Vector3 speed;
    public float collisionTime;
    float testCollisionTime;
    Vector3 dropSpeed;
    public bool knocked;
    public bool hasSeal;
    [SerializeField]
    Transform respawnPos;
    bool disabled;

    public bool moveInWater;
    public bool moveOnIce;
    void Start()
    {
        
        hasSeal = false;
        disabled = false;
        knocked = false;
        rigidBody = GetComponent<Rigidbody>();
        isDrop = false;
        collisionTime = 1;
        testCollisionTime = 0;
        level = MainGameController.Instance.level;
    }




    void Heal()
    {
        knocked = false;
    }


    void FixedUpdate()
    {
        if (hasSeal)
        {
            moveInWater = true;
            moveOnIce = false;
        }
        if (!isDrop && disabled)
        {
            return;
        }
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (!isDrop && !moveInWater)
        {
            testCollisionTime += Time.fixedDeltaTime;
        }

        if (level == 2 && testCollisionTime > 0.1f)
        {
            if (collisionTime < 0.05 && !moveInWater)
            {
                testCollisionTime = 0;
                isDrop = true;
                Respawn(3);
                dropSpeed = 0.1f * speed + new Vector3(0, -200, 0);
            }
            else
            {
                testCollisionTime = 0;
                collisionTime = 0;
            }
        }

        // used for calibration

        float vel_y = rigidBody.velocity.y;


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

            speed = dropSpeed;

        }

        if (speed != Vector3.zero && canMove == true)
        {
            if (speed.y != -200)
            {
                gameObject.transform.forward = new Vector3(speed.x, 0, speed.z);

            }

            Move(speed * Time.deltaTime);
            drawRunDust();
        }

        if (speed.x == 0 && speed.z == 0 && canSlide == false && canMove == true)
        {
            rigidBody.velocity = Vector3.zero;
        }
    }

    private void Move(Vector3 motion)
    {
        if (canSlide)
            rigidBody.AddForce(motion * slideFactor);
        else
            rigidBody.velocity = motion;
    }

    public void Move(float xaxis, float yaxis, float zaxis)
    {
        if (canMove == false)
        {
            return;
        }
        float xval = xaxis - initPos.x;
        float yval = yaxis - initPos.y;
        float zval = zaxis - initPos.z;

        //inverted inputs to accomodate physical controller position
        Vector3 move = new Vector3(zval, 0f, xval * -1);
        Debug.Log("DebugLog - ReceivingVector: " + move);

        float currentMaxSpeed = maxSpeed;
        if (maxSpeed - hatBurden >= 0)
            currentMaxSpeed = maxSpeed - hatBurden;

        // adjusts speed according to our thresholds for an acceleration effect
        float speed_x, speed_z;
        if (Mathf.Abs(move.x) <= MinthresholdLR)
            speed_x = 0f;
        else if (Mathf.Abs(move.x) >= MaxthresholdLR)
            speed_x = currentMaxSpeed;
        else
            speed_x = constantSpeed;
        //speed_x = map(Mathf.Abs(move.x), MaxthresholdLR);

        if (Mathf.Abs(move.z) <= MinthresholdFB)
            speed_z = 0f;
        else if (Mathf.Abs(move.z) >= MaxthresholdFB)
            speed_z = currentMaxSpeed;
        else
            speed_z = constantSpeed;
        //speed_z = map(Mathf.Abs(move.z), MaxthresholdFB);

        // assigns direction
        speed_x *= Mathf.Sign(move.x);
        speed_z *= Mathf.Sign(move.z);
        speed = new Vector3(speed_x, 0f, speed_z);
        //Debug.Log("DebugLog - Speed: " + speed);

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

    //map arduino values between our thresholds to our maxSpeed
    private float map(float arduinoInput, float threshold)
    {
        float factor = maxSpeed / threshold;
        float mapEquation = arduinoInput * factor;
        //Debug.Log("map value: " + mapVal);
        return mapEquation;
    }

    public void SetOffset(Vector3 i_offset)
    {
        initPos = i_offset;
    }

    public void jumpset(bool value)
    {

        //GetComponentInChildren<HatCollecter>().hatdrop = value;
    }

    //the player will be respawned in time seconds
    public void Respawn(float time)
    {
        disabled = true;
        Invoke("RespawnPlayer", time);
    }

    void RespawnPlayer()
    {

        speed = Vector3.zero;
        transform.position = respawnPos.transform.position;
        disabled = false;
        isDrop = false;
        collisionTime = 1;
    }

}