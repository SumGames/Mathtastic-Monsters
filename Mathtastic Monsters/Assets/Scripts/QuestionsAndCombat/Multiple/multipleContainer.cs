using UnityEngine;

public class multipleContainer : MonoBehaviour
{

    public int enemyAnswerNeeded;

    public GameObject calculator;

    public MultipleAnswer[] answers;

    public Player player;

    // Use this for initialization
    void Start()
    {
        DisableMultiple();
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void SetMultiple(int answer, bool phase, QuizButton a_running)
    {
        enemyAnswerNeeded = answer;

        if (phase && a_running != null)
        {
            MultipleAnswers(a_running);
        }
        else
        {
            DisableMultiple();
        }
        if (a_running)
            player.setTime(phase, a_running.enemPhaseTime);
    }

    void MultipleAnswers(QuizButton a_running)
    {
        calculator.SetActive(false);

        foreach (MultipleAnswer item in answers)
        {
            item.setAnswer(-1);
        }
        int index = Random.Range(0, 8);



        if (a_running.enemyChoices > 8)
            a_running.enemyChoices = 8;


        answers[index].gameObject.SetActive(true);
        answers[index].setAnswer(enemyAnswerNeeded);

        for (int i = 1; i < a_running.enemyChoices; i++)
        {
            int wrongAnswer = -3;
            while (wrongAnswer <= a_running.minAnswer || wrongAnswer >= a_running.maxAnswer)// || CheckMultiple(a_running, wrongAnswer))
            {
                int range = Random.Range(-a_running.enemyAnswerRange, a_running.enemyAnswerRange);
                wrongAnswer = enemyAnswerNeeded + range;
            }


            index = Random.Range(0, 8);
            while (answers[index].getAnswer() != -1)
            {
                index = Random.Range(0, 8);
            }
            answers[index].gameObject.SetActive(true);
            answers[index].setAnswer(wrongAnswer);
        }
    }

    //Loop if we return true.
    bool CheckMultiple(QuizButton button, int result)
    {
        int number = 1;

        foreach (MultipleAnswer item in answers)
        {
            if (result == item.getAnswer())
                number++;
        }

        //No duplicates.
        if (number == 1)
        {
            return false;
        }

        //Duplicates, but too many to avoid getting more :(
        if (number >= button.enemyAnswerRange)
        {
            return false;
        }

        return true;
    }


    void DisableMultiple()
    {
        foreach (MultipleAnswer item in answers)
        {
            item.gameObject.SetActive(false);
        }
        calculator.SetActive(true);
    }
}