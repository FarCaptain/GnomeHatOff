using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : HatSteal
{
    [Header("Mobility Variables")]
    [SerializeField] float movementSpeed = 0.5f;
    [SerializeField] float rotationSpeed = 100f;

    [Header("Direction Variables")]
    [SerializeField] bool xAxis;
    [SerializeField] bool zAxis;
    [SerializeField] int reverseX = 1;
    [SerializeField] int reverseZ = 1;

    [Header("Landing Variables")]
    [SerializeField] float fallAmount = 0.5f;

    private bool hasLanded = false;

    private AudioSource logAudioSource;
	private GameObject originLog;

    void Start()
    {
        logAudioSource = GetComponent<AudioSource>();
        AudioManager.PlayLogAudioClip(LogAudioStates.Rolling, logAudioSource);
    }

    // Update is called once per frame
    void Update()
	{
		LandLog();
		transform.Rotate(new Vector3(0, -rotationSpeed * Time.deltaTime, 0));
		MoveLog();
	}

	private void LandLog()
	{
		if (hasLanded == false)
		{
			transform.position -= new Vector3(0f, fallAmount * Time.deltaTime, 0f);
		}
	}
	private void MoveLog()
	{
		if (xAxis)
		{
			transform.position = new Vector3(transform.position.x + (movementSpeed * Time.deltaTime * reverseX), transform.position.y, transform.position.z);
		}
		if (zAxis)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (movementSpeed * Time.deltaTime * reverseZ));
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "LogDestroyer")
		{
			originLog.SetActive(true);
            Destroy(gameObject);
		}
        if(other.gameObject.tag=="Ground")
		{
            hasLanded = true;
        }
	}

	public void GetOriginLog(GameObject spawnLocationObject)
	{
		originLog = spawnLocationObject;
	}
}
