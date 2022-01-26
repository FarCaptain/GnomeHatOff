using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatFade : MonoBehaviour
{

    public float defaultTransparency = 1f;
    public float fadeDuration = 3f;
    //public float destroyHeight = 10f;
    public GameObject[] players;

    float currentTransparency;
    float toFadeTo;
    float tempDist;
    bool isFadingUp;
    bool isFadingDown;
    float headTop;

    void Start()
    {
        currentTransparency = defaultTransparency;
        ApplyTransparency();

        for (int i = 0; i < players[0].transform.childCount; i++)
        {
            // inefficient, way to do on resources?
            if (players[0].transform.GetChild(i).name == "HeadTop")
            {
                headTop = players[0].transform.GetChild(i).transform.position.y;
                break;
            }
        }
    }

    void FixedUpdate()
    {
        //if (players.Length > 0)// && transform.localPosition.y < players[0].transform.localPosition.y
        //{
        //    for(int i = 0; i < players.Length; i ++)
        //        Physics.IgnoreCollision(GetComponent<MeshCollider>(), players[i].GetComponent<BoxCollider>());

        //}
        
        if(players.Length > 0 && transform.position.y < headTop)
        {
            Destroy(GetComponent<Rigidbody>());
            FadeT(0.0f);
            Destroy(gameObject, 3f);
        }
            

        if (isFadingUp)
        {
            if (currentTransparency < toFadeTo)
            {
                currentTransparency += (tempDist / fadeDuration) * Time.deltaTime;
                ApplyTransparency();
            }
            else
            {
                isFadingUp = false;
            }
        }
        else if (isFadingDown)
        {
            if (currentTransparency > toFadeTo)
            {
                currentTransparency -= (tempDist / fadeDuration) * Time.deltaTime;
                ApplyTransparency();
            }
            else
            {
                isFadingDown = false;
            }
        }
    }

    void ApplyTransparency()
    {
        transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color *= new Color(1.0f, 1.0f, 1.0f, currentTransparency);
    }

    public void SetT(float newT)
    {
        currentTransparency = newT;
        ApplyTransparency();
    }
    public void FadeT(float newT)
    {
        toFadeTo = newT;
        if (currentTransparency < toFadeTo)
        {
            tempDist = toFadeTo - currentTransparency;
            isFadingUp = true;
        }
        else
        {
            tempDist = currentTransparency - toFadeTo;
            isFadingDown = true;
        }
    }
}