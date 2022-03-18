using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlane : MonoBehaviour
{
    
    public float time;
    public float speed;
    public Vector2Int size;
    float t = 0;
    GameObject ice;
    float maxDis;
    float minDis;
    public int shrinkStage;
    int currentStage;
    float scale;
    SortedList<float, List<Ice>> planeMap;
    // Start is called before the first frame update
    void Start()
    {
        ice = Resources.Load<GameObject>("ice");
        planeMap = new SortedList<float, List<Ice>>();
        currentStage = 0;
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
        if (currentStage < shrinkStage -1 )
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
        for(int i =-size.x; i <= size.x; i++)
        {
            for (int j = -size.y; j <= size.y; j++)
            {
                GameObject go_ice = Instantiate(ice, new Vector3(i,0,j), Quaternion
                    .identity, transform);
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
        

    }
}
