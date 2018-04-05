using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingObject : MonoBehaviour
{
    private Rigidbody2D scrollImage;
    public float scrollSpeed;

    bool scrolling;

    private float groundHorizontalLength;

    // Use this for initialization
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        groundHorizontalLength = rect.rect.width;

    }

    void Update()
    {
        if (scrolling)
        {
            if (transform.localPosition.x < -groundHorizontalLength)
            {
                RepositionBackground();
            }
        }
    }

    internal void Scroll(bool move)
    {
        scrolling = move;

        if (!scrollImage)
            scrollImage = GetComponent<Rigidbody2D>();

        if (move)
        {
            scrollImage.velocity = new Vector2(scrollSpeed, 0);
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
        Vector2 groundOffSet = new Vector2(groundHorizontalLength * 2f, 0);

        //Move this object from it's position offscreen, behind the player, to the new position off-camera in front of the player.
        transform.localPosition = (Vector2)transform.localPosition + groundOffSet;
    }
}
