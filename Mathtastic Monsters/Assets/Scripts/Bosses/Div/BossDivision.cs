using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDivision : MonoBehaviour
{
    public DivisionDragger[] draggers = new DivisionDragger[6];

    public DivisionAnswers[] answers = new DivisionAnswers[3];


    public int[] AnswerList = new int[6];

    BossMonster boss;

    int runningNumber;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void GenerateDivision(QuizButton button)
    {
        ClearEverything();

        answers[0].GetComponentInChildren<Text>().text = MakeQuestion(button, 0);
        answers[1].GetComponentInChildren<Text>().text = MakeQuestion(button, 1);
        answers[2].GetComponentInChildren<Text>().text = MakeQuestion(button, 2);


        SetDraggerButtons();

        FindObjectOfType<questionManager>().questionNeeded = "";
    }



    //Uses given values to calculate a random sum and its components, then store and display them.
    string MakeQuestion(QuizButton a_running, int index)
    {
        float numberRandom;
        float numberEither;

        numberRandom = (int)Random.Range(a_running.minNumber, (a_running.maxNumber + 1));


        runningNumber = a_running.enemyChoices;
        if (a_running.enemyChoices < 3)
        {
            runningNumber = 3;
        }
        else if (a_running.enemyChoices > 6)
        {
            runningNumber = 6;
        }


        int rand = Random.Range(0, (a_running.secondFixedNumber.Length));

        numberEither = a_running.secondFixedNumber[rand];

        float firstanswer = IsWhole(numberRandom / numberEither);


        int answer = IsWhole(firstanswer);

        //if Answer is too low/too high, or requires rounding to solve, we try again.
        if (answer < a_running.minAnswer || !NoDuplicateInAnswers(answer) || answer > a_running.maxAnswer)
        {
            return MakeQuestion(a_running, index);
        }

        AnswerList[index] = answer;
        answers[index].SetAnswer(answer);

        string answerWords;

        answerWords = "   ";
        answerWords += numberRandom.ToString("F0");


        answerWords += " ÷ " + numberEither.ToString("F0");


        return answerWords;
    }

    int IsWhole(float answer)
    {
        if (Mathf.Floor(answer) == answer)
        {
            return (int)answer;
        }
        return -1;
    }

    bool NoDuplicateInAnswers(int answer)
    {
        for (int i = 0; i < AnswerList.Length; i++)
        {
            if (AnswerList[i] == answer)
                return false;
        }
        return true;
    }

    void ClearEverything()
    {
        foreach (DivisionDragger item in draggers)
        {
            item.ResetDragger();

        }
        foreach (DivisionAnswers item in answers)
        {
            item.SetAnswer(-2);
        }
        for (int i = 0; i < AnswerList.Length; i++)
        {
            AnswerList[i] = -2;
        }

    }

    void SetDraggerButtons()
    {
        int wrongAnswer = -2;

        for (int i = 0, j = 3; i < 3 && j < runningNumber; i++, j++)
        {
            while (wrongAnswer <= 0 || !NoDuplicateInAnswers(wrongAnswer))
            {
                int range = Random.Range(-5, 5);
                wrongAnswer = AnswerList[i] + range;
            }
            AnswerList[j] = wrongAnswer;
        }
        for (int i = 0; i < draggers.Length; i++)
        {
            int index = Random.Range(0, draggers.Length);
            while (draggers[index].DraggerAnswer >= 0)
            {
                index = Random.Range(0, draggers.Length);
            }

            draggers[index].GetComponentInChildren<Text>().text = AnswerList[i].ToString();
            draggers[index].SetDragger(AnswerList[i]);
        }
    }


    public void CalculateCorrect()
    {
        bool correct = true;

        if (!boss)
            boss = FindObjectOfType<BossMonster>();

        for (int i = 0; i < 3; i++)
        {
            if (!answers[i].AnswerCorrect())
            {
                correct = false;
                break;
            }
        }

        if (correct)
        {
            boss.MonsterHurt();
        }
        else

        {
            boss.EnemyAttack();
        }
    }

}