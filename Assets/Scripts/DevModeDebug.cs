using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevModeDebug : MonoBehaviour
{
    HatCollecter[] hatCollecters;
    void Start()
    {
        hatCollecters = FindObjectsOfType<HatCollecter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild)
		{
			UseMKeyForDrop();
		}
	}

	private void UseMKeyForDrop()
	{
		if (Input.GetKeyDown(KeyCode.M))
		{
			for (int i = 0; i < hatCollecters.Length; i++)
			{
				hatCollecters[i].hatdrop = true;
			}
		}
	}
}
