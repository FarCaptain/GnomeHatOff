using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Man : MonoBehaviour
{
    // Start is called before the first frame update
    public float accelerationTime = 2f;
    public float maxSpeed = 5f;
    private Vector3 movement;
    private float timeLeft;
    [SerializeField]public Rigidbody rb;
    bool ground;
   

    void Update()
    {
        
        if(ground==true)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                movement = new Vector3(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f));
                timeLeft = accelerationTime;
            }

        }
    }

    void FixedUpdate()
    {
        rb.AddForce(movement * maxSpeed);
    }
}
