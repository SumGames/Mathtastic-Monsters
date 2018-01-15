using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Torso,
    FourArm,
    FourLeg
}

public class ModelLines : MonoBehaviour
{
    public Type m_type;

    public Transform[] links;


	// Use this for initialization
	void Start ()
    {
//        links = GetComponentsInChildren<Transform>();        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_type == Type.Torso)
            DrawNormal();

    }

    public void DrawNormal()
    {
        Debug.DrawLine(links[1].position, links[2].position);
        Debug.DrawLine(links[1].position, links[3].position);

        Debug.DrawLine(links[1].position, links[7].position);
        Debug.DrawLine(links[7].position, links[8].position);
        Debug.DrawLine(links[8].position, links[9].position);
        Debug.DrawLine(links[9].position, links[10].position);


        //Left Leg
        Debug.DrawLine(links[2].position, links[3].position);
        Debug.DrawLine(links[3].position, links[4].position);
        Debug.DrawLine(links[4].position, links[5].position);

        //Neck?
        Debug.DrawLine(links[1].position, links[12].position);
        Debug.DrawLine(links[1].position, links[13].position);
        Debug.DrawLine(links[13].position, links[14].position);
        Debug.DrawLine(links[14].position, links[15].position);
        
        //Left Arm
        Debug.DrawLine(links[14].position, links[15].position);
        Debug.DrawLine(links[15].position, links[16].position);
        Debug.DrawLine(links[16].position, links[17].position);
        Debug.DrawLine(links[17].position, links[18].position);

        Debug.DrawLine(links[22].position, links[23].position);
        Debug.DrawLine(links[23].position, links[24].position);
        Debug.DrawLine(links[24].position, links[25].position);


        //Head
        Debug.DrawLine(links[14].position, links[19].position);
        Debug.DrawLine(links[19].position, links[20].position);
        Debug.DrawLine(links[20].position, links[21].position);

    }
}
