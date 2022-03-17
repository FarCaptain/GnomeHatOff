using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlane : MonoBehaviour
{
    Vector3 scale;
    public float time;
    public float speed;
    float t = 0;
    // Start is called before the first frame update
    void Start()
    {
        scale = GetComponent<Transform>().localScale;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        GetComponent<Transform>().localScale -= GetComponent<Transform>().localScale*(speed*Time.deltaTime*0.01f);
        if (t > time)
        {
            t = 0;
         //   Shrink();
        }
    }

    void Shrink()
    {
        
        scale = GetComponent<Transform>().localScale;
    }
}
