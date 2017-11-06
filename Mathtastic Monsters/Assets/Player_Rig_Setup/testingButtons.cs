using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingButtons : MonoBehaviour
{
    public enum anims
    {
        idle,
        attack,
        hurt,
        die
    }


    public Animator body;



	// Use this for initialization
	void Start ()
    {
        body = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void animating(int a_anim)
    {
        string anim = "";

        switch (a_anim)
        {
            case 1:
                anim = "Idle";
                break;
            case 2:
                anim = "Attack_Anim";
                break;
            case 3:
                anim = "Hit_Anim";
                break;
            case 4:
                anim = "Death_Anim";
                break;
            default:
                break;
        }
        body.Play(anim);



    }

}
