using UnityEngine;
using UnityEngine.UI;

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

        player.SetTime(phase, 5);
    }

    void MultipleAnswers()
    {
        calculator.SetActive(false);

        foreach (TutMultipleAnswer item in answers)
        {
            item.setAnswer(-1);
        }
        int index = Random.Range(0, 6);




        answers[index].gameObject.SetActive(true);
        answers[index].setAnswer(enemyAnswerNeeded);


        int[] wrongAnswers = new int[2];
        wrongAnswers[0] = enemyAnswerNeeded + 1;
        wrongAnswers[1] = enemyAnswerNeeded - 1;

        for (int i = 1; i < 3; i++)
        {
            int wrongAnswer = wrongAnswers[(i - 1)];


            index = Random.Range(0, 6);
            while (answers[index].gameObject.activeSelf)
            {
                index = Random.Range(0, 6);
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

    public void interactableButtons(bool canInteract)
    {
        foreach (TutMultipleAnswer item in answers)
        {
            item.gameObject.GetComponent<Button>().interactable = canInteract;
        }
    }
}