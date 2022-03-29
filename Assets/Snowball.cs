using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    public bool Hit;
    // Start is called before the first frame update
    void Start()
    {
        Hit = false;
        
    }

    public void AddForce(Vector3 force)
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(50, -300, 50));
        GetComponent<Rigidbody>().useGravity = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
