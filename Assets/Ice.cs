using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    bool isSink;
    Animation animation;
    GameObject go_Ice;
    bool isInitialized = false;
    // Start is called before the first frame update
    void Start()
    {
        isSink = false;

        go_Ice = transform.Find("Ice").gameObject;
        Debug.Log("ice" + go_Ice);
        animation = GetComponent<Animation>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag.Equals("Player"))
        {
            if (go_Ice != null)
            {
                Down();
            }
          
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag.Equals("Player"))
        {
            if (go_Ice != null)
            {
                Up();
            }

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
        Invoke("DestroyGameObject", 2);
        
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
