using UnityEngine;
using System.Collections.Generic;

public class multipleContainer : MonoBehaviour
{

    public int enemyAnswerNeeded;

    public GameObject calculator;

    public MultipleAnswer[] answers;

    public Player player;

    playerAbilities playerAbilities;

    public List<int> answersList;

    public int attacksPherPhase = 1;

    public int attacks = 1;
    bool attacking;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void setAttacks(bool boss, bool startWithAttack)
    {
        if (!playerAbilities)
            playerAbilities = FindObjectOfType<playerAbilities>();

        bool doubleAttck = playerAbilities.DoubleStrike();

        if (doubleAttck && !boss)
        {
            attacksPherPhase = 2;
        }
        else
        {
            attacksPherPhase = 1;
        }
        if (startWithAttack || !boss)
        {
            attacks = attacksPherPhase;
        }
        else
            attacks = 0;
    }


    internal bool SetMultiple(int answer, QuizButton a_running, bool resetTime, int overRide)
    {        
        enemyAnswerNeeded = answer;
        if (overRide == 0)
        {
            if (attacks > 0)
            {
                attacking = false;
                attacks--;

            }
            else
            {
                attacks = attacksPherPhase;
                attacking = true;
            }
        }
        else if (overRide == 1)
        {
            attacking = true;
        }
        else
        {
            attacking = false;
        }

        bool enemyPhase = false;

        if (attacking)
        {
            enemyPhase = true;
            attacks = attacksPherPhase;
            MultipleAnswers(a_running);
        }
        else
        {
            enemyPhase = false;
            attacks--;
            DisableMultiple();
        }
        if(!resetTime)
        {
            Debug.Log("Not resetting");
        }


        if (a_running && resetTime)
        {
            player.SetTime(enemyPhase, a_running.enemPhaseTime);
        }

        player.EndTurn(enemyPhase);

        return enemyPhase;
    }

    void MultipleAnswers(QuizButton a_running)
    {


        calculator.SetActive(false);

        DisableMultiple(true);

        foreach (MultipleAnswer item in answers)
        {
            item.setAnswer(-1);
        }
        int index = Random.Range(0, 6);

        answersList = new List<int>();
        answersList.Add(enemyAnswerNeeded);


        if (a_running.enemyChoices > 6)
            a_running.enemyChoices = 6;

        if (a_running.enemyAnswerRange * 2 < a_running.enemyChoices)
        {
            a_running.enemyAnswerRange = a_running.enemyChoices / 2 + 2;
        }



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

            index = Random.Range(0, 6);
            while (answers[index].getAnswer() != -1)
            {
                index = Random.Range(0, 6);
            }
            answers[index].gameObject.SetActive(true);
            answers[index].setAnswer(wrongAnswer);
        }
    }

    //Loop if we return true.
    bool CheckMultiple(QuizButton button, int result)
    {
        bool dupes = false;

        foreach (MultipleAnswer item in answers)
        {
            if (result == item.getAnswer())
                dupes = true;
        }

        //No duplicates.
        if (dupes == false)
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