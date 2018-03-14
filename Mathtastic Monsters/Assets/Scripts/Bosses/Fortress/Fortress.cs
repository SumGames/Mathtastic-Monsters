using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fortress : MonoBehaviour
{
    public Text AnswerText;

    public questionManager questionManager;



    List<float> variablesUsed;

    List<operators> StoredOps;

    float AnswerNeeded;

    public FortressButton[] FortressButtons;


    public Button Defend;

    public Text[] operatorsText;


    int variableCount;


    public FortressDragger[] Draggers;


    public BossMonster bossMonster;

    public GameObject patience;
    public Text PatienceText;
    int PatienceLeft;


    operators mainOp;


    // Update is called once per frame
    void Update()
    {

    }

    internal void CalculateBODMAS(QuizButton a_running, int failures)
    {
        mainOp = a_running.Operator;

        variableCount = a_running.variableCount;


        variablesUsed = new List<float>(variableCount);


        variablesUsed = questionManager.randomiseBODMASNumbers(a_running);



        List<float> summingNumbers = new List<float>(5);

        List<operators> summingOps;

        summingOps = questionManager.ChooseOperators(a_running.Operator, (variableCount - 1));


        StoredOps = new List<operators>(summingOps);



        //Randomise as many numbers as required, within range.
        for (int i = 0; i < variableCount; i++)
        {
            summingNumbers.Add((int)variablesUsed[i]);
        }

        if (a_running.Operator == operators.AddSubMultDiv)
        {

            AnswerNeeded = summingNumbers[0] / summingNumbers[1];
            switch (summingOps[1])
            {
                case operators.Addition:
                    AnswerNeeded = AnswerNeeded + summingNumbers[2];
                    break;
                case operators.Subtraction:
                    AnswerNeeded = AnswerNeeded - summingNumbers[2];
                    break;
            }
            AnswerNeeded = AnswerNeeded * summingNumbers[3];
        }
        else
        {
            while (summingOps.Count > 0)
            {
                if (summingOps.Contains(operators.Division))
                {
                    int i = summingOps.IndexOf(operators.Division);

                    summingNumbers[i] /= summingNumbers[(i + 1)];
                    summingNumbers.RemoveAt(i + 1);
                    summingOps.RemoveAt(i);
                    continue;
                }

                if (summingOps.Contains(operators.Multiplication))
                {
                    int i = summingOps.IndexOf(operators.Multiplication);

                    summingNumbers[i] *= summingNumbers[(i + 1)];
                    summingNumbers.RemoveAt(i + 1);
                    summingOps.RemoveAt(i);
                    continue;
                }

                if (summingOps.Contains(operators.Addition))
                {
                    int i = summingOps.IndexOf(operators.Addition);

                    summingNumbers[i] += summingNumbers[(i + 1)];
                    summingNumbers.RemoveAt(i + 1);
                    summingOps.RemoveAt(i);
                    continue;
                }
                if (summingOps.Contains(operators.Subtraction))
                {
                    int i = summingOps.IndexOf(operators.Subtraction);

                    summingNumbers[i] -= summingNumbers[(i + 1)];

                    summingNumbers.RemoveAt(i + 1);
                    summingOps.RemoveAt(i);
                    continue;
                }
            }
            AnswerNeeded = summingNumbers[0];

        }

        bool whole = questionManager.IsWhole(AnswerNeeded);


        //if Answer is too low/too high, or requires rounding to solve, we try again.
        if ((AnswerNeeded <= a_running.minAnswer || AnswerNeeded > a_running.maxAnswer || !whole) && failures < 20)
        {
            int failed = failures + 1;

            CalculateBODMAS(a_running, failed);
            return;
        }

        if (failures >= 20)
            Debug.Log("Failed");


        ResetAll();
       

        if (a_running.Operator == operators.AddSubMultDiv)
        {
            operatorsText[3].text = "(";
            operatorsText[0].text = " / ";
            operatorsText[1].text = " + ";
            operatorsText[2].text = ") x ";

            operatorsText[2].gameObject.SetActive(true);
            FortressButtons[3].gameObject.SetActive(true);
        }
        else
        {
            operatorsText[3].text = "";
            for (int i = 0; i < 3; i++)
            {
                if (i < (variableCount - 1))
                {

                    switch (StoredOps[i])
                    {
                        case operators.Addition:
                            operatorsText[i].text = "+ ";
                            break;
                        case operators.Subtraction:
                            operatorsText[i].text = "- ";
                            break;
                        case operators.Multiplication:
                            operatorsText[i].text = "x ";
                            break;
                        case operators.Division:
                            operatorsText[i].text = "/ ";
                            break;
                        default:
                            break;
                    }
                }
            }

            operatorsText[2].gameObject.SetActive(false);
            FortressButtons[3].gameObject.SetActive(false);
        }
        AnswerText.text = " = " + AnswerNeeded.ToString();

        SetDraggers();

        Defend.interactable = false;
    }


    void SetDraggers()
    {
        for (int i = 0; i < variableCount; i++)
        {
            int index = Random.Range(0, (variableCount));

            while (Draggers[index].DraggerAnswer > 0)
            {
                index = Random.Range(0, (variableCount));
            }

            Draggers[index].SetDragger((int)variablesUsed[i]);
        }
    }

    internal bool CheckDefenceReady()
    {

        for (int i = 0; i < variableCount; i++)
        {
            if (!FortressButtons[i].draggedIn)
            {
                Defend.interactable = false;
                return false;
            }
        }
        Defend.interactable = true;

        return true;
    }

    public void FortressDefend()
    {
        if (!CheckDefenceReady())
            return;


        List<int> summingNumbers = new List<int>();

        List<operators> summingOps;


        summingOps = new List<operators>(StoredOps);

        for (int i = 0; i < variableCount; i++)
        {
            summingNumbers.Add(FortressButtons[i].draggedIn.DraggerAnswer);
        }

        float answer = 0;

        if (mainOp == operators.AddSubMultDiv)
        {
            float total;

            total = summingNumbers[0] / summingNumbers[1];
            switch (summingOps[1])
            {
                case operators.Addition:
                    total = total + summingNumbers[2];
                    break;
                case operators.Subtraction:
                    total = total - summingNumbers[2];
                    break;
            }
            total = total * summingNumbers[3];

            answer = total;
        }
        else
        {
            while (summingOps.Count > 0)
            {
                if (summingOps.Contains(operators.Division))
                {
                    int i = summingOps.IndexOf(operators.Division);

                    summingNumbers[i] /= summingNumbers[(i + 1)];
                    summingNumbers.RemoveAt(i + 1);
                    summingOps.RemoveAt(i);
                    continue;
                }

                if (summingOps.Contains(operators.Multiplication))
                {
                    int i = summingOps.IndexOf(operators.Multiplication);

                    summingNumbers[i] *= summingNumbers[(i + 1)];
                    summingNumbers.RemoveAt(i + 1);
                    summingOps.RemoveAt(i);
                    continue;
                }

                if (summingOps.Contains(operators.Addition))
                {
                    int i = summingOps.IndexOf(operators.Addition);

                    summingNumbers[i] += summingNumbers[(i + 1)];
                    summingNumbers.RemoveAt(i + 1);
                    summingOps.RemoveAt(i);
                    continue;
                }
                if (summingOps.Contains(operators.Subtraction))
                {
                    int i = summingOps.IndexOf(operators.Subtraction);

                    summingNumbers[i] -= summingNumbers[(i + 1)];

                    summingNumbers.RemoveAt(i + 1);
                    summingOps.RemoveAt(i);
                    continue;
                }
            }
            answer = summingNumbers[0];

        }

        if (answer == AnswerNeeded)
        {
            if (patience.gameObject.activeSelf)
            {
                SetPatience();
            }

            bossMonster.CreateQuestion();
        }
        else
        {
            bossMonster.EnemyAttack();
        }
    }

    void ResetAll()
    {
        foreach (FortressButton item in FortressButtons)
        {
            item.ResetButton();
        }
        foreach (FortressDragger item in Draggers)
        {
            item.ResetDragger();

        }
    }

    internal void SetPatience(int a_PatienceSet=-1)
    {
        if (a_PatienceSet > 0)
        {
            PatienceLeft = a_PatienceSet;
            patience.SetActive(true);
            PatienceText.text = PatienceLeft.ToString();
            return;
        }
        PatienceLeft--;

        if (PatienceLeft < 0)
        {
            bossMonster.health = -2;
            bossMonster.CheckDeath(true);
            bossMonster.sprite.gameObject.SetActive(false);
        }

        patience.SetActive(true);
        PatienceText.text = PatienceLeft.ToString();
    }
}