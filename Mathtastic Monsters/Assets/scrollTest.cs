using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollTest : MonoBehaviour
{

    public Vector3[] corners;

    RectTransform rect;

    public float distPastX = 111;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (!rect)
            rect = GetComponent<RectTransform>();

        if (distPastX <= 0)
            RepositionBackground();

        transform.Translate(new Vector3(-10, 0, 0));



        Vector3 pos = rect.localPosition;

        rect.GetWorldCorners(corners);
        var width = corners[2].x - corners[0].x;



        distPastX = corners[2].x;
    }

    //Moves the object this script is attached to right in order to create our looping background effect.
    private void RepositionBackground()
    {

        float jump = transform.localPosition.x * -1;

        Vector3 groundOffSet = new Vector3(jump, transform.localPosition.y, transform.localPosition.z);


        transform.localPosition = groundOffSet;

    }
}