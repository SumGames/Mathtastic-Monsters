﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionContainer : MonoBehaviour
{
    public int enemyAnswerNeeded;

    public AdditionAnswer[] answers;

    public Player player;


    bool gotten;


    internal void MultipleAnswers(BossMonster boss, BossButton a_running)
    {
        DisableMultiple();

        enemyAnswerNeeded = boss.answerNeeded;

        player.setTime(true, a_running.levelTime);

        foreach (AdditionAnswer item in answers)
        {
            item.setAnswer(-1);
        }
        int index = Random.Range(0, 8);



        if (a_running.enemyChoices > 8)
            a_running.enemyChoices = 8;


        answers[index].gameObject.SetActive(true);
        answers[index].setAnswer(enemyAnswerNeeded);

        for (int i = 1; i < a_running.enemyChoices; i++)
        {
            int wrongAnswer = -3;
            while (wrongAnswer <= a_running.minAnswer || wrongAnswer >= a_running.maxAnswer || CheckMultiple(a_running, wrongAnswer))
            {
                int range = Random.Range(-a_running.enemyAnswerRange, a_running.enemyAnswerRange);
                wrongAnswer = enemyAnswerNeeded + range;
            }


            index = Random.Range(0, answers.Length);
            while (answers[index].getAnswer() != -1)
            {
                index = Random.Range(0, 8);
            }
            answers[index].gameObject.SetActive(true);
            answers[index].setAnswer(wrongAnswer);
        }
    }

    //Loop if we return true.
    bool CheckMultiple(QuizButton button, int result)
    {
        int number = 1;

        int differentAnswers = 0;

        foreach (AdditionAnswer item in answers)
        {
            if (item.getAnswer() >= 0)
                differentAnswers++;

            if (result == item.getAnswer())
                number++;
        }

        //No duplicates.
        if (number == 1)
        {
            return false;
        }

        //Duplicates, but too many to avoid getting more :(
        if (differentAnswers >= button.enemyAnswerRange)
        {
            return false;
        }

        return true;
    }


    void DisableMultiple()
    {
        if (!gotten)
            getChildren();

        foreach (AdditionAnswer item in answers)
        {
            item.gameObject.SetActive(false);
        }
    }

    void getChildren()
    {
        answers = GetComponentsInChildren<AdditionAnswer>();
        gotten = true;
    }
}