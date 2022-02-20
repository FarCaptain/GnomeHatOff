using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Header("Knockback Variables")]
    [SerializeField] public int knockBackForceAmount = 10;
    [HideInInspector]
    private float waitTimeBeforeMoving = 0.5f;

    public enum TypesOfHatSteal { None, Some, All };
    [Header("Hat Steal Variables")]
    [SerializeField] public TypesOfHatSteal typeOfHatStealChosen = TypesOfHatSteal.Some;
    [SerializeField] public int maxHatsToSteal = 0;

    public float KnockBackTime
	{
        get
		{
           return waitTimeBeforeMoving;
		}
	}
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
