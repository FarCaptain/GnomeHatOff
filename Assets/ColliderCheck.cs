using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    public PlayerMovement player;
    public BoxCollider hatCollector = null;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Knockback"))
        {
            if (!player.isDrop)
                player.knocked = true;

        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Ground") || other.tag.Equals("Knockback"))
        {
            player.isDrop = false;
            player.collisionTime += 0.01f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Ground"))
        {


        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.hasSeal)
        {
            GetComponent<BoxCollider>().isTrigger = false;
            // Scale the collider into seal size
            hatCollector.transform.localScale = new Vector3(1.5f, 1f, 4f);
        }
        else
        {
            GetComponent<BoxCollider>().isTrigger = true;
            if (hatCollector != null)
            {
                hatCollector.transform.localScale = new Vector3(1f, 1f, 1f);
            }
           
        }
    }

}
