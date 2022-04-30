using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatspawnmodifier : MonoBehaviour
{
    float timer;
    bool setshrink = true;
    public float HatAreaShrinkTime;
    public Vector3 HatShrinkArea;
    public HatSpawning HS;

    // Start is called before the first frame update
    void Start()
    {
        HS = FindObjectOfType<HatSpawning>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > HatAreaShrinkTime && setshrink)
        {
            HS.size = HatShrinkArea;
            setshrink = false;
        }


    }
}
