using UnityEngine;
using System.Collections.Generic;

public class multipleContainer : MonoBehaviour
{

    public int enemyAnswerNeeded;

    public GameObject calculator;

    public MultipleAnswer[] answers;

    public Player player;

    playerAbilities playerAbilities;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void SetMultiple(int answer, bool phase, QuizButton a_running)
    {
        player.EndTurn(phase);

        enemyAnswerNeeded = answer;

        if (phase && a_running != null)
        {
            MultipleAnswers(a_running);
        }
        else
        {
            DisableMultiple();
        }
        if (a_running)
            player.setTime(phase, a_running.enemPhaseTime);
    }

    void MultipleAnswers(QuizButton a_running)
    {


        calculator.SetActive(false);

        foreach (MultipleAnswer item in answers)
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
            while (wrongAnswer <= a_running.minAnswer || wrongAnswer >= a_running.maxAnswer)// || CheckMultiple(a_running, wrongAnswer))
            {
                int range = Random.Range(-a_running.enemyAnswerRange, a_running.enemyAnswerRange);
                wrongAnswer = enemyAnswerNeeded + range;
            }

            index = Random.Range(0, 8);
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

        foreach (MultipleAnswer item in answers)
        {
            if (result == item.getAnswer())
                number++;
        }

        //No duplicates.
        if (number == 1)
        {
            return false;
        }

        //Duplicates, but too many to avoid getting more :(
        if (number >= button.enemyAnswerRange)
        {
            return false;
        }

        return true;
    }


    internal void removeSingle()
    {
        MultipleAnswer removing = null;

        foreach (MultipleAnswer item in answers)
        {
            if (item.getAnswer() != enemyAnswerNeeded && item.getAnswer() > 0)
            {
                removing = item;
            }
        }
        if (removing)
        {
            removing.setAnswer(-2);
            removing.gameObject.SetActive(false);
        }
    }

    internal void DisableMultiple(bool both=false)
    {
        foreach (MultipleAnswer item in answers)
        {
            item.gameObject.SetActive(false);
        }

        if (both)
            calculator.SetActive(false);
        else
        {
            calculator.SetActive(true);
        }
    }
}