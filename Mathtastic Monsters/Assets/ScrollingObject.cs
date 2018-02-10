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
        //Get and store a reference to the Rigidbody2D attached to this GameObject.
        scrollImage = GetComponent<Rigidbody2D>();
        //Start the object moving.
        scrollImage.velocity = new Vector2(scrollSpeed, 0);
    }
    void Update()
    {

    }
}