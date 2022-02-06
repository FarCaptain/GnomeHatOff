#define KEYBOARD
//else use alt controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController controller;
    public float MinthresholdFB = 2f;
    public float MinthresholdLR = 2f;

    public float MaxthresholdFB = 4f;
    public float MaxthresholdLR = 4f;

    public float maxSpeed;
    public float minSpeed;

    // to filter the value we get from the gyroscope
    public int delayedFrames;
    private int remainingFrames;
    private float prevLRSign;
    private Rigidbody rigidBody;

    public ParticleSystem runDust;

    public Vector3 shownSpeed;

#if KEYBOARD
    public float keyboardSpeed;
#else
    public float xval = ArduinoReceiver.xaxis;
    public float zval = ArduinoReceiver.zaxis;
#endif

    Vector2 initPos;        // new default position for controller when calibrated
    bool ifInit;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

#if KEYBOARD
        ifInit = true;
#else
        //speed = 5f;
        ifInit = false;

        remainingFrames = delayedFrames;
#endif
    }

    void FixedUpdate()
    {
#if KEYBOARD
#else
        // used for calibration
        if (Input.GetKey(KeyCode.Q))
        {
            initPos = new Vector2(ArduinoReceiver.xaxis, ArduinoReceiver.zaxis);
            ifInit = true;
            prevLRSign = 0;
        }
#endif

        if (ifInit)
        {
#if KEYBOARD
            Vector3 move = Vector3.zero;

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");
                move = new Vector3(x, 0f, z);
            }

            Vector3 speed = move * keyboardSpeed;
#else

            xval = ArduinoReceiver.xaxis - initPos.x;
            zval = ArduinoReceiver.zaxis - initPos.y;

            Vector3 move = new Vector3(xval, 0f, zval);

            // adjusts speed according to our thresholds for an acceleration effect
            float speed_x, speed_z;
            if (Mathf.Abs(move.x) <= MinthresholdLR)
                speed_x = 0f;
            else if (Mathf.Abs(move.x) >= MaxthresholdLR)
                speed_x = maxSpeed;
            else 
                speed_x = map(Mathf.Abs(move.x), MinthresholdLR, MaxthresholdLR, minSpeed, maxSpeed);

            if (Mathf.Abs(move.z) <= MinthresholdFB)
                speed_z = 0f;
            else if (Mathf.Abs(move.z) >= MaxthresholdFB)
                speed_z = maxSpeed;
            else
                speed_z = map(Mathf.Abs(move.z), MinthresholdFB, MaxthresholdFB, minSpeed, maxSpeed);

            // assigns direction
            speed_x *= Mathf.Sign(move.x);
            speed_z *= Mathf.Sign(move.z);
            Vector3 speed = new Vector3(speed_x, 0f, speed_z);
#endif

            if (speed != Vector3.zero)
            {
                gameObject.transform.forward = speed;
                drawRunDust( );
            }

            Move(speed * Time.deltaTime);
        }
        // keeps object from flying off (might be removable)
        Vector3 pos = gameObject.transform.position;
        gameObject.transform.position = new Vector3(pos.x, 0.1f, pos.z);
    }

    private void Move(Vector3 motion)
    {
        rigidBody.velocity = motion;
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
}
