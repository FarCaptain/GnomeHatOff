using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHatPileSpawn : MonoBehaviour
{
    [SerializeField] int numberOfHats;
    [SerializeField] GameObject hatPrefab;
    float hatWidth, hatHeight;
    Vector3 initialSpawnLocation;
    Vector3 spawnLocation;
    int zAdjustCouter = 0;
    int yAdjustCounter = 0;
    void Start()
    {
        initialSpawnLocation = new Vector3(0, 1, 0);
        spawnLocation = initialSpawnLocation;
        hatWidth = hatPrefab.transform.GetChild(0).GetComponent<BoxCollider>().bounds.extents.x;
        hatHeight = hatPrefab.transform.GetChild(0).GetComponent<BoxCollider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        print(hatHeight + hatWidth);
        if(Input.GetKeyDown(KeyCode.Alpha0))
		{
		    SpawnHatPile();
		}
	}

	private void SpawnHatPile()
	{
        for(int i=1;i< numberOfHats+1;i++)
		{
            Instantiate(hatPrefab, spawnLocation, Quaternion.identity);
            if (i>=3 && i%3==0)
			{
                zAdjustCouter++;
                spawnLocation = new Vector3(initialSpawnLocation.x, initialSpawnLocation.y + (0.8f*yAdjustCounter), initialSpawnLocation.z - (0.5f*zAdjustCouter));
                if(i%9==0 && i>=9)
				{
                    zAdjustCouter = 0;
                    yAdjustCounter++;
                    spawnLocation = new Vector3(initialSpawnLocation.x, initialSpawnLocation.y + (0.8f*yAdjustCounter), initialSpawnLocation.z);
                }
            }
            else
            {
                spawnLocation = new Vector3(spawnLocation.x + 0.6f, spawnLocation.y, spawnLocation.z);
            }
		}
	}
}
