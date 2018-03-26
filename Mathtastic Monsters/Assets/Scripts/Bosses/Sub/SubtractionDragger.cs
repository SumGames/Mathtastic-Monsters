using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SubtractionDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string AnswerNeeded;

    bool positionSet;
    public Vector3 originalPosition;

    bool dragging;

    Vector3 lastKnownMousePosition;

    internal void ResetDragger(bool positionOnly)
    {
        dragging = false;

        if (!positionSet)
        {
            originalPosition = transform.localPosition;
            positionSet = true;
        }
        else
        {
            transform.localPosition = originalPosition;

        }

        GetComponent<Image>().color = Color.white;

        if (positionOnly)
            return;

        SetDragger("", "");
    }

    internal void SetDragger(string answer, string words )
    {
        AnswerNeeded = answer;
        GetComponentInChildren<Text>().text = words;

        if (answer == "")
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            Vector3 inputMoved = new Vector3();
            if (Input.touchCount > 0)
            {

                Vector3 input = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0);


                transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 9));

                return;
            }
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                inputMoved = ray.origin - lastKnownMousePosition;

                lastKnownMousePosition = ray.origin;

                transform.position += inputMoved;
            }            
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
}