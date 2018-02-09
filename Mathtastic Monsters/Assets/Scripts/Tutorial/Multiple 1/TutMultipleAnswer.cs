using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutMultipleAnswer : MonoBehaviour
{
    int Answer;

    Text answerText;

    TutmultipleContainer container;

    internal Image image;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal int getAnswer()
    {
        return Answer;
    }

    internal void setAnswer(int newAnswer)
    {
        Answer = newAnswer;
        if (!answerText)
        {
            answerText = GetComponentInChildren<Text>();
        }
        answerText.text = newAnswer.ToString();
    }

    public void submitAnswer()
    {
        if (!container)
            container = FindObjectOfType<TutmultipleContainer>();

        image = GetComponent<Image>();

        if (container.SelectedAnswer != null)
        {
            if (container.SelectedAnswer == this)
            {
                container.SelectedAnswer.image.color = Color.white;
                container.submit.interactable = false;
                container.SelectedAnswer = null;
                return;
            }
            else
            {
                container.SelectedAnswer.image.color = Color.white;
                container.SelectedAnswer = null;
            }
        }
        container.submit.interactable = true;
        image.color = Color.yellow;
        container.SelectedAnswer = this;
    }
}
