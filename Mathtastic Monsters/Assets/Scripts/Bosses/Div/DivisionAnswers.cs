using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionAnswers : MonoBehaviour
{
    int CorrectAnswer;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    internal void SetAnswer(int a_answer)
    {
        CorrectAnswer = a_answer;
    }

    internal int ReturnAnswer()
    {
        return CorrectAnswer;
    }

}
