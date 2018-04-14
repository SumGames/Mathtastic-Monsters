using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingObject : MonoBehaviour
{
    public float scrollSpeed;

    public bool scrolling;

    public float groundHorizontalLength;

    RectTransform rect;

    float CameraWidth;

    public float movementSpeed;


    public Vector3[] corners;

    public ScrollingObject nextScrollingObject;

    public float distPastX;

    // Use this for initialization
    void Start()
    {
        rect = GetComponent<RectTransform>();
        groundHorizontalLength = rect.rect.width;

        CameraWidth = Camera.main.pixelWidth;

        corners = new Vector3[4];
        

        if (CameraWidth < 800)
            CameraWidth = 800;
    }

    void Update()
    {
        /*

        if (scrolling)
        {
            transform.Translate(new Vector3(movementSpeed, 0, 0));

            float width = (groundHorizontalLength / 2);

            float cameraOffScreen = -(CameraWidth) / 2;

            float currentPositionX = transform.localPosition.x + width;


            //currentPositionX = (transform.localPosition.x + rect.rect.xMax);




            if (currentPositionX < cameraOffScreen)
            {
                Debug.Log(currentPositionX);

                RepositionBackground();
            }
        }
    */

        if (scrolling)
        {
            transform.Translate(new Vector3(movementSpeed, 0, 0));


            rect.GetWorldCorners(corners);



            distPastX = corners[2].x + 10;


            if (distPastX <= 0)
            {
                RepositionBackground();
            }
        }

    }

    internal void Scroll(bool move, float a_speed)
    {
        movementSpeed = scrollSpeed * a_speed * 1;


        scrolling = move;
    }

    //Moves the object this script is attached to right in order to create our looping background effect.
    private void RepositionBackground()
    {
        float jump = 0;

        if (nextScrollingObject)
        {
            rect.GetWorldCorners(corners);

            float otherWidth = (nextScrollingObject.corners[2].x);

            otherWidth = otherWidth - (1);


            float myWidth = (corners[2].x - corners[1].x) / 2;

            jump = otherWidth + myWidth;
        }
        else
        {
            rect.GetWorldCorners(corners);

            float width = corners[2].x - corners[1].x / 2;


            jump = CameraWidth + width;
        }

       // jump = transform.localPosition.x * -1;

        Vector3 groundOffSet = new Vector3(jump, transform.position.y, transform.position.z);

        //Move this object from it's position offscreen, behind the player, to the new position off-camera in front of the player.


//        Vector3 groundOffSet = transform.localPosition;
  //      groundOffSet.x = transform.localPosition.x * -1;


        transform.position = groundOffSet;

    }
}
