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
    }



    //Uses given values to calculate a random sum and its components, then store and display them.
    string MakeQuestion(QuizButton a_running, int index)
    {
        float[] numbers = new float[a_running.variableCount];

        //Randomise as many numbers as required, within range.
        for (int i = 0; i < 2; i++)
        {
            numbers[i] = (int)Random.Range(a_running.minNumber, (a_running.maxNumber + 1));
        }

        int answer = IsWhole(numbers[0] / numbers[1]);

        //if Answer is too low/too high, or requires rounding to solve, we try again.
        if (answer < a_running.minAnswer || !NoDuplicateInAnswers(answer))
        {
            return MakeQuestion(a_running, index);
        }

        AnswerList[index] = answer;
        answers[index].SetAnswer(answer);

        string answerWords;

        answerWords = "   ";
        answerWords += numbers[0].ToString("F0");

        for (int i = 1; i < a_running.variableCount; i++)
        {
            answerWords += "\n /" + numbers[i].ToString("F0");
        }

        answerWords += "\n= ";


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

    }

    void SetDraggerButtons()
    {
        int wrongAnswer = -2;

        for (int i = 0, j = 3; i < 3; i++, j++)
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
            while (draggers[index].DraggerAnswer > 0)
            {
                index = Random.Range(0, draggers.Length);
            }

            draggers[index].GetComponentInChildren<Text>().text = AnswerList[i].ToString();
            draggers[index].DraggerAnswer = AnswerList[i];

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