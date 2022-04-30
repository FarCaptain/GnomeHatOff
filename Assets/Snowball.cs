using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    public bool Hit;
    float bornTime;
    float hitTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        Hit = false;
        bornTime = 0;
    }

   
    public void AddForce(Vector3 force)
    {
        GetComponent<Rigidbody>().AddForce(force);
        GetComponent<Rigidbody>().useGravity = true;
    }
    // Update is called once per frame
    void Update()
    {
        bornTime += Time.deltaTime;
        if (Hit)
        {
            hitTime = hitTime==0? bornTime: hitTime;
            transform.localScale = Vector3.one * ((8-bornTime + hitTime) / 8);
        }
        
        if(hitTime!=0 && bornTime - hitTime> 4)
        {
            Destroy(gameObject);
        }
    }
}
