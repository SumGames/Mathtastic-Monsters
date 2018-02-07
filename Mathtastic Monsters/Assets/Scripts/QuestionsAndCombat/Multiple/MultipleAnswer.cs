using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleAnswer : MonoBehaviour
{
    int Answer;

    Text answerText;

    multipleContainer container;

    Image image;


    // Use this for initialization
    void Start () {
		
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
        if (!image)
            image = GetComponent<Image>();

        image.color = Color.white;

        Answer = newAnswer;
        if (!answerText)
        {
            answerText = GetComponentInChildren<Text>();
        }
        answerText.text = newAnswer.ToString();
    }

    public void SelectButton()
    {
        if (!container)
            container = FindObjectOfType<multipleContainer>();

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
