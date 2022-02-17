using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiglettBehaviour : MonoBehaviour
{
    //x, z (-x, -z) ~ (x, z)
    public Vector2 spawnRange;

    public float minSpawnTimeGap;
    public float maxSpawnTimeGap;

    public float stayTime;
    public bool stealHat;

    private NewTimer stayTimer;
    private NewTimer spawnTimer;
    private bool isStaying;

    // Start is called before the first frame update
    void Start()
    {
        stayTimer =gameObject.AddComponent<NewTimer>();
        spawnTimer =gameObject.AddComponent<NewTimer>();
        stayTimer.MaxTime = stayTime;
        spawnTimer.MaxTime = Random.Range(minSpawnTimeGap, maxSpawnTimeGap);
        spawnTimer.TimerStart = true;
        isStaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnTimer.TimerStart == false && stayTimer.TimerStart == false)
        {
            if (isStaying)
            {
                transform.position = new Vector3(0f, -0.5f, 0f);

                spawnTimer.MaxTime = Random.Range(minSpawnTimeGap, maxSpawnTimeGap);
                spawnTimer.TimerStart = true;
                isStaying = false;
            }
            else
            {
                transform.position = new Vector3(Random.Range(-spawnRange.x, spawnRange.x), 0f, Random.Range(-spawnRange.y, spawnRange.y));

                stayTimer.MaxTime = stayTime;
                stayTimer.TimerStart = true;
                isStaying = true;
            }
        }
    }
}
