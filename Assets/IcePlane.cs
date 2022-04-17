#define automatically

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlane : MonoBehaviour
{
    public static IcePlane instance;
    public float time;
    public float iceSize;
    public Vector2Int size;
    public bool manuallySet;
    float t = 0;
    GameObject ice;
    float maxDis;
    float minDis;
    public int shrinkStage;
    public int shrinkStopStage;
    int currentStage;
    float scale;
    SortedList<float, List<Ice>> planeMap;
    public Dictionary<int, Ice> iceSet;
    // Start is called before the first frame update
    void Start()
    {
        iceSet = new Dictionary<int, Ice>();
        iceSet.Clear();
        ice = Resources.Load<GameObject>("Ice/Ice");
        planeMap = new SortedList<float, List<Ice>>();
        currentStage = 0;
        shrinkStage += 1;
        shrinkStopStage += 1;
        if (manuallySet)
        {
            GameObject[] ices = GameObject.FindGameObjectsWithTag("Ground");
            int len = ices.Length;
            int amount = 0;
            for (int i = 0; i < len; i++)
            {
                Ice ice = ices[i].GetComponent<Ice>();
                if (ice != null)
                {
                    
                    iceSet.Add(amount++, ice);
                   
                }
                
                
                
            }
            
        }
        else
        {
            InitiatePlane();
        }
        
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        
        if (t > time && !manuallySet)
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
                        {
                            iceSet.Remove(list[i].id);
                            list[i].Melt();
                        }
                            
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
                GameObject go_ice = Instantiate(ice, new Vector3(j, -0.1f, i), Quaternion
                    .identity, transform);
                int count = iceSet.Count;
                go_ice.GetComponent<Ice>().id = count + 1;
                Debug.Log("addd");
                iceSet.Add(count+1,go_ice.GetComponent<Ice>());
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


    }

    public Vector3 GetRespawnPos()
    {
        int amount = iceSet.Count;
        int random = Random.Range(0, amount);
        if(amount <= 0)
        {
            return Vector3.zero;
        }
        while (true)
        {
            if (iceSet.ContainsKey(random) && iceSet[random]!=null)
            {
                Debug.Log(random + " " + iceSet[random]);
                return iceSet[random].transform.position;
            }
            random = Random.Range(0, amount);
        }

        return Vector3.zero;
    }
}
