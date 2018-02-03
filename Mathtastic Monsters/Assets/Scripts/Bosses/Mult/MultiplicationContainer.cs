﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplicationContainer : MonoBehaviour
{
    public MultiplicationBox[] answers = new MultiplicationBox[3];

    public int[] AnswerList = new int[6];

    public Text QuestionText;

    public int CorrectAnswer;

    public MultiplicationBox[] multiplicationBoxes = new MultiplicationBox[2];

    BossMonster boss;

    int nextPick = 0;

    public Button submit;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void GenerateMultiplication(QuizButton button, BossMonster a_boss)
    {
        ClearEverything();

        QuestionText.text = CreateCorrectAnswer(button);

        boss = a_boss;

        SetBoxes(button);
    }

    string CreateCorrectAnswer(QuizButton a_running)
    {
        int numberOne;
        int numberTwo;

        numberOne = (int)Random.Range(a_running.minNumber, (a_running.maxNumber + 1));


        int rand = Random.Range(0, (a_running.secondFixedNumber.Length));
        numberTwo = a_running.secondFixedNumber[rand];



        CorrectAnswer = numberOne * numberTwo;


        //if Answer is too low/too high, or requires rounding to solve, we try again.
        if (CorrectAnswer < a_running.minAnswer || CorrectAnswer > a_running.maxAnswer)
        {
            return CreateCorrectAnswer(a_running);
        }

        AnswerList[0] = numberOne;
        AnswerList[1] = numberTwo;


        return "_ x _\n=" + CorrectAnswer.ToString();
    }

    void SetBoxes(QuizButton button)
    {
        int wrongAnswer = -8;

        for (int i = 0; i < AnswerList.Length; i++)
        {
            wrongAnswer = -8;
            if (AnswerList[i] < 0)
            {
                while (wrongAnswer <= 0 || !NoDuplicateInAnswers(wrongAnswer))
                {
                    int range = (int)Random.Range(button.minNumber, button.maxNumber);
                    wrongAnswer = AnswerList[i] + range;
                }
                AnswerList[i] = wrongAnswer;
            }
        }

        for (int i = 0; i < answers.Length; i++)
        {
            int index = Random.Range(0, answers.Length);
            while (answers[index].boxAnswer > 0)
            {
                index = Random.Range(0, answers.Length);
            }

            answers[index].SetAnswer(AnswerList[i]);
        }
    }

    void ClearEverything()
    {
        submit.interactable = false;

        for (int i = 0; i < 2; i++)
        {
            if (multiplicationBoxes[i] != null)
                multiplicationBoxes[i].GetComponent<Image>().color = Color.white;
            multiplicationBoxes[i] = null;
        }

        for (int i = 0; i < AnswerList.Length; i++)
        {
            AnswerList[i] = -2;
        }

        foreach (MultiplicationBox item in answers)
        {
            item.SetAnswer(-2);
        }
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

    internal void AddNewButton(MultiplicationBox box)
    {
        for (int i = 0; i < 2; i++)
        {
            if (multiplicationBoxes[i] == null)
            {
                multiplicationBoxes[i] = box;
                multiplicationBoxes[i].GetComponent<Image>().color = Color.yellow;

                if (i == 1)
                    submit.interactable = true;

                return;
            }
            if (box == multiplicationBoxes[i])
            {
                multiplicationBoxes[i].GetComponent<Image>().color = Color.white;
                multiplicationBoxes[i] = null;
                submit.interactable = false;
                return;
            }
        }
        multiplicationBoxes[nextPick].GetComponent<Image>().color = Color.white;
        multiplicationBoxes[nextPick] = box;
        multiplicationBoxes[nextPick].GetComponent<Image>().color = Color.yellow;

        submit.interactable = true;


        if (nextPick == 0)
            nextPick = 1;
        else
            nextPick = 0;

    }

    public void SubmitAnswer()
    {
        submit.interactable = false;

        if (multiplicationBoxes[1] == null)
            return;

        int answer = multiplicationBoxes[0].boxAnswer * multiplicationBoxes[1].boxAnswer;

        if (CorrectAnswer == answer)
        {
            boss.CreateMultiplication();
        }
        else
        {
            boss.EnemyAttack();
        }
    }
}