using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    private int health = 3;

    [SerializeField]
    private Material halfCrack;
    [SerializeField]
    private Material fullCrack;

    bool isSink;
    Animation animation;
    GameObject go_Ice;
    bool isInitialized = false;
    Material iceMat;
    GameObject go_IceMat;
    // Start is called before the first frame update
    void Start()
    {
        isSink = false;
        go_IceMat = transform.Find("Ice/pasted__group7/pasted__pasted__pCylinder2/polySurface11").gameObject; 
       
        animation = GetComponent<Animation>();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Knockback"))
        {
            Debug.Log("HIT");
            if (!collision.gameObject.GetComponent<Snowball>().Hit)
            {
                Crack();
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
              //  Up();
            }

        }
    }
    public void Crack()
    {
        health--;

        switch (health)
        {
            case 2:
                GetComponentInChildren<MeshRenderer>().material = halfCrack;
                Invoke("Crack", 1);
                break;
            case 1:
                GetComponentInChildren<MeshRenderer>().material = fullCrack;
                Invoke("Crack", 1);
                break;
            case 0:
                DestroyGameObject();
                break;
        }


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
