using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DivisionDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector3 parentPositon;


    bool dragging;

    Vector2 lastKnownMousePosition;


    // Use this for initialization
    void Start()
    {

        parentPositon = transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        if (dragging)
        {
            Vector3 inputMoved = new Vector3();


            if (Input.touchCount > 0)
            {
                inputMoved = Input.GetTouch(0).position - lastKnownMousePosition;

                lastKnownMousePosition = Input.GetTouch(0).position;
            }
            else if (Input.GetMouseButton(0))
            {
                inputMoved = (Vector2)(Input.mousePosition) - lastKnownMousePosition;

                lastKnownMousePosition = Input.mousePosition;
            }

            transform.position += inputMoved;

        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.touchCount > 0)
        {
            lastKnownMousePosition = Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButton(0))
        {
            lastKnownMousePosition = Input.mousePosition;
        }

        dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }
}