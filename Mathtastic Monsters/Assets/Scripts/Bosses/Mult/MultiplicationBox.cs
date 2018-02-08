using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplicationBox : MonoBehaviour
{
    public int boxAnswer;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void SetAnswer(int a_answer)
    {
        if (a_answer < 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }

        boxAnswer = a_answer;
        GetComponentInChildren<Text>().text = a_answer.ToString();
    }

    public void Select()
    {
        FindObjectOfType<MultiplicationContainer>().AddNewButton(this);
    }
}
