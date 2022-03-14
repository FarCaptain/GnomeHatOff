using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDevModeDebug : MonoBehaviour
{
    private void Awake()
	{
		if(FindObjectsOfType<DevModeDebug>().Length>1)
		{
            Destroy(gameObject);
		}
        else
		{
            DontDestroyOnLoad(gameObject);
		}
	}
	void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
		{
            SceneManager.LoadScene(2);
		}

        if(Input.GetKeyDown(KeyCode.Escape))
		{
            SceneManager.LoadScene(1);
		}
    }
}
