using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DivisionDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    internal int DraggerAnswer;

    public Vector3 parentPositon;


    bool dragging;

    Vector3 lastKnownMousePosition;

    DivisionAnswers CurrentAnswer;
    // Use this for initialization
    void Start()
    {
        parentPositon = transform.position;
    }

    bool AnswerCorrect()
    {
        if (CurrentAnswer == null)
            return false;

        if (DraggerAnswer == CurrentAnswer.ReturnAnswer())
            return true;


        return false;

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
        Debug.Log(other.name);

    }
}