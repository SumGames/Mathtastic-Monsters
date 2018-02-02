using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtractionContainer : MonoBehaviour
{
    int[] answers = new int[4];
    string[] questions = new string[4];

    public SubtractionDragger[] draggers;

    public GameObject start;

    public RectTransform end;


    public BossMonster boss;

    int runningNumber;



    public GameObject TorpedoPrefab;
    public Torpedo FiredTorpedo;
    int TorpedoesFired;


    bool readyToFire;

    public StoryManager storyManager;

    public CombatStateManager stateManager;

    void Update()
    {
        if (readyToFire && storyManager.phase == phases.None && FiredTorpedo == null)
        {
            LaunchTorpedo();
            readyToFire = false;
        }
    }


    internal void LaunchTorpedo()
    {
        if (stateManager.gameState != playStatus.playing)
            return;

        ResetPosition();


        if (TorpedoesFired >= 3)
        {
            boss.CreateSubtraction(false);
            return;
        }


        GameObject Torpedo = Instantiate(TorpedoPrefab, this.transform, false);
        FiredTorpedo = Torpedo.GetComponent<Torpedo>();

        float speed = 1;


        TorpedoesFired++;

        switch (TorpedoesFired)
        {
            case 1:
                speed = 0.5f;
                break;
            case 2:
                speed = 0.75f;
                break;
            case 3:
                speed = 1;
                break;
            default:
                speed = 2;
                break;
        }


        FiredTorpedo.CreateTorpedo(end, boss, answers[TorpedoesFired].ToString(), this, speed);
    }

    internal void GenerateSubtraction(QuizButton button)
    {
        ClearEverything();

        MakeQuestion(button, 0);
        MakeQuestion(button, 1);
        MakeQuestion(button, 2);
        MakeQuestion(button, 3);

        SetDraggerButtons();

        TorpedoesFired = 0;
        readyToFire = true;
    }



    //Uses given values to calculate a random sum and its components, then store and display them.
    bool MakeQuestion(QuizButton a_running, int index)
    {
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
        for (int i = 0; i < draggers.Length; i++)
        {
            int index = i;
            draggers[index].GetComponentInChildren<Text>().text = answers[i].ToString();
            draggers[index].SetDragger(answers[i].ToString(), questions[i]);

        }
    }
}