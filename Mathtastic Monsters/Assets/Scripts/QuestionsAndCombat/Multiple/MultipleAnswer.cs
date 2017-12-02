using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleAnswer : MonoBehaviour
{
    int Answer;

    Text answerText;

    multipleContainer container;

    public MonsterManager MonsterManager;


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
        if(!MonsterManager)
        {
            MonsterManager = FindObjectOfType<MonsterManager>();
        }

        if (!container)
            container = FindObjectOfType<multipleContainer>();


        if (Answer == container.enemyAnswerNeeded)
        {
            MonsterManager.currentEnemy.MonsterHurt();
        }
        else
        {
            MonsterManager.currentEnemy.EnemyAttack();
        }

    }
}
