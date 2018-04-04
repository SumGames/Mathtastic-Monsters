using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingParent : MonoBehaviour
{


	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    ScrollingObject[] objects;


    public void ScrollAll(bool progress)
    {
        if (objects == null)
            objects = GetComponentsInChildren<ScrollingObject>();


        foreach (ScrollingObject item in objects)
        {
            item.Scroll(progress);
        }
    }


}
