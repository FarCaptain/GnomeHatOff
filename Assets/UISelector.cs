using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelector : MonoBehaviour
{
    GameObject objectHit;
    void Start()
    {
       
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Ray playerRay = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(playerRay.origin, Vector3.down, Color.red, 10f);
        Physics.Raycast(playerRay, out hit, 100f);
        if (hit.collider!=null)
		{
            if(hit.collider.gameObject.GetComponent<ButtonHandler>())
			{
                hit.collider.gameObject.GetComponent<ButtonHandler>().playerOver=true;
                objectHit = hit.collider.gameObject;
                if (!objectHit.GetComponent<ButtonHandler>().UISelectors.Contains(this))
				{
                    objectHit.GetComponent<ButtonHandler>().UISelectors.Add(this);
                }
            }
        }
        else
		{
            if(objectHit!=null)
			{
                if (objectHit.GetComponent<ButtonHandler>().UISelectors.Contains(this))
                {
                    objectHit.GetComponent<ButtonHandler>().UISelectors.Remove(this);
                }
                objectHit.GetComponent<ButtonHandler>().playerOver = false;
            }
        }
    }
}
