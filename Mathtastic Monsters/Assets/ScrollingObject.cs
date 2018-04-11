using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingObject : MonoBehaviour
{
    private Rigidbody2D scrollImage;
    public float scrollSpeed;

    bool scrolling;

    public float groundHorizontalLength;

    RectTransform rect;


    // Use this for initialization
    void Start()
    {
        //Leave position is 478 for an object of 100 width.
        //Ideal width is 854.

        rect = GetComponent<RectTransform>();
        groundHorizontalLength = rect.rect.width;

        float width = Camera.main.pixelWidth;

        Debug.Log(width);
        return;
        Debug.Log(groundHorizontalLength);

        float Division = 854 / groundHorizontalLength;
        Debug.Log(Division);


//        Division = groundHorizontalLength * Division;
//        Debug.Log(Division);

    }

    void Update()
    {
        if (scrolling)
        {
            float width = (groundHorizontalLength / 2);

            float cameraOffScreen = -(Camera.main.pixelWidth) / 2;

            float currentPositionX = transform.localPosition.x + width;


            if (currentPositionX < cameraOffScreen)
            {
                RepositionBackground();
            }
        }
    }

    internal void Scroll(bool move, float speed)
    {
        scrolling = move;

        if (!scrollImage)
            scrollImage = GetComponent<Rigidbody2D>();

        if (move)
        {
            scrollImage.velocity = new Vector2(scrollSpeed*speed, 0);
        }
        else
        {
            scrollImage.velocity = new Vector2(0, 0);
        }
    }

    //Moves the object this script is attached to right in order to create our looping background effect.
    private void RepositionBackground()
    {
        //This is how far to the right we will move our background object, in this case, twice its length. This will position it directly to the right of the currently visible background object.
        //Vector2 groundOffSet = new Vector2(groundHorizontalLength * 2f, 0);

        //Move this object from it's position offscreen, behind the player, to the new position off-camera in front of the player.


        Vector3 groundOffSet = transform.localPosition;
        groundOffSet.x = transform.localPosition.x * -1;


        transform.localPosition = groundOffSet;

    }
}
