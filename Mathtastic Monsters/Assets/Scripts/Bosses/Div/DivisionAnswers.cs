using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DivisionAnswers : MonoBehaviour
{
    int CorrectAnswer;

    DivisionDragger draggedIn;

    internal bool AnswerCorrect()
    {
        if (draggedIn == null)
            return false;

        if (CorrectAnswer == draggedIn.DraggerAnswer)
            return true;

        return false;
    }


    // Use this for initialization
    void Start ()
    {
        GetComponent<Image>().color = Color.white;		
	}

    internal void SetAnswer(int a_answer)
    {
        CorrectAnswer = a_answer;
        draggedIn = null;
    }

    internal int ReturnAnswer()
    {
        return CorrectAnswer;
    }

    internal void ChangeDragger(DivisionDragger collided, bool entered)
    {
        if (entered)
        {
            if (draggedIn != null)
            {
                return;
            }

            draggedIn = collided;
            draggedIn.GetComponent<Image>().color = Color.cyan;
        }
        else
        {
            if (draggedIn != null && collided == draggedIn)
            {

                if (draggedIn != null)
                    draggedIn.GetComponent<Image>().color = Color.white;
                draggedIn = null;

            }
        }
    }

}
