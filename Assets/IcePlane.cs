#define automatically

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlane : MonoBehaviour
{

    public float time;
    public float iceSize;
    public Vector2Int size;
    float t = 0;
    GameObject ice;
    float maxDis;
    float minDis;
    public int shrinkStage;
    public int shrinkStopStage;
    int currentStage;
    float scale;
    SortedList<float, List<Ice>> planeMap;
    GameObject[] iceList;
    // Start is called before the first frame update
    void Start()
    {
        ice = Resources.Load<GameObject>("Ice/Ice");
        planeMap = new SortedList<float, List<Ice>>();
        currentStage = 0;
        shrinkStage += 1;
        shrinkStopStage += 1;
        InitiatePlane();
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if (t > time)
        {
            t = 0;
            Shrink();
        }
    }

    void Shrink()
    {

        int length = planeMap.Count;
        if (currentStage < shrinkStopStage - 1)
        {

            float leftBoarder = maxDis - (currentStage + 1) * scale;

            for (int j = length - 1; j >= 0; j--)
            {
                if (planeMap.Keys[j] > leftBoarder)
                {
                    List<Ice> list = planeMap.Values[j];
                    int count = list.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (list[i] != null)
                            list[i].Melt();
                    }
                    planeMap.RemoveAt(j);
                }
                else
                {
                    break;
                }

            }
        }
        currentStage++;


    }

    void InitiatePlane()
    {
#if automatically
        int check = 0;
        for (float i = -size.x * 0.797f * iceSize + 0.797f * 0.5f * iceSize; i <= size.x * 0.797f * iceSize + 0.797f * 0.5f * iceSize; i += 0.797f * iceSize)
        {
            check++;
            float initPos;
            if (check % 2 == 0)
            {
                initPos = -size.y * 2.763f * iceSize + 2.763f * 0.5f * iceSize;
            }
            else
            {
                initPos = -size.y * 2.763f * iceSize + 2.763f * 0.5f * iceSize + 1.379f * iceSize;
            }
            for (float j = initPos; j <= size.y * 2.763f * iceSize - 2.763f * 0.5f * iceSize; j += 2.763f * iceSize)
            {
                GameObject go_ice = Instantiate(ice, new Vector3(j, 0, i), Quaternion
                    .identity, transform);
                go_ice.transform.localScale *= iceSize;
                float distance = Vector3.Distance(go_ice.transform.position, transform.position);
                if (planeMap.ContainsKey(distance))
                {
                    planeMap[distance].Add(go_ice.GetComponent<Ice>());
                }
                else
                {
                    List<Ice> list = new List<Ice>();
                    list.Add(go_ice.GetComponent<Ice>());
                    planeMap.Add(distance, list);
                }
            }
        }

        int length = planeMap.Count;
        maxDis = planeMap.Keys[length - 1];
        minDis = planeMap.Keys[0];
        scale = (maxDis - minDis) / shrinkStage;

#else
        iceList = GameObject.FindGameObjectsWithTag("Ground");
#endif
    }
}
