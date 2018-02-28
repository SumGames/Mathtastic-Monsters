using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFilterTest : MonoBehaviour
{

    public MeshFilter mesh;

    public MeshFilter otherHead;

    public MeshFilter Upperarm;
    public MeshFilter Forearm;
    public MeshFilter Hand;

    public MeshFilter OtherArm;
    public MeshFilter OtherForeArm;
    public MeshFilter otherHand;

    // Use this for initialization
    void Start()
    {
        mesh.mesh = otherHead.mesh;

        /*
        Upperarm.mesh = OtherArm.mesh;
        Forearm.mesh = OtherForeArm.mesh;
        Hand.mesh = otherHand.mesh;
        */

        OtherArm.mesh = Upperarm.mesh;
        Forearm.mesh = OtherForeArm.mesh;
        Hand.mesh = otherHand.mesh;
    }

    // Update is called once per frame
    void Update()
    {

    }
}