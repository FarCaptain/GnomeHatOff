using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatCollecter : MonoBehaviour
{
    public GameObject gnome;
    public AudioClip collectSound;
    public int hatCount;

    const float gap = 0.06f; // the height difference between hats
    GameObject hatTop;
    float initHatHeight;
    float headTop;

    Vector3 initColliderSize;
    Vector3 initColliderCenter;

    private void Start()
    {
        for (int i = 0; i < gnome.transform.childCount; i++)
        {
            if (gnome.transform.GetChild(i).name == "HatTop")
                hatTop = gnome.transform.GetChild(i).gameObject;
            else if (gnome.transform.GetChild(i).name == "HeadTop")
                headTop = gnome.transform.GetChild(i).position.y;
        }
        initHatHeight = hatTop.transform.position.y;
        initColliderSize = GetComponent<BoxCollider>().size;
        initColliderCenter = GetComponent<BoxCollider>().center;
    }

    private void Update()
    {
        if (hatTop.transform.position.y != initHatHeight)
        {
            if (gnome.name == "Gnome" && ScoreSystem.hatCount0 == 0 || (gnome.name == "Gnome(1)" && ScoreSystem.hatCount1 == 0))
            {
                Vector3 hatPos = hatTop.transform.position;
                hatTop.transform.position = new Vector3(hatPos.x, initHatHeight, hatPos.z);

                GetComponent<BoxCollider>().size = initColliderSize;
                GetComponent<BoxCollider>().center = initColliderCenter;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hat")
        {
            if (other.gameObject.transform.position.y > (headTop + 0.01f))
            {
                //print("Yeay! Hat Collected!" + (++count));
                hatCount += 1;
                gameObject.GetComponent<AudioSource>().PlayOneShot(collectSound);

                if (gnome.name == "Gnome")
                    ScoreSystem.hatCount0 += 1;
                else
                    ScoreSystem.hatCount1 += 1;

                Vector3 hatPos = hatTop.transform.position;
                hatPos = new Vector3(hatPos.x, hatPos.y + gap, hatPos.z);
                Destroy(other.gameObject.GetComponent<Rigidbody>());
                //other.gameObject.GetComponentInChildren<MeshRenderer>().material

                other.transform.parent = gnome.transform;
                other.gameObject.transform.position = hatPos;
                hatTop.transform.position = hatPos;

                // expand collider
                BoxCollider bc = GetComponent<BoxCollider>();
                Vector3 size = bc.size;
                Vector3 center = bc.center;

                float param = 0.2f;
                bc.size = new Vector3(size.x, size.y + gap * param, size.z);
                bc.center = new Vector3(center.x, center.y + 0.5f * gap * param, center.z);
            }
        }
    }
}
