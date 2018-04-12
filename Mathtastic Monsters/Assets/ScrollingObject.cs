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


    // Use this for initialization
    void Start()
    {
        rect = GetComponent<RectTransform>();
        groundHorizontalLength = rect.rect.width;

        CameraWidth = Camera.main.pixelWidth;





        if (CameraWidth < 800)
            CameraWidth = 800;
    }

    void Update()
    {
        
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
    }

    internal void Scroll(bool move, float a_speed)
    {
        movementSpeed = scrollSpeed * a_speed * 1;


        scrolling = move;
    }

    //Moves the object this script is attached to right in order to create our looping background effect.
    private void RepositionBackground()
    {
        //This is how far to the right we will move our background object, in this case, twice its length. This will position it directly to the right of the currently visible background object.

        float jump = transform.localPosition.x + (groundHorizontalLength * 2);

       // jump = transform.localPosition.x * -1;

        Vector3 groundOffSet = new Vector3(jump, transform.localPosition.y, transform.localPosition.z);

        //Move this object from it's position offscreen, behind the player, to the new position off-camera in front of the player.


//        Vector3 groundOffSet = transform.localPosition;
  //      groundOffSet.x = transform.localPosition.x * -1;


        transform.localPosition = groundOffSet;

    }
}
