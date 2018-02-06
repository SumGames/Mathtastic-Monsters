﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtractionContainer : MonoBehaviour
{
    int[] answers = new int[3];
    string[] questions = new string[3];

    public SubtractionDragger[] draggers;

    public RectTransform start;

    public RectTransform end;


    public BossMonster boss;

    int runningNumber;



    public GameObject TorpedoPrefab;
    public Torpedo FiredTorpedo;
    int TorpedoesFired;


    public StoryManager storyManager;

    public CombatStateManager stateManager;

    public Text questionText;


    int choices = 2;

    void Update()
    {
        if (storyManager.phase == phases.None && FiredTorpedo == null)
        {
            LaunchTorpedo();
        }
    }


    internal void LaunchTorpedo()
    {
        questionText.text = "";

        if (stateManager.gameState != playStatus.playing)
            return;

        ResetPosition();



        GameObject Torpedo = Instantiate(TorpedoPrefab, this.transform, false);
        FiredTorpedo = Torpedo.GetComponent<Torpedo>();

        float speed = 1;


        TorpedoesFired++;

        speed = (TorpedoesFired * 0.2f);

        FiredTorpedo.CreateTorpedo(start, end, boss, answers[TorpedoesFired].ToString(), this, speed);
    }

    internal void GenerateSubtraction(QuizButton button)
    {
        ClearEverything();

        MakeQuestion(button, 0);
        MakeQuestion(button, 1);
        MakeQuestion(button, 2);

        SetDraggerButtons();

        TorpedoesFired = 0;
    }



    //Uses given values to calculate a random sum and its components, then store and display them.
    bool MakeQuestion(QuizButton a_running, int index)
    {
        if (a_running.enemyChoices < 2)
        {
            choices = 2;
        }
        else if (a_running.enemyChoices > 3)
        {
            choices = 3;
        }
        else
        {
            choices = a_running.enemyChoices;
        }
        int[] numberRandom = new int[2];

        for (int i = 0; i < 2; i++)
        {
            numberRandom[i] = (int)Random.Range(a_running.minNumber, (a_running.maxNumber + 1));
        }

        int answer = numberRandom[0] - numberRandom[1];


        //if Answer is too low/too high, or requires rounding to solve, we try again.
        if (answer < a_running.minAnswer || !NoDuplicateInAnswers(answer) || answer > a_running.maxAnswer)
        {
            return MakeQuestion(a_running, index);
        }

        answers[index] = answer;

        string answerWords;

        answerWords = "   ";
        answerWords += numberRandom[0].ToString("F0");


        answerWords += "\n -" + numberRandom[1].ToString("F0");

        questions[index] = answerWords;

        return true;
    }

    bool NoDuplicateInAnswers(int answer)
    {
        for (int i = 0; i < answers.Length; i++)
        {
            if (answers[i] == answer)
                return false;
        }
        return true;
    }

    void ClearEverything()
    {
        foreach (SubtractionDragger item in draggers)
        {
            item.ResetDragger(false);
        }
        for (int i = 0; i < answers.Length; i++)
        {
            answers[i] = 0;
            questions[i] = "";
        }
    }

    internal void ResetPosition()
    {
        foreach (SubtractionDragger item in draggers)
        {
            item.ResetDragger(true);
        }

    }

    void SetDraggerButtons()
    {
        for (int i = 0; i < draggers.Length && i < choices; i++)
        {
            int index = Random.Range(0, choices);
            while (draggers[index].AnswerNeeded != "")
            {
                index = Random.Range(0, choices);
            }

            draggers[index].GetComponentInChildren<Text>().text = answers[i].ToString();
            draggers[index].SetDragger(answers[i].ToString(), questions[i]);

        }
    }
}