using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    [Header("Mobility Variables")]
    [SerializeField] float movementSpeed = 0.5f;
    [SerializeField] float rotationSpeed = 100f;

    [Header("Direction Variables")]
    [SerializeField] bool xAxis;
    [SerializeField] bool zAxis;
    [SerializeField] int reverseX = 1;
    [SerializeField] int reverseZ = 1;

    private GameObject parentGameObject;

    void Start()
    {
        parentGameObject = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, -rotationSpeed * Time.deltaTime, 0));
        if (xAxis)
		{
            transform.position = new Vector3(transform.position.x + (movementSpeed * Time.deltaTime * reverseX), transform.position.y, transform.position.z);
        }
        if(zAxis)
		{
            transform.position = new Vector3(transform.position.x , transform.position.y, transform.position.z + (movementSpeed * Time.deltaTime * reverseZ));
        }
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "LogDestroyer")
		{
            parentGameObject.GetComponent<LogSpawner>().logSpawnTimer.TimerStart = true;
            parentGameObject.GetComponent<LogSpawner>().logAlive = false;
            Destroy(gameObject);
		}
	}
}
