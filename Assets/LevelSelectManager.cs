using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] Animator[] levelSelectUIAnimators;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsAnimationStateOver(levelSelectUIAnimators[0],"CurtainOpen"))
		{
            levelSelectUIAnimators[1].SetTrigger("RemoteRiseUp");
		}
    }

    private bool IsAnimationStateOver(Animator animator, string animationStateName)
	{
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName) 
               && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
	}
}
