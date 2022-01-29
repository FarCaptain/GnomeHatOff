using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatCollecter : MonoBehaviour
{
    public GameObject gnome;

    int count = 0;
    const float gap = 0.06f; // the height difference between hats
    GameObject hatTop;
    float headTop;
    bool flag = true;

    private void Start()
    {
        for (int i = 0; i < gnome.transform.childCount; i++)
        {
            if (gnome.transform.GetChild(i).name == "HatTop")
                hatTop = gnome.transform.GetChild(i).gameObject;
            else if (gnome.transform.GetChild(i).name == "HeadTop")
                headTop = gnome.transform.GetChild(i).position.y;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hat")
        {
            if (other.gameObject.transform.position.y > (headTop + 0.01f))
            {
                print("Yeay! Hat Collected!" + (++count));

                Vector3 hatPos = hatTop.transform.position;
                hatPos = new Vector3(hatPos.x, hatPos.y + gap, hatPos.z);
                Destroy(other.gameObject.GetComponent<Rigidbody>());
                other.transform.parent = gnome.transform;
                other.gameObject.transform.position = hatPos;
                hatTop.transform.position = hatPos;
            }
        }
    }
}
