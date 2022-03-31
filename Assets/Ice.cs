using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ice : MonoBehaviour
{
    private float health;

    [SerializeField]
    private Material[] meltMaterials;
    int stageNum;

    public float meltTime;
    float playerOnMeltTime;
    bool hasPlayerOn;
    bool isSink;
    bool isHit;
    Animation animation;
    GameObject go_Ice;
    bool isInitialized = false;
    Material iceMat;
    GameObject go_IceMat;
    // Start is called before the first frame update
    void Start()
    {
        stageNum = meltMaterials.Length;
        health = 0;
        isSink = false;
        hasPlayerOn = false;
        go_IceMat = transform.Find("Ice/pasted__group7/pasted__pasted__pCylinder2/polySurface11").gameObject;
        playerOnMeltTime = 0;
         animation = GetComponent<Animation>();
        
    }

    private void FixedUpdate()
    {
        if (isHit&&hasPlayerOn)
        {
            health -= 2*Time.fixedDeltaTime;
        }else if (hasPlayerOn && !isHit)
        {
            health -= Time.fixedDeltaTime;
        }
        else if (!hasPlayerOn && isHit)
        {
            health -= Time.fixedDeltaTime;
        }
        else
        {
            health += Time.fixedDeltaTime;
        }

        //int stage = (int)Math.Floor(health);
        //if (health == stageNum)
        //{
        //    DestroyGameObject();
        //    return;
        //}


        //go_IceMat.GetComponent<MeshRenderer>().material = meltMaterials[stage];
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Knockback"))
        {
            
            if (!collision.gameObject.GetComponent<Snowball>().Hit)
            {
                //Crack();
                collision.gameObject.GetComponent<Snowball>().Hit = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag.Equals("Player"))
        {
            if (go_Ice != null)
            {
                hasPlayerOn = true;
               // Down();
            }
          
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag.Equals("Player"))
        {
            if (go_Ice != null)
            {
                hasPlayerOn = true;
                playerOnMeltTime += 0.01f;
                // Down();
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag.Equals("Player"))
        {
            if (go_Ice != null)
            {
                hasPlayerOn = false;
                playerOnMeltTime = 0;
                //  Up();
            }

        }
    }
    public void Crack()
    {
        if (health == stageNum)
        {
            DestroyGameObject();
            return;
        }
            
        
        go_IceMat.GetComponent<MeshRenderer>().material = meltMaterials[health];
        health++;
        Invoke("Crack", 1);
     




    }

   
    public void Melt()
    {
        float waitTime = Random.Range(0, 1.5f);
        Invoke("PlayMeltAnimation",waitTime);
        
    }
    public void Down()
    {
        if (!isSink)
        {
            go_Ice.transform.localPosition = new Vector3(0,-0.1f,0);
            animation.Play("IceDown");
            isSink = true;
        }
    }
    public void Up()
    {
        if (isSink)
        {
            go_Ice.transform.localPosition = new Vector3(0, 0, 0);
            animation.Play("IceUp");
            isSink = false;
        }
    }
    void PlayMeltAnimation()
    {
        
        animation.Play("IceMelting");
        Invoke("DestroyGameObject", 3.5f);
        
    }

    // TODO: Possible shatter animation
    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
