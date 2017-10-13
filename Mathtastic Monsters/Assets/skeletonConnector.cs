using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonConnector : MonoBehaviour
{
    public Transform[] limbs;


    // Use this for initialization
    void Start()
    {

        limbs = GetComponentsInChildren<Transform>(true);

    }

    // Update is called once per frame
    void Update ()
    {        

    }
    void OnDrawGizmosSelected()
    {
        if (limbs.Length > 2)
        {
            Gizmos.color = Color.blue;
            //Body.
            Gizmos.DrawLine(limbs[2].position, limbs[3].position);
            Gizmos.DrawLine(limbs[3].position, limbs[4].position);

            //Head.
            Gizmos.DrawLine(limbs[4].position, limbs[5].position);

            //L Upper.
            Gizmos.DrawLine(limbs[3].position, limbs[6].position);
            Gizmos.DrawLine(limbs[6].position, limbs[7].position);
            Gizmos.DrawLine(limbs[7].position, limbs[8].position);
            //R Upper.
            Gizmos.DrawLine(limbs[3].position, limbs[9].position);
            Gizmos.DrawLine(limbs[9].position, limbs[10].position);
            Gizmos.DrawLine(limbs[10].position, limbs[11].position);

            //L Lower;
            Gizmos.DrawLine(limbs[2].position, limbs[12].position);
            Gizmos.DrawLine(limbs[12].position, limbs[13].position);
            Gizmos.DrawLine(limbs[13].position, limbs[14].position);

            // R Lower
            Gizmos.DrawLine(limbs[2].position, limbs[15].position);
            Gizmos.DrawLine(limbs[15].position, limbs[16].position);
            Gizmos.DrawLine(limbs[16].position, limbs[17].position);
        }
    }

}
