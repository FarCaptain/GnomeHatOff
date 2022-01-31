#define KEYBOARD
//else use alt controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    public CharacterController controller;
    public float speed;
    public float thresholdFB = 2f;
    public float thresholdLR = 2f;

    public ParticleSystem runDust;

#if KEYBOARD
#else
    public float xval = ArduinoReceiver1.xaxis;
    public float zval = ArduinoReceiver1.zaxis;
#endif

    Vector2 initPos;
    bool ifInit;

    void Start()
    {
    #if KEYBOARD
        speed = 12f;
        ifInit = true;
    #else
        //speed = 5f;
        ifInit = false;
    #endif
    }

    void Update()
    {
        #if KEYBOARD
        #else
                if(Input.GetKey(KeyCode.Q))
                {
                    initPos = new Vector2(ArduinoReceiver1.xaxis, ArduinoReceiver1.zaxis);
                    ifInit = true;
                }
        #endif

        if (ifInit)
        {
#if KEYBOARD
            Vector3 move = Vector3.zero;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                float x = Input.GetAxis("Horizontal1");
                float z = Input.GetAxis("Vertical1");
                move = new Vector3(x, 0f, z);
            }

#else

            xval = ArduinoReceiver1.xaxis - initPos.x;
            zval = ArduinoReceiver1.zaxis - initPos.y;

            Vector3 move = new Vector3(ArduinoReceiver1.xaxis - initPos.x, 0f, ArduinoReceiver1.zaxis - initPos.y);
            move.x = (Mathf.Abs(move.x) > thresholdLR) ? Mathf.Sign(move.x) : 0f;
            move.z = (Mathf.Abs(move.z) > thresholdFB) ? Mathf.Sign(move.z) : 0f;
#endif

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
                drawRunDust();
            }

            float inputSpeed = speed;
            if (move.x != 0f && move.z != 0f)
                inputSpeed *= 0.7071f; // 1/sqrt(2)
            controller.Move(move * inputSpeed * Time.deltaTime);
        }
        Vector3 pos = gameObject.transform.position;
        gameObject.transform.position = new Vector3(pos.x, 0.1f, pos.z);
    }

    private void drawRunDust()
    {
        runDust.Play();
    }
}
