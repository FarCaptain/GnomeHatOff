//#define KEYBOARD
//else use alt controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController controller;
    public float MinthresholdFB = 2f;
    public float MinthresholdLR = 2f;

    public float MaxthresholdFB = 4f;
    public float MaxthresholdLR = 4f;

    public float maxSpeed;
    public float minSpeed;

    //TODO. to filter the value we get from the gyroscope

    private Rigidbody rigidBody;

    public ParticleSystem runDust;

    //private Rigidbody rigidBody;

#if KEYBOARD
#else
    public float xval = ArduinoReceiver1.xaxis;
    public float zval = ArduinoReceiver1.zaxis;
#endif

    Vector2 initPos;
    bool ifInit;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

    #if KEYBOARD
        //speed = 12f;
        ifInit = true;
#else
        //speed = 5f;
        ifInit = false;

#endif
    }

    void FixedUpdate()
    {
#if KEYBOARD
#else
        if (Input.GetKey(KeyCode.Q))
        {
            initPos = new Vector2(ArduinoReceiver1.xaxis, ArduinoReceiver1.zaxis);
            ifInit = true;
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
#else

            xval = ArduinoReceiver1.xaxis - initPos.x;
            zval = ArduinoReceiver1.zaxis - initPos.y;

            Vector3 move = new Vector3(xval, 0f, zval);

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

            speed_x *= Mathf.Sign(move.x);
            speed_z *= Mathf.Sign(move.z);
#endif

            Vector3 speed = new Vector3(speed_x, 0f, speed_z);
            if (speed != Vector3.zero)
            {
                gameObject.transform.forward = speed;
                drawRunDust();
            }
            
            Move(speed * Time.deltaTime);
        }
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
