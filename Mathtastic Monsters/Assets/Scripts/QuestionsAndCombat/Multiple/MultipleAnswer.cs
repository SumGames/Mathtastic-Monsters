using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleAnswer : MonoBehaviour
{
    int Answer;

    Text answerText;

    multipleContainer container;
    Monster monster;


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
        Answer = newAnswer;
        if (!answerText)
        {
            answerText = GetComponentInChildren<Text>();
        }
        answerText.text = newAnswer.ToString();
    }

    public void submitAnswer()
    {
        if(!monster)
        {
            container = FindObjectOfType<multipleContainer>();
            monster = FindObjectOfType<Monster>();
        }

        if (Answer == container.enemyAnswerNeeded)
        {
            monster.MonsterHurt();
        }
        else
        {
            monster.EnemyAttack();
        }

    }
}
