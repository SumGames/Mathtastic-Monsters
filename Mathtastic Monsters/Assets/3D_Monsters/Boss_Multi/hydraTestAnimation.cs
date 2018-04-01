using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hydraTestAnimation : MonoBehaviour
{
    Animator animator;

    bool HeadOneDead;
    bool headTwoDead;

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();
        
        
	}
    internal void KillOne()
    {
        if (!HeadOneDead)
        {
            animator.Play("HeadOneDying", 1);
            HeadOneDead = true;
        }
    }

    internal void killTwo()
    {
        if (!headTwoDead)
        {
            animator.Play("HeadTwoDying", 2);
            headTwoDead = true;
        }
    }
}
