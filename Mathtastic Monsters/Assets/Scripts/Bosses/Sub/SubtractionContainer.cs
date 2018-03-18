using System.Collections;
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


    public StoryManager storyManager;

    public CombatStateManager stateManager;


    float bossMaxHealth;
    float bossHealth;


    int choices = 2;

    void Update()
    {
        if (storyManager.phase == phases.None)
        {
            if (FiredTorpedo == null)
                LaunchTorpedo();
        }
        else
        {
            ResetPosition(false);
        }
    }


    internal void LaunchTorpedo()
    {
        FindObjectOfType<questionManager>().questionNeeded = "";


        if (stateManager.gameState != playStatus.playing)
            return;

        ResetPosition(true);



        GameObject Torpedo = Instantiate(TorpedoPrefab, start.transform.position, start.transform.rotation, this.transform);
        FiredTorpedo = Torpedo.GetComponent<Torpedo>();

        float speed = .04f;

        speed = ((bossMaxHealth - bossHealth) * speed) + speed;

        FiredTorpedo.CreateTorpedo(start, end, boss, answers[0].ToString(), this, speed);
    }

    internal void GenerateSubtraction(QuizButton button, float Health)
    {
        bossHealth = Health;

        ClearEverything();

        MakeQuestion(button, 0);
        MakeQuestion(button, 1);
        MakeQuestion(button, 2);

        SetDraggerButtons();

    }


    internal void SetUpSubtraction(float health)
    {
        if(FiredTorpedo)
        {
            Destroy(FiredTorpedo.gameObject);
        }

        bossMaxHealth = health;

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


        answerWords += "-" + numberRandom[1].ToString("F0");

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

    internal void ResetPosition(bool a_interactable)
    {
        foreach (SubtractionDragger item in draggers)
        {
            item.GetComponent<Button>().interactable = a_interactable;

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