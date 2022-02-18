﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatCollecter : MonoBehaviour
{
    public GameObject gnome;
    public int hatCount;
    public ScoreSystem scoreSystem;
    public ParticleSystem sparks;

    public float gap = 0.1f; // the height difference between hats
    
    public GameObject hatTop;
    public float initHatHeight;
    public float headTop;
    
    public Vector3 initColliderSize;
    public Vector3 initColliderCenter;
    public bool hatdrop = false;

    public Stack<GameObject> hatStack = new Stack<GameObject>();
    public bool isdamaged = false;

    private void Start()
    {
        hatdrop = false;
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
        //Debug.Log(hatdrop);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hat" && isdamaged == false)
        {
            if (other.gameObject.transform.position.y > (headTop + 0.01f))
            {
                //print("Yeay! Hat Collected!" + (++count));
                hatCount += 1;
                AudioManager.PlayHatAudioClip(HatAudioStates.Collected, gameObject.GetComponentInParent<AudioSource>());
                Vector3 hatPos = hatTop.transform.position;
                hatPos = new Vector3(hatPos.x, hatPos.y + gap, hatPos.z);
                Destroy(other.gameObject.GetComponent<Rigidbody>());
                //other.gameObject.GetComponentInChildren<MeshRenderer>().material

                GameObject hat = other.gameObject;
                //hat.gameObject.GetInstanceID
                hatStack.Push(hat);

                hat.transform.parent = gnome.transform;
                hat.gameObject.transform.position = hatPos;
                hatTop.transform.position = hatPos;

                //decrease speed of Gnome
                PlayerMovement movement = GetComponentInParent<PlayerMovement>();
                movement.hatBurden = movement.speedDecreaseEachHat * hatCount;

                hat.GetComponent<HatFade>().hatShadowDestroy();
            }
        }
        if (other.tag == "Mushroom")
        {
            if (other.gameObject.transform.position.y > (headTop + 0.01f))
            {
                Debug.Log("Catch in the sky");
                hatCount += 5;
                AudioManager.PlayHatAudioClip(HatAudioStates.Collected, gameObject.GetComponentInParent<AudioSource>());

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

                other.gameObject.GetComponent<MushroomController>().hatShadowDestroy();
            }
            else
            {
                Debug.Log("Catch on the ground");

                AudioManager.PlayHatAudioClip(HatAudioStates.Collected, gameObject.GetComponentInParent<AudioSource>());

                other.gameObject.GetComponent<MushroomController>().hatShadowDestroy();
                scoreSystem.AddScore(gnome.GetComponent<PlayerMovement>().playerIndex, 5);
                Destroy(other.gameObject);

            }
        }
    }
    private void drawSparks()
    {
        sparks.Play();
    }

    public void updateCollecter()
    {
        BoxCollider bc = GetComponent<BoxCollider>();
        Vector3 size = bc.size;
        Vector3 center = bc.center;
        int hatCount = hatStack.Count;

        Vector3 hatPos = hatTop.transform.position;
        hatTop.transform.position = new Vector3(hatPos.x, initHatHeight, hatPos.z);

        // the scale of the collider is different from the hat model
        float param = 0.2f;
        bc.size = new Vector3(size.x, initColliderSize.y + gap * param * hatCount, size.z);
        bc.center = new Vector3(center.x, initColliderCenter.y + 0.5f * gap * param * hatCount, center.z);
    }
}
