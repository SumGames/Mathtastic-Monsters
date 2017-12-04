using UnityEngine;
using UnityEngine.UI;

public class questionManager : MonoBehaviour
{

    public Calculator calculator;

    public multipleContainer container;

    //Uses given values to calculate a random sum and its components, then store and display them.
    internal bool makeQuestion(QuizButton a_running, bool resetTime = true, int overRide = 0)
    {
        if (calculator == null)
            calculator = FindObjectOfType<Calculator>();

        if (container == null)
            container = FindObjectOfType<multipleContainer>();

        float[] numbers = new float[a_running.variableCount];
        float answer = -2;
        string oper = "";

        //Randomise as many numbers as required, within range.
        for (int i = 0; i < a_running.variableCount; i++)
        {
            numbers[i] = (int)Random.Range(a_running.minNumber, (a_running.maxNumber + 1));
        }

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
                //If operator is 'x', second number is one of two possible numbers.
                numbers[1] = (int)UnityEngine.Random.Range(a_running.secondNumberMin, (a_running.secondNumberMax + 1));

                answer = numbers[0] * numbers[1];
                oper = "x ";
                break;

            case operators.Division:
                answer = numbers[0];
                numbers[1] = (int)UnityEngine.Random.Range(a_running.secondNumberMin, (a_running.secondNumberMax + 1));
                answer = numbers[0] / numbers[1];
                oper = "/ ";
                break;
            default:
                break;
        }

        bool rounding = a_running.preventRounding;

        if (a_running.preventRounding) //if box is ticked, need to make sure numbers don't require rounding up.
        {
            rounding = preventRounding(numbers, a_running, (int)answer);
        }


        bool whole = IsWhole(answer);


        //if Answer is too low/too high, or requires rounding to solve, we try again.
        if (answer < a_running.minAnswer || answer > a_running.maxAnswer || rounding || !whole)
        {
            return makeQuestion(a_running, resetTime, overRide);

        }

        string answerNeeded = answer.ToString("F0");

        bool enemyPhase = container.SetMultiple((int)answer, a_running, resetTime, overRide);

        string answerWords;

        answerWords = "   ";
        answerWords += numbers[0].ToString("F0");

        for (int i = 1; i < a_running.variableCount; i++)
        {
            answerWords += "\n" + oper + numbers[i].ToString("F0");

            answer += numbers[i];
        }

        answerWords += "\n= ";
        GetComponent<Text>().text = answerWords;

        calculator.answerNeeded = answerNeeded;

        return enemyPhase;
    }


    //Repeatedly run the Rounding function using different digit amounts.
    public bool preventRounding(float[] a_numbers, QuizButton a_running, int a_answer)
    {
        float checkingAnswer = a_answer;

        if (a_numbers[0] > a_answer)
        {
            checkingAnswer = a_numbers[0];
        }


        if (checkingAnswer > 1000)
        {
            if (rounding(a_numbers, a_running.Operator, 1000))
            {
                return true;
            }
        }
        if (checkingAnswer > 100)
        {
            if (rounding(a_numbers, a_running.Operator, 100))
            {
                return true;
            }
        }
        if (checkingAnswer > 10)
        {
            if (rounding(a_numbers, a_running.Operator, 10))
            {
                return true;
            }
        }

        return false;
    }


    //Checking if the calculation needs rounding. Returns true if yes.
    //a_operator is + or -, and a_digits as the number of digits of the comparison being made
    public bool rounding(float[] a_numbers, operators a_operator, int a_digits)
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
}
