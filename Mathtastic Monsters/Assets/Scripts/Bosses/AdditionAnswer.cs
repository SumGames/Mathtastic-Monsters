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


    internal int GetAnswer()
    {
        return Answer;
    }

    internal void SetAnswer(int newAnswer)
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
