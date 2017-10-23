using UnityEngine;

public class TutmultipleContainer : MonoBehaviour
{

    public int enemyAnswerNeeded;

    public GameObject calculator;

    public TutMultipleAnswer[] answers;

    public TutorialPlayer player;

    // Use this for initialization
    void Start()
    {
        DisableMultiple();
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void SetMultiple(int answer, bool phase)
    {
        enemyAnswerNeeded = answer;

        if (phase)
        {
            MultipleAnswers();
        }
        else
        {
            DisableMultiple();
        }

        player.setTime(phase, 5);
    }

    void MultipleAnswers()
    {
        calculator.SetActive(false);

        foreach (TutMultipleAnswer item in answers)
        {
            item.setAnswer(-1);
        }
        int index = Random.Range(0, 8);




        answers[index].gameObject.SetActive(true);
        answers[index].setAnswer(enemyAnswerNeeded);

        for (int i = 1; i < 2; i++)
        {
            int wrongAnswer = -3;
            while (wrongAnswer <= 1 || wrongAnswer >= 5)// || CheckMultiple(a_running, wrongAnswer))
            {
                int range = Random.Range(-1, 2);
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

    void DisableMultiple()
    {
        foreach (TutMultipleAnswer item in answers)
        {
            item.gameObject.SetActive(false);
        }
        calculator.SetActive(true);
    }
}