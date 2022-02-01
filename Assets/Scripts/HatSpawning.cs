using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatSpawning : MonoBehaviour
{
    public GameObject hatPrefab;
    public GameObject hatShadowPrefab;
    public float hatshadowDestroyTime=4;

    public Vector3 center;
    public Vector3 size;
    public float minGap = 0.3f;
    public float maxGap = 2.0f;
    public float hatRushTime = 30f;
    public float hatRushMinGap = 0.2f;

    float timeInterval = 0f;
    bool isHatRush = false;

    float timeGap;
    float deltaDashChange;
    public float accumulatedSpeed;
    Color[] hatColors = new Color[4];

    // Start is called before the first frame update
    void Start()
    {
        timeGap = Random.Range( 0.6f, 3.5f);

        ColorUtility.TryParseHtmlString("#3768A7",out hatColors[0]);
        ColorUtility.TryParseHtmlString("#7637A7", out hatColors[1]);
        ColorUtility.TryParseHtmlString("#A7A037", out hatColors[2]);
        ColorUtility.TryParseHtmlString("#FFFFFF", out hatColors[3]); //original

        float dashStartSpeed = minGap;
        hatRushMinGap = Mathf.Min(dashStartSpeed, hatRushMinGap);

        deltaDashChange = (dashStartSpeed - hatRushMinGap) / hatRushTime;
        accumulatedSpeed = dashStartSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHatRush && Timer.timeRemaining < hatRushTime)
            isHatRush = true;

        timeInterval += Time.deltaTime;
        if (timeInterval > timeGap)
        {
            timeGap = isHatRush? accumulatedSpeed = Mathf.Max(accumulatedSpeed - (deltaDashChange * timeGap), hatRushMinGap) : Random.Range(minGap, maxGap);
            SpawnHat();
            timeInterval = 0f;
        }
        if (Input.GetKey(KeyCode.P))
            SpawnHat();
    }

    public void SpawnHat()
    {
        Vector3 pos = transform.localPosition + center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
        GameObject hat = Instantiate(hatPrefab, pos, Quaternion.identity);
        hat.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = hatColors[Random.Range(0, hatColors.Length - 1)];
        generateShadow(pos, hat);
    }

    public void generateShadow(Vector3 pos, GameObject hat)
    {
        GameObject hatShadow = Instantiate(hatShadowPrefab, pos - (new Vector3(0, pos.y, 0)), Quaternion.identity);
        hat.GetComponent<HatFade>().HatshadowPrefab = hatShadow;
        //  Destroy(hatShadow, hatshadowDestroyTime);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawCube(transform.localPosition + center, size);
    }
}
