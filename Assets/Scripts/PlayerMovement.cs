//#define KEYBOARD
//else use alt controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed;

#if KEYBOARD
#else
    public float xval = ArduinoReceiver.xaxis;
    public float zval = ArduinoReceiver.zaxis;
#endif

    Vector2 initPos;
    bool ifInit;

    void Start()
    {
    #if KEYBOARD
        speed = 12f;
        ifInit = true;
    #else
        speed = 0.35f;
        ifInit = false;
    #endif
    }

    void Update()
    {
        #if KEYBOARD
        #else
                if(Input.GetKey(KeyCode.Q))
                {
                    initPos = new Vector2(ArduinoReceiver.xaxis, ArduinoReceiver.zaxis);
                    ifInit = true;
                }
        #endif

        if (ifInit)
        {
#if KEYBOARD
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = new Vector3(x, 0f, z);
#else

            xval = ArduinoReceiver.xaxis - initPos.x;
            zval = ArduinoReceiver.zaxis - initPos.y;

            Vector3 move = new Vector3(ArduinoReceiver.xaxis - initPos.x, 0f, ArduinoReceiver.zaxis - initPos.y);
            move.x = (Mathf.Abs(move.x) > 3f) ? Mathf.Sign(move.x) * 12f : 0f;
            move.z = (Mathf.Abs(move.z) > 3f) ? Mathf.Sign(move.z) * 12f : 0f;
#endif
            controller.Move(move * speed * Time.deltaTime);
        }
    }
}
