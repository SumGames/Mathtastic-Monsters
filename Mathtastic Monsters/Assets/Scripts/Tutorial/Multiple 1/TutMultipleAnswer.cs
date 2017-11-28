using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutMultipleAnswer : MonoBehaviour
{
    int Answer;

    Text answerText;

    TutmultipleContainer container;
    TutorialMonster monster;


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
        if(!monster)
        {
            container = FindObjectOfType<TutmultipleContainer>();
            monster = FindObjectOfType<TutorialMonster>();
        }

        if (Answer == container.enemyAnswerNeeded)
        {
            monster.MonsterHurt();
            FindObjectOfType<combatFeedback>().DamageSet(SetFeedback.PlayerCountered);
        }
        else
        {
            monster.EnemyAttack();
            FindObjectOfType<combatFeedback>().DamageSet(SetFeedback.PlayerHit);
        }

    }
}
