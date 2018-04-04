using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingObject : MonoBehaviour
{
    private Rigidbody2D scrollImage;
    public float scrollSpeed;

    // Use this for initialization
    void Start()
    {        
        //Start the object moving.        
    }
    void Update()
    {

    }

    internal void Scroll(bool move)
    {
        if (!scrollImage)
            scrollImage = GetComponent<Rigidbody2D>();

        if(move)
        {
            scrollImage.velocity = new Vector2(scrollSpeed, 0);
        }
        else
        {
            scrollImage.velocity = new Vector2(0, 0);
        }
    }
}