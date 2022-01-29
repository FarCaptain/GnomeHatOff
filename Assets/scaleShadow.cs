using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scaleShadow : MonoBehaviour
{
    public float scaleSpeed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0, 0, 0); 
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x<1)
        {
            transform.localScale = transform.localScale + new Vector3(1, 1, 1) * scaleSpeed * Time.deltaTime;
        }
    }
}
