using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questionManager : MonoBehaviour
{

    public Calculator calculator;

    public multipleContainer container;

    public QuizButton button;

    public TransitionManager transition;

    internal string questionNeeded;


    public float answer;

    public Text[] texts;

    float previousAnswer = -1;

    void Update()
    {
        if (!transition)
            return;
    
        if (transition.transitionState == TransitionState.None)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].text = questionNeeded;
                texts[i].enabled = true;

            }

        }
        else if (transition.transitionState != TransitionState.None)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].enabled = false;

            }

        }
    }


    //Uses given values to calculate a random sum and its components, then store and display them.
    internal bool MakeQuestion(QuizButton a_running, bool bossAttacking = false)
    {
        if (calculator == null)
            calculator = FindObjectOfType<Calculator>();

        if (container == null)
            container = FindObjectOfType<multipleContainer>();

        button = a_running;

        float[] numbers = new float[button.variableCount];
        answer = -2;
        string oper = "";

        int rand = 1;

        //Randomise as many numbers as required, within range.
        for (int i = 0; i < a_running.variableCount; i++)
        {
            numbers[i] = (int)Random.Range(button.minNumber, (button.maxNumber + 1));
        }

        float multiple = 1;


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
                return CalculateBODMAS(a_running, 0, bossAttacking);

        }

        bool rounding = a_running.preventRounding;

        if (a_running.preventRounding) //if box is ticked, need to make sure numbers don't require rounding up.
        {
            rounding = PreventRounding(numbers, a_running, (int)answer);
        }


        bool whole = IsWhole(answer);


        //if Answer is too low/too high, or requires rounding to solve, we try again.
        if (answer < a_running.minAnswer || answer > a_running.maxAnswer || rounding || !whole || answer == previousAnswer)
        {
            return MakeQuestion(a_running, bossAttacking);
        }
        previousAnswer = answer;

        string answerNeeded = answer.ToString("F0");

        bool enemyPhase = container.SetMultiple((int)answer, a_running, multiple, bossAttacking);

        string answerWords;

        answerWords = "   ";
        answerWords += numbers[0].ToString("F0");

        for (int i = 1; i < a_running.variableCount; i++)
        {
            answerWords += oper + numbers[i].ToString("F0");

            answer += numbers[i];
        }

        answerWords += "= ";

        questionNeeded = answerWords;

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

    internal bool IsWhole(float answer)
    {
        if (Mathf.Floor(answer) == answer)
        {
            return true;

        }
        return false;
    }


    internal List<operators> ChooseOperators(operators main, int size)
    {
        int newOp = 0;

        List<operators> op = new List<operators>();

        switch (main)
        {
            case operators.AddSub:
                for (int i = 0; i < size; i++)
                {
                    newOp = Random.Range(0, 2);
                    op.Add((operators)newOp);
                }
                break;
            case operators.AddSubMult:
                op.Add(operators.Multiplication);
                newOp = Random.Range(0, 2);
                op.Add((operators)newOp);
                break;
            default:
                op.Add(operators.Division);
                newOp = Random.Range(0, 2);
                op.Add((operators)newOp);
                op.Add(operators.Multiplication);
                break;
        }

        return op;
    }

    public List<operators> ops;

    bool CalculateBODMAS(QuizButton a_running, int failures, bool bossAttacking)
    {
        List<float> randomised = new List<float>(5);

        List<float> summingNumbers = new List<float>(5);


        ops = ChooseOperators(a_running.Operator, (a_running.variableCount - 1));



        randomised = randomiseBODMASNumbers(a_running);



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
            summingNumbers.Add(randomised[i]);
        }

        if (a_running.Operator == operators.AddSubMultDiv)
        {

            float total;

            total = summingNumbers[0] / summingNumbers[1];
            switch (ops[1])
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
            for (int i = 0; i < ops.Count; i++)
            {
                switch (ops[i])
                {
                    case operators.Addition:
                        summingNumbers[0] += summingNumbers[i + 1];
                        break;
                    case operators.Subtraction:
                        summingNumbers[0] -= summingNumbers[i + 1];
                        break;
                    case operators.Multiplication:
                        summingNumbers[0] *= summingNumbers[i + 1];
                        break;
                    default:
                        break;
                }
                if (summingNumbers[0] < 0)
                    return CalculateBODMAS(a_running, failures, bossAttacking);

            }
            answer = summingNumbers[0];

        }

        bool whole = IsWhole(answer);


        //if Answer is too low/too high, or requires rounding to solve, we try again.
        if ((answer <= a_running.minAnswer || answer > a_running.maxAnswer || !whole) && failures < 20)
        {
            int failed = failures + 1;

            return CalculateBODMAS(a_running, failed, bossAttacking);

        }
        if (failures >= 20)
            Debug.Log("Failed");

        string answerNeeded = answer.ToString("F0");

        bool enemyPhase = container.SetMultiple((int)answer, a_running, 1, bossAttacking);

        string answerWords;

        if (a_running.Operator == operators.AddSubMultDiv)
        {
            answerWords = BODMASDivisionQuestion(operatorStrings, randomised);
        }
        else
        {
            answerWords = "   ";
            answerWords += randomised[0].ToString("F0");

            for (int i = 1; i < a_running.variableCount; i++)
            {
                answerWords += operatorStrings[(i - 1)] + randomised[i].ToString("F0");
            }

        }
        answerWords += "= ";
        calculator.answerNeeded = answerNeeded;

        questionNeeded = answerWords;

        return enemyPhase;
    }

    string BODMASDivisionQuestion(string[] a_opStrings, List<float> Randomised)
    {
        string returning = "(" + Randomised[0] + " / " + Randomised[1] + " " + a_opStrings[1] + " " + Randomised[2] + ")x " + Randomised[3];


        return returning;
    }


    internal List<float> randomiseBODMASNumbers(QuizButton a_button)
    {
        int newop;
        List<float> returning = new List<float>(a_button.variableCount);

        switch (a_button.Operator)
        {
            case operators.AddSub:
                //Randomise as many numbers as required, within range.
                for (int i = 0; i < a_button.variableCount; i++)
                {
                    if (a_button.Operator == operators.AddSub)
                    {
                        int test = (int)Random.Range(a_button.minNumber, (a_button.maxNumber));
                        returning.Add(test);

                    }
                }

                break;
            case operators.AddSubMult:
                newop = Random.Range(0, a_button.secondFixedNumber.Length);
                returning.Add(a_button.secondFixedNumber[newop]);

                newop = Random.Range(1, 12);
                returning.Add(newop);

                returning.Add((int)Random.Range(a_button.minNumber, (a_button.maxNumber + 1)));
                break;
            default:
                returning.Add((int)Random.Range(a_button.minNumber, (a_button.maxNumber + 1)));

                newop = Random.Range(0, a_button.secondFixedNumber.Length);
                returning.Add(a_button.secondFixedNumber[newop]);

                newop = Random.Range(1, 12);
                returning.Add(newop);

                newop = Random.Range(1, 12);
                returning.Add(newop);

                break;
        }
        return returning;
    }
}