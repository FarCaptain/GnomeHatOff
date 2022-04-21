using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatFade : MonoBehaviour
{
    private AudioSource hatAudioSource;
    public float defaultTransparency = 1f;
    public float fadeDuration = 3f;
    public float groundDuration;
    public GameObject shadowPrefab;

    public ParticleSystem circleDust;
    public float headTop;

    float currentTransparency;
    float toFadeTo;
    float tempDist;
    bool isFadingUp;
    public bool isFadingDown;

    public bool hatFadeEnabled = true;
    public bool hatCollectedByPlayer = false;

    // drop Animation
    private bool dropAnimationOn = false;
    private Vector3 dropAnimationEnd;
    private float dropAnimationTime = 0f;
    public float dropAnimationDuration;
    public float pauseBetweenDropAnimations;

    void Start()
    {
        hatAudioSource = GetComponent<AudioSource>();
        currentTransparency = defaultTransparency;
        ApplyTransparency();
    }

    void FixedUpdate()
    {

        /// Fading
        if (hatFadeEnabled && transform.position.y < headTop)
        {

            StartCoroutine(FadeT(0.0f));
            
            hatShadowDestroy();

            circleDust.Play();
            //AudioManager.PlayHatAudioClip(HatAudioStates.Destroyed, hatAudioSource);
            Destroy(circleDust, 1f);
            hatFadeEnabled = false;
        }

        if (isFadingUp)
        {
            if (currentTransparency < toFadeTo)
            {
                currentTransparency += (tempDist / fadeDuration) * Time.deltaTime;
                ApplyTransparency();
            }
            else
            {
                isFadingUp = false;
            }
        }
        else if (isFadingDown)
        {
            if (currentTransparency > toFadeTo)
            {
                currentTransparency -= (tempDist / fadeDuration) * Time.deltaTime;
                ApplyTransparency();
            }
            else
            {
                isFadingDown = false;
            }
        }

        /// Drop Animation
        if(dropAnimationOn)
        {
            // interpolation
            Vector3 pos = Vector3.Lerp(transform.position, dropAnimationEnd, dropAnimationTime/ dropAnimationDuration);
            dropAnimationTime += Time.deltaTime;

            transform.position = pos;

            // get to the end
            if(dropAnimationTime >= dropAnimationDuration || transform.position == dropAnimationEnd)
            {
                // reset things
                dropAnimationTime = 0f;
                dropAnimationOn = false;
                dropAnimationEnd = Vector3.zero;

                Destroy(gameObject);
            }
        }
    }

    void ApplyTransparency()
    {
        GetComponentInChildren<MeshRenderer>().material.color *= new Color(1.0f, 1.0f, 1.0f, currentTransparency);
    }

    public void SetT(float newT)
    {
        currentTransparency = newT;
        ApplyTransparency();
    }
    public IEnumerator FadeT(float newT)
    {
        yield return new WaitForSeconds(30);

        if (hatCollectedByPlayer == false)
        {
            toFadeTo = newT;
            if (currentTransparency < toFadeTo)
            {
                tempDist = toFadeTo - currentTransparency;
                isFadingUp = true;
            }
            else
            {
                tempDist = currentTransparency - toFadeTo;
                isFadingDown = true;
            }

            Destroy(gameObject, fadeDuration);
        }
    }

    public void RegisterDropAnimation(Vector3 ed)
    {
        if (dropAnimationOn == false)
        {
            transform.parent = null;
            dropAnimationOn = true;
            dropAnimationEnd = ed;
        }
    }

    public void hatFadeDisable()
    {
        hatFadeEnabled = false;
    }
    public void hatShadowDestroy()
    {
        Destroy(shadowPrefab);
    }
    public void drawCircleDust()
    {
        circleDust.Play();
    }
}