using UnityEngine;
using UnityEngine.UI;

public class TutmultipleContainer : MonoBehaviour
{

    public int enemyAnswerNeeded;

    public GameObject calculator;

    public TutMultipleAnswer[] answers;

    public TutorialPlayer player;

    TutorialMonster monster;

    internal TutMultipleAnswer SelectedAnswer;

    public Button submit;

    public GameObject scalar;

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
        submit.interactable = false;


        enemyAnswerNeeded = answer;

        if (phase)
        {
            MultipleAnswers();
        }
        else
        {
            DisableMultiple();
        }

        player.SetTime(phase, 60);
    }

    void MultipleAnswers()
    {
        calculator.SetActive(false);
        gameObject.SetActive(true);
        scalar.gameObject.SetActive(true);
        foreach (TutMultipleAnswer item in answers)
        {
            item.setAnswer(-1);
        }
        int index = Random.Range(0, 3);


        submit.gameObject.SetActive(true);

        answers[index].gameObject.SetActive(true);
        answers[index].setAnswer(enemyAnswerNeeded);


        int[] wrongAnswers = new int[2];
        wrongAnswers[0] = enemyAnswerNeeded + 1;
        wrongAnswers[1] = enemyAnswerNeeded - 1;

        for (int i = 1; i < 3; i++)
        {
            int wrongAnswer = wrongAnswers[(i - 1)];


            index = Random.Range(0, 3);
            while (answers[index].gameObject.activeSelf)
            {
                index = Random.Range(0, 3);
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
        submit.gameObject.SetActive(false);
        scalar.gameObject.SetActive(false);
    }

    public void interactableButtons(bool canInteract)
    {
        foreach (TutMultipleAnswer item in answers)
        {
            item.gameObject.GetComponent<Button>().interactable = canInteract;
        }
    }


    public void submitAnswer()
    {
        if (SelectedAnswer == null)
        {
            submit.interactable = false;
            return;
        }

        if (!monster)
        {
            monster = FindObjectOfType<TutorialMonster>();
        }



        if (SelectedAnswer.getAnswer() == enemyAnswerNeeded)
        {
            monster.MonsterHurt();
        }
        else
        {
            monster.EnemyAttack();
        }

        submit.interactable = false;
        SelectedAnswer.GetComponent<Image>().color = Color.white;
        SelectedAnswer = null;

    }
}