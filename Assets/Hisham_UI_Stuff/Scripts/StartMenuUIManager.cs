using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuUIManager : MonoBehaviour
{
    [SerializeField] GameObject fader;
    void Start()
    {
        fader.GetComponent<Animator>().SetBool("isFadingIn", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
