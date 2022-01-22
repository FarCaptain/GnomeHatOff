using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatSpawning : MonoBehaviour
{
    public GameObject hatPrefab;

    public Vector3 center;
    public Vector3 size;

    public float timeInterval = 0f;

    float timeGap;
    // Start is called before the first frame update
    void Start()
    {
        timeGap = Random.Range( 0.6f, 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        timeInterval += Time.deltaTime;
        if (timeInterval > timeGap)
        {
            timeGap = Random.Range(1.3f, 3.5f);
            SpawnHat();
            timeInterval = 0f;
        }
        if (Input.GetKey(KeyCode.P))
            SpawnHat();
    }

    public void SpawnHat()
    {
        Vector3 pos = transform.localPosition + center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
        Instantiate(hatPrefab, pos, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawCube(transform.localPosition + center, size);
    }
}
