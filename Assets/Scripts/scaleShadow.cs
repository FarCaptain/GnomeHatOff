using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class scaleShadow : MonoBehaviour
{
    public float scaleSpeed = 0.2f;
    bool attraction;
    GameObject player;
    float MagnetismStrength = 4f;
    float MagnetismReductionOverTime = 0.1f;
    float MinimumSnapValue = 0.08f;
    float Timer = 1;
    // Start is called before the first frame update
    void Start()
    {
        attraction = false;
        transform.localScale = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x<1)
        {
            transform.localScale = transform.localScale + new Vector3(1, 1, 1) * scaleSpeed * Time.deltaTime;
        }
        if(player)
        {
            if (attraction == true )
            {
                Vector3 direction = (this.transform.position - player.transform.position).normalized;
                // Debug.Log(direction);
                Vector3 Finaldir = new Vector3(Mathf.Abs(direction.x), Mathf.Abs(direction.y), Mathf.Abs(direction.z));

                player.GetComponent<Rigidbody>().AddForce((Finaldir) * MagnetismStrength, ForceMode.Force) ;
                if(MagnetismStrength > 0)
                MagnetismStrength -= MagnetismReductionOverTime;
                attraction = false;
                /*                if ((direction.x >= -MinimumSnapValue && direction.x <= MinimumSnapValue) || (direction.z >= -MinimumSnapValue && direction.z <= MinimumSnapValue))
                                {
                                    player.transform.position = this.transform.position;
                                    attraction = false;
                                }*/

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        attraction = true;
        if(other.gameObject.tag == "Player")
        { player = other.gameObject; }
    }

    private void OnTriggerExit(Collider other)
    {
        attraction = false;
        player = null;
    }
}
