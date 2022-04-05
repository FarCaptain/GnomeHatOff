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
    int stage;
    GameObject go_IceMat;
    // Start is called before the first frame update
    void Start()
    {
      
        health = 2;


        go_IceMat = transform.Find("Ice/pasted__group7/pasted__pasted__pCylinder2/polySurface11").gameObject;
    
        animation = GetComponent<Animation>();
       
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Knockback"))
        {

            if (!collision.gameObject.GetComponent<Snowball>().Hit)
            {
                if (health > 1)
                {
                    health = 1;
                    go_IceMat.GetComponent<MeshRenderer>().material = meltMaterials[1];
                }
               

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag.Equals("PlayerCollider"))
        {
            if (health == 1)
            {
                Invoke("DestroyGameObject", 2);
                health = 0;
                go_IceMat.GetComponent<MeshRenderer>().material = meltMaterials[2];
            }



        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag.Equals("PlayerCollider"))
        {
            if (health == 1)
            {
                health = 0;
                go_IceMat.GetComponent<MeshRenderer>().material = meltMaterials[2];
            }
           

        }
    }

   


    public void Melt()
    {
        float waitTime = UnityEngine.Random.Range(0, 1.5f);
        Invoke("PlayMeltAnimation", waitTime);

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