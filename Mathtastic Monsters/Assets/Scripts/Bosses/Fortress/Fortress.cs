using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortress : MonoBehaviour
{

    /*
    public questionManager questionManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    bool CalculateBODMAS(QuizButton a_running, int failures, bool bossAttacking)
    {
        float[] randomised = new float[a_running.variableCount];

        //Randomise as many numbers as required, within range.
        for (int i = 0; i < a_running.variableCount; i++)
        {
            randomised[i] = (int)Random.Range(a_running.minNumber, (a_running.maxNumber + 1));
        }


        List<int> summingNumbers = new List<int>(5);


        List<operators> ops = ChooseOperators(a_running.Operator, (a_running.variableCount - 1));

        string[] operatorStrings = new string[ops.Count];

        for (int i = 0; i < operatorStrings.Length; i++)
        {
            switch (ops[i])
            {
                case operators.Addition:
                    operatorStrings[i] = "+ ";
                    break;
                case operators.Subtraction:
                    operatorStrings[i] = "- ";
                    break;
                case operators.Multiplication:
                    operatorStrings[i] = "x ";
                    break;
                case operators.Division:
                    operatorStrings[i] = "/ ";
                    break;
                default:
                    break;
            }
        }

        //Randomise as many numbers as required, within range.
        for (int i = 0; i < a_running.variableCount; i++)
        {
            summingNumbers.Add((int)randomised[i]);
        }


        while (ops.Count > 0)
        {
            if (ops.Contains(operators.Division))
            {
                int i = ops.IndexOf(operators.Division);

                summingNumbers[i] /= summingNumbers[(i + 1)];
                summingNumbers.RemoveAt(i + 1);
                ops.RemoveAt(i);
                continue;
            }

            if (ops.Contains(operators.Multiplication))
            {
                int i = ops.IndexOf(operators.Multiplication);

                summingNumbers[i] *= summingNumbers[(i + 1)];
                summingNumbers.RemoveAt(i + 1);
                ops.RemoveAt(i);
                continue;
            }

            if (ops.Contains(operators.Addition))
            {
                int i = ops.IndexOf(operators.Addition);

                summingNumbers[i] += summingNumbers[(i + 1)];
                summingNumbers.RemoveAt(i + 1);
                ops.RemoveAt(i);
                continue;
            }
            if (ops.Contains(operators.Subtraction))
            {
                int i = ops.IndexOf(operators.Subtraction);

                summingNumbers[i] -= summingNumbers[(i + 1)];

                summingNumbers.RemoveAt(i + 1);
                ops.RemoveAt(i);
                continue;
            }
        }
        int answer = summingNumbers[0];


        bool whole = questionManager.IsWhole(answer);


        //if Answer is too low/too high, or requires rounding to solve, we try again.
        if ((answer <= a_running.minAnswer || answer > a_running.maxAnswer || !whole) && failures < 20)
        {
            int failed = failures + 1;

            return CalculateBODMAS(a_running, failed, bossAttacking);

        }
        if (failures >= 20)
            Debug.Log("Failed");

        string answerNeeded = answer.ToString("F0");



        string answerWords;

        answerWords = "   ";
        answerWords += randomised[0].ToString("F0");

        for (int i = 1; i < a_running.variableCount; i++)
        {
            answerWords += "\n" + operatorStrings[(i - 1)] + randomised[i].ToString("F0");
        }

        answerWords += "\n= ";
        GetComponent<Text>().text = answerWords;

        calculator.answerNeeded = answerNeeded;

        return enemyPhase;
    }


    List<operators> ChooseOperators(operators main, int size)
    {
        int newOp = 0;

        List<operators> op = new List<operators>();

        for (int i = 0; i < size; i++)
        {

            switch (main)
            {
                case operators.AddSub:
                    newOp = Random.Range(0, 2);
                    break;
                case operators.AddSubMult:
                    newOp = Random.Range(0, 3);
                    break;
                case operators.AddSubMultDiv:
                    newOp = Random.Range(0, 4);
                    break;
                case operators.Fortress:
                    newOp = Random.Range(0, 4);
                    break;
                default:
                    break;
            }
            op.Add((operators)newOp);
        }
        return op;
    }
    */
}
