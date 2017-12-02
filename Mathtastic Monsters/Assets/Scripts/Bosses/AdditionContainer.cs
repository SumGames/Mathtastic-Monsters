using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionContainer : MonoBehaviour
{
    public int enemyAnswerNeeded;

    public AdditionAnswer[] answers;

    public Player player;


    bool gotten;


    Dictionary<int, int> abilities; //A list of abilities with charges >1, and their charge count.

    List<int> answersList;

    internal void MultipleAnswers(BossMonster boss, BossButton a_running)
    {
        DisableMultiple();

        enemyAnswerNeeded = boss.answerNeeded;

        player.SetTime(true, a_running.levelTime);

        foreach (AdditionAnswer item in answers)
        {
            item.SetAnswer(-1);
        }
        int index = Random.Range(0, answers.Length);



        answersList = new List<int>();


        answers[index].gameObject.SetActive(true);
        answers[index].SetAnswer(enemyAnswerNeeded);

        answersList.Add(enemyAnswerNeeded);

        for (int i = 1; i < a_running.enemyChoices; i++)
        {


            int wrongAnswer = -3;
            while (wrongAnswer < a_running.minNumber || wrongAnswer > a_running.maxNumber || CheckMultiple(a_running,wrongAnswer))
            {
                int range = Random.Range(-a_running.enemyAnswerRange, a_running.enemyAnswerRange);

                wrongAnswer = enemyAnswerNeeded + range;
            }
            index = Random.Range(0, answers.Length);
            while (answers[index].GetAnswer() != -1)
            {
                index = Random.Range(0, answers.Length);
            }
            answers[index].gameObject.SetActive(true);
            answers[index].SetAnswer(wrongAnswer);

        }
    }

    //Loop if we return true.
    bool CheckMultiple(QuizButton button, int result)
    {
        bool dupes=false;

        foreach (AdditionAnswer item in answers)
        {
            if (result == item.GetAnswer())
                dupes = true;
        }

        //No duplicates.
        if (dupes==false)
        {
            answersList.Add(result);
            return false;
        }

        //Duplicates, but too many to avoid getting more :(
        if (answersList.Count >= button.enemyAnswerRange * 2)
        {
            return false;
        }

        return true;
    }


    void DisableMultiple()
    {
        if (!gotten)
            GetChildren();

        foreach (AdditionAnswer item in answers)
        {
            item.gameObject.SetActive(false);
        }
    }

    void GetChildren()
    {
        answers = GetComponentsInChildren<AdditionAnswer>();
        gotten = true;
    }
}