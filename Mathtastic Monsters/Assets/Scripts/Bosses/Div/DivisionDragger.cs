using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DivisionDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int DraggerAnswer;

    bool positionSet;
    Vector3 originalPosition;

    bool dragging;

    Vector3 lastKnownMousePosition;

    // Use this for initialization
    void Start()
    {
        originalPosition = transform.position;
    }

    internal void ResetDragger()
    {
        if(!positionSet)
        {
            originalPosition = transform.position;
            positionSet = true;
        }
        else
        {
            transform.position = originalPosition;

        }

        GetComponent<Image>().color = Color.white;

        DraggerAnswer = -2;


    }

    // Update is called once per frame
    void Update()
    {

        if (dragging)
        {
            Vector3 inputMoved = new Vector3();
            if (Input.touchCount > 0)
            {

                lastKnownMousePosition = Input.GetTouch(0).position;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                inputMoved = (Vector3)Input.GetTouch(0).position - lastKnownMousePosition;

                lastKnownMousePosition = ray.origin;

            }
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                inputMoved = ray.origin - lastKnownMousePosition;

                lastKnownMousePosition = ray.origin;
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            lastKnownMousePosition = ray.origin;

            //lastKnownMousePosition = Input.mousePosition;
        }

        dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Answer")
        {
            DivisionAnswers answer = other.GetComponent<DivisionAnswers>();
            answer.ChangeDragger(this, true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Answer")
        {
            DivisionAnswers answer = other.GetComponent<DivisionAnswers>();
            answer.ChangeDragger(this, false);


        }

    }



}