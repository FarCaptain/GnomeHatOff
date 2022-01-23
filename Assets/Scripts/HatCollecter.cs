using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatCollecter : MonoBehaviour
{
    int count = 0;
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hat")
        {
            if(other.gameObject.transform.position.y > gameObject.transform.position.y)
                print("Yeay! Hat Collected!" + (++count));
            
            Destroy(other.gameObject);
        }
    }
}
