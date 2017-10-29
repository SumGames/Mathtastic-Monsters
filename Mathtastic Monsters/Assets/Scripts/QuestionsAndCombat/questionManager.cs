using UnityEngine;
using UnityEngine.UI;

public class questionManager : MonoBehaviour 
{

    public Calculator calculator;

    public multipleContainer container;

    //Uses given values to calculate a random sum and its components, then store and display them.
    internal void makeQuestion(QuizButton a_running, bool phase)
    {
        if (calculator == null)
            calculator = FindObjectOfType<Calculator>();

        if (container == null)
            container = FindObjectOfType<multipleContainer>();

        int[] numbers = new int[a_running.variableCount];
        int answer = -2;
        string oper = "";

		//Randomise as many numbers as required, within range.
        for (int i = 0; i < a_running.variableCount; i++)
        {
            numbers[i] = (int)UnityEngine.Random.Range(a_running.minNumber, (a_running.maxNumber + 1));
        }

        switch (a_running.Operator)
        {
            case '+':
                answer = numbers[0];
                for (int i = 1; i < a_running.variableCount; i++)
                {
                    answer += numbers[i];
                    oper = "+ ";
                }
                break;
            case '-':
                answer = numbers[0];
                for (int i = 1; i < a_running.variableCount; i++)
                {
                    answer -= numbers[i];
                    oper = "- ";
                }
                break;
            case 'x':
			    //If operator is 'x', second number is one of two possible numbers.
				numbers[1] = (int)UnityEngine.Random.Range(a_running.secondNumberMin, (a_running.secondNumberMax + 1));
			
                answer = numbers[0] * numbers[1];
                oper = "x ";
                break;
            default:
                answer = numbers[0];
                for (int i = 1; i < a_running.variableCount; i++)
                {
                    answer /= numbers[i];
                    oper = "/ ";
                }
                break;
        }

        bool rounding = a_running.rounding;

        if (a_running.rounding) //if box is ticked, need to make sure numbers don't require rounding up.
        {
            rounding = preventRounding(numbers, a_running, answer);
        }

        //if Answer is too low/too high, or requires rounding to solve, we try again.
        if (answer <= a_running.minAnswer || answer >= a_running.maxAnswer || rounding)
        {
            makeQuestion(a_running, phase);
            return;
        }

        string answerNeeded = answer.ToString("F0");

        container.SetMultiple(answer, phase, a_running);


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
    }


    //Repeatedly run the Rounding function using different digit amounts.
    public bool preventRounding(int[] a_numbers, QuizButton a_running, int a_answer)
    {
        if (a_answer > 1000)
        {
            if (rounding(a_numbers, a_running.Operator, 1000))
                return true;
        }
        if (a_answer > 100)
        {
            if (rounding(a_numbers, a_running.Operator, 100))
                return true;
        }
        if (a_answer > 10)
        {
            if (rounding(a_numbers, a_running.Operator, 10))
                return true;
        }

        return false;
    }


    //Checking if the calculation needs rounding. Returns true if yes.
    //a_operator is + or -, and a_digits as the number of digits of the comparison being made
    public bool rounding(int[] a_numbers, char a_operator, int a_digits)
    {
        int[] shortened = new int[a_numbers.Length];
        a_numbers.CopyTo(shortened, 0);

        for (int i = 0; i < a_numbers.Length; i++)
        {
            shortened[i] = a_numbers[i] % a_digits;
        }
        int total = shortened[0];
        switch (a_operator)
        {
            case '+':
                for (int i = 1; i < a_numbers.Length; i++)
                {
                    total += shortened[i];
                }
                if (total >= a_digits)
                {
                    return true;
                }
                break;

            case '-':
                for (int i = 1; i < a_numbers.Length; i++)
                {
                    total -= shortened[i];
                }
                if (total <= (a_digits / 10))
                {
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }

}
