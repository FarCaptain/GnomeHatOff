using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballSpawner : MonoBehaviour
{
    public Transform[] initializePos;
    float timeInterval;
    float spawnTime;
    public float minSpawnTime;
    public float maxSpawnTime;

    public float minForce;
    public float maxForce;
    public GameObject go_Snowball;
    public float dropVelocity;
    // Start is called before the first frame update
    void Start()
    {
        timeInterval = 0;
        spawnTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeInterval += Time.fixedDeltaTime;
        if(timeInterval >= spawnTime)
        {
            timeInterval = 0;
            spawnTime = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
            int index = UnityEngine.Random.Range(0, 4);
            Debug.Log("index:"+index);
            Spawn(index);
        }
    }

    void Spawn(int i)
    {
        Vector3 force = GetRandomForce(i);
        GameObject snowball = Instantiate<GameObject>(go_Snowball, initializePos[i]);
        snowball.transform.Find("SnowBoulder").GetComponent<Snowball>().AddForce(force);
    }

    Vector3 GetRandomForce(int i)
    {
        float x =0;
        float z =0;
        switch (i)
        {
            case 0: 
                x = -Random.Range(minForce, maxForce); 
                z = -Random.Range(minForce, maxForce);
                break;
            case 1:
                x = Random.Range(minForce, maxForce);
                z = -Random.Range(minForce, maxForce);
                break;
            case 2:
                x = Random.Range(minForce, maxForce);
                z = Random.Range(minForce, maxForce);
                break;
            case 3:
                x = -Random.Range(minForce, maxForce);
                z = Random.Range(minForce, maxForce);
                break;
        }
        return new Vector3(x,dropVelocity*-20,z);
    }


}
