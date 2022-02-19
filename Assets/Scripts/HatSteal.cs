using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatSteal : MonoBehaviour
{
    public enum TypesOfHatSteal { None, Some, All };
    [Header("Hat Steal Variables")]
    [SerializeField] public TypesOfHatSteal typeOfHatStealChosen = TypesOfHatSteal.Some;
    [SerializeField] public int maxHatsToSteal = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
