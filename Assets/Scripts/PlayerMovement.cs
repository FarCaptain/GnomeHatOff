using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 21f;
    public float xval = ArduinoReceiver.xaxis;
    public float zval = ArduinoReceiver.zaxis;

    Vector2 initPos;

    void Start()
    {
        initPos = new Vector2(ArduinoReceiver.xaxis, ArduinoReceiver.zaxis);
    }
    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.Q))
        {
            initPos = new Vector2(ArduinoReceiver.xaxis, ArduinoReceiver.zaxis);
        }
        //float x = Input.GetAxis("Horizontal");
        //float z = Input.GetAxis("Vertical");

        xval = ArduinoReceiver.xaxis;
        zval = ArduinoReceiver.zaxis;

        Vector3 move = new Vector3(ArduinoReceiver.xaxis - initPos.x, 0f, ArduinoReceiver.zaxis - initPos.y);
        move.x = (Mathf.Abs(move.x) > 10f) ? move.x : 0f;
        move.z = (Mathf.Abs(move.z) > 10f) ? move.z : 0f;
        controller.Move(move * speed * Time.deltaTime);
    }
}
