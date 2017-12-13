using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionAnswer : MonoBehaviour
{
    int Answer;

    Text answerText;

    AdditionContainer container;
    BossMonster monster;

    //Return our answer.
    internal int GetAnswer()
    {
        return Answer;
    }

    //Set this answer, then set the button's text.
    internal void SetAnswer(int newAnswer)
    {
        Answer = newAnswer;
        if (!answerText)
        {
            answerText = GetComponentInChildren<Text>();
        }
        answerText.text = newAnswer.ToString();
    }

    //Player has clicked on button, button checks if the answer was correct, and acts accordingly.
    public void submitAnswer()
    {
        if (!monster)
        {
            container = GetComponentInParent<AdditionContainer>();
            monster = FindObjectOfType<BossMonster>();
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
