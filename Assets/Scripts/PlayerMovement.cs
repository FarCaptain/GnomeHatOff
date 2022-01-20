using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    Vector2 initPos;

    void Start()
    {
        initPos = new Vector2(ArduinoReceiver.xaxis, ArduinoReceiver.zaxis);
    }
    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.R))
        {
            initPos = new Vector2(ArduinoReceiver.xaxis, ArduinoReceiver.zaxis);
        }
        //float x = Input.GetAxis("Horizontal");
        //float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(ArduinoReceiver.xaxis - initPos.x, 0f, ArduinoReceiver.zaxis - initPos.y);
        controller.Move(move * speed * Time.deltaTime);
    }
}
