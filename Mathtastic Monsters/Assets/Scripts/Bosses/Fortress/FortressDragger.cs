using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FortressDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int DraggerAnswer;

    bool positionSet;
    public Vector3 originalPosition;

    bool dragging;

    Vector3 lastKnownMousePosition;

    internal void ResetDragger()
    {
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
        SetDragger(-2);
    }

    internal void SetDragger(int newValue)
    {
        DraggerAnswer = newValue;

        GetComponentInChildren<Text>().text = newValue.ToString();

        if (DraggerAnswer <= 0)
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


                transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 9));


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
            FortressButton answer = other.GetComponent<FortressButton>();
            answer.ChangeDragger(this, true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Answer")
        {
            FortressButton answer = other.GetComponent<FortressButton>();
            answer.ChangeDragger(this, false);


        }

    }
}