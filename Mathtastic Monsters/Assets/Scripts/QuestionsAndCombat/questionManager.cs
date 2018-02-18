﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questionManager : MonoBehaviour
{

    public Calculator calculator;

    public multipleContainer container;

    public QuizButton button;

    public TransitionManager transition;

    internal string questionNeeded;

    bool justTransitioned;

    int failures = 0;

    void Update()
    {
        if (!transition)
            return;

        if (transition.transitionState == TransitionState.None && justTransitioned)
        {
            GetComponent<Text>().text = questionNeeded;
            justTransitioned = false;
        }
        else if (transition.transitionState != TransitionState.None)
        {
            if (GetComponent<Text>().text != "")
            {
                questionNeeded = GetComponent<Text>().text;
                GetComponent<Text>().text = "";
                justTransitioned = true;
            }
        }
    }


    //Uses given values to calculate a random sum and its components, then store and display them.
    internal bool MakeQuestion(QuizButton a_running, bool resetTime = true, OverRidePhases overRide = OverRidePhases.Default)
    {
        if (calculator == null)
            calculator = FindObjectOfType<Calculator>();

        if (container == null)
            container = FindObjectOfType<multipleContainer>();

        button = a_running;

        float[] numbers = new float[button.variableCount];
        float answer = -2;
        string oper = "";

        int rand = 1;

        //Randomise as many numbers as required, within range.
        for (int i = 0; i < a_running.variableCount; i++)
        {
            numbers[i] = (int)Random.Range(button.minNumber, (button.maxNumber + 1));
        }

        float multiple = 1;

        failures = 0;

        switch (a_running.Operator)
        {
            case operators.Addition:
                answer = numbers[0];
                for (int i = 1; i < a_running.variableCount; i++)
                {
                    answer += numbers[i];
                    oper = "+ ";
                }
                break;
            case operators.Subtraction:
                answer = numbers[0];
                for (int i = 1; i < a_running.variableCount; i++)
                {
                    answer -= numbers[i];
                    oper = "- ";
                }
                break;
            case operators.Multiplication:
                //If operator is 'x', second number is one of two possible numbers

                if (a_running.boss && a_running.validNumbers != 0)
                {
                    rand = Random.Range(0, (a_running.validNumbers));
                }
                else
                {
                    rand = Random.Range(0, (a_running.secondFixedNumber.Length));
                }
                numbers[1] = a_running.secondFixedNumber[rand];

                multiple = numbers[1];


                answer = numbers[0] * numbers[1];
                oper = "x ";
                break;
            case operators.Division:
                rand = Random.Range(0, (a_running.secondFixedNumber.Length));

                numbers[1] = a_running.secondFixedNumber[rand];

                multiple = numbers[1];

                answer = numbers[0] / numbers[1];
                oper = "÷ ";
                break;
            default:
                return CalculateBODMAS(a_running, 0, resetTime);

        }

        bool rounding = a_running.preventRounding;

        if (a_running.preventRounding) //if box is ticked, need to make sure numbers don't require rounding up.
        {
            rounding = PreventRounding(numbers, a_running, (int)answer);
        }


        bool whole = IsWhole(answer);


        //if Answer is too low/too high, or requires rounding to solve, we try again.
        if (answer < a_running.minAnswer || answer > a_running.maxAnswer || rounding || !whole)
        {
            return MakeQuestion(a_running, resetTime, overRide);

        }

        string answerNeeded = answer.ToString("F0");

        bool enemyPhase = container.SetMultiple((int)answer, a_running, resetTime, multiple, overRide);

        string answerWords;

        answerWords = "   ";
        answerWords += numbers[0].ToString("F0");

        for (int i = 1; i < a_running.variableCount; i++)
        {
            answerWords += "\n" + oper + numbers[i].ToString("F0");

            answer += numbers[i];
        }

        answerWords += "\n= ";

        justTransitioned = true;
        questionNeeded = answerWords;

        GetComponent<Text>().text = answerWords;

        calculator.answerNeeded = answerNeeded;

        return enemyPhase;
    }


    //Repeatedly run the Rounding function using different digit amounts.
    bool PreventRounding(float[] a_numbers, QuizButton a_running, int a_answer)
    {
        float checkingAnswer = a_answer;

        if (a_numbers[0] > a_answer)
        {
            checkingAnswer = a_numbers[0];
        }


        if (checkingAnswer > 1000)
        {
            if (Rounding(a_numbers, a_running.Operator, 1000))
            {
                return true;
            }
        }
        if (checkingAnswer > 100)
        {
            if (Rounding(a_numbers, a_running.Operator, 100))
            {
                return true;
            }
        }
        if (checkingAnswer > 10)
        {
            if (Rounding(a_numbers, a_running.Operator, 10))
            {
                return true;
            }
        }

        return false;
    }


    //Checking if the calculation needs rounding. Returns true if yes.
    //a_operator is + or -, and a_digits as the number of digits of the comparison being made
    bool Rounding(float[] a_numbers, operators a_operator, int a_digits)
    {
        float[] shortened = new float[a_numbers.Length];
        a_numbers.CopyTo(shortened, 0);

        for (int i = 0; i < a_numbers.Length; i++)
        {
            shortened[i] = a_numbers[i] % a_digits;
        }
        float total = shortened[0];
        switch (a_operator)
        {
            case operators.Addition:
                for (int i = 1; i < a_numbers.Length; i++)
                {
                    total += shortened[i];
                }
                if (total >= a_digits)
                {
                    return true;
                }
                break;

            case operators.Subtraction:
                for (int i = 1; i < a_numbers.Length; i++)
                {
                    total -= shortened[i];
                }
                if (total < (a_digits / 10))
                {
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }

    bool IsWhole(float answer)
    {
        if (Mathf.Floor(answer) == answer)
        {
            return true;

        }
        return false;
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

    bool CalculateBODMAS(QuizButton a_running, int failures, bool resetTime = true)
    {
        float[] randomised = new float[a_running.variableCount];

        //Randomise as many numbers as required, within range.
        for (int i = 0; i < a_running.variableCount; i++)
        {
            randomised[i] = (int)Random.Range(button.minNumber, (button.maxNumber + 1));
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

        bool rounding = a_running.preventRounding;

        if (a_running.preventRounding) //if box is ticked, need to make sure numbers don't require rounding up.
        {
            rounding = PreventRounding(randomised, a_running, answer);
        }


        bool whole = IsWhole(answer);


        //if Answer is too low/too high, or requires rounding to solve, we try again.
        if ((answer <= a_running.minAnswer || answer > a_running.maxAnswer || rounding || !whole) && failures < 20)
        {
            int failed = failures + 1;

            return CalculateBODMAS(a_running, failed, resetTime);

        }
        if (failures >= 20)
            Debug.Log("Failed");

        string answerNeeded = answer.ToString("F0");

        bool enemyPhase = container.SetMultiple((int)answer, a_running, resetTime, 1, OverRidePhases.Default);

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
}