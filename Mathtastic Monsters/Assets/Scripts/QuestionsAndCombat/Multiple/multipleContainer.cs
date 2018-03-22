using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public enum TransitioningObjects
{
    SwapToMultiple,
    SwapToCalculator,
    JustSwap
}

public class multipleContainer : MonoBehaviour
{

    public int enemyAnswerNeeded;

    public GameObject calculator;

    public MultipleAnswer[] answers;

    public Player player;

    playerAbilities playerAbilities;

    public List<int> answersList;

    public int attacksPherPhase = 1;

    public int attacks = 1;
    bool attacking;


    public TransitionManager transitionManager;




    public MultipleAnswer SelectedAnswer;
    MonsterManager MonsterManager;
    public Button submit;

    public GameObject Defend;

    public GameObject MultipleBackground;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void SetAttacks(bool boss, bool startWithAttack)
    {
        if (!playerAbilities)
            playerAbilities = FindObjectOfType<playerAbilities>();

        bool doubleAttck = playerAbilities.DoubleStrike();

        if (doubleAttck && !boss)
        {
            attacksPherPhase = 2;
        }
        else
        {
            attacksPherPhase = 1;
        }
        if (startWithAttack || !boss)
        {
            attacks = attacksPherPhase;
        }
        else
            attacks = 0;
    }


    internal bool SetMultiple(int answer, QuizButton a_running, float multiple, bool BossAttacking=false)
    {
        

        enemyAnswerNeeded = answer;
        if (!BossAttacking)
        {
            if (attacks > 0)
            {
                attacking = false;
                attacks--;

            }
            else
            {
                attacks = attacksPherPhase;
                attacking = true;
            }
        }
        else
        {
            attacking = false;
        }

        bool enemyPhase = false;

        if (attacking)
        {
            enemyPhase = true;
            attacks = attacksPherPhase;
            MultipleAnswers(a_running, multiple);
        }
        else
        {
            enemyPhase = false;
            attacks--;
            TransitionButtons(false, a_running);
        }

        if (a_running)
        {
            player.SetTime(enemyPhase, a_running.enemPhaseTime);
        }

        player.EndTurn(enemyPhase);

        return enemyPhase;
    }

    void MultipleAnswers(QuizButton a_running, float multiple)
    {
        SelectedAnswer = null;
        submit.interactable = false;

        DisableMultipleChoiceButtons();

        MultipleBackground.SetActive(true);

        TransitionButtons(true, a_running);

        foreach (MultipleAnswer item in answers)
        {
            item.setAnswer(-1);
        }
        int index = Random.Range(0, 6);

        answersList = new List<int>
        {
            enemyAnswerNeeded
        };


        if (a_running.enemyChoices > 6)
            a_running.enemyChoices = 6;

        if (Defend)
            Defend.gameObject.SetActive(true);

        answers[index].gameObject.SetActive(true);
        answers[index].setAnswer(enemyAnswerNeeded);

        for (int i = 1; i < a_running.enemyChoices; i++)
        {
            int wrongAnswer = -3;
            while (wrongAnswer <= a_running.minAnswer || CheckMultiple(a_running, wrongAnswer))
            {
                int range = Random.Range(-a_running.enemyAnswerRange, a_running.enemyAnswerRange);
                wrongAnswer = enemyAnswerNeeded + range;
            }

            index = Random.Range(0, 6);
            while (answers[index].getAnswer() != -1)
            {
                index = Random.Range(0, 6);
            }
            answers[index].gameObject.SetActive(true);
            answers[index].setAnswer(wrongAnswer);
        }
    }

    //Loop if we return true.
    bool CheckMultiple(QuizButton button, int result)
    {
        bool dupes = false;

        foreach (MultipleAnswer item in answers)
        {
            if (result == item.getAnswer())
                dupes = true;
        }

        //No duplicates.
        if (dupes == false)
        {
            answersList.Add(result);
            return false;
        }

        //Duplicates, but too many to avoid getting more :(
        if (answersList.Count >= button.enemyAnswerRange * 2)
        {
            return false;
        }

        return true;
    }


    internal void RemoveSingle()
    {
        MultipleAnswer removing = null;

        foreach (MultipleAnswer item in answers)
        {
            if (item.getAnswer() != enemyAnswerNeeded && item.getAnswer() > 0)
            {
                removing = item;
            }
        }
        if (removing)
        {
            removing.setAnswer(-2);
            removing.gameObject.SetActive(false);
        }
    }

    void DisableMultipleChoiceButtons()
    {
        MultipleBackground.SetActive(false);

        if (Defend)
            Defend.gameObject.SetActive(false);

        foreach (MultipleAnswer item in answers)
        {
            item.gameObject.SetActive(false);
        }
    }

    internal void DisableThisAndCalculator()
    {
        DisableMultipleChoiceButtons();
        calculator.SetActive(false);
    }


    void TransitionButtons(bool both, QuizButton a_Button = null)
    {

        if (transitionManager && PlayerPrefs.GetInt("Transitions", 1) == 1)
        {
            if (a_Button == null || a_Button.boss)
            {
                if (both)
                    calculator.SetActive(false);
                else
                {
                    calculator.SetActive(true);
                }

                transitionManager.TransitionContainers(TransitioningObjects.JustSwap);
                return;
            }
            calculator.SetActive(true);

            if (both)
            {
                transitionManager.TransitionContainers(TransitioningObjects.SwapToMultiple);
            }
            else
            {
                transitionManager.TransitionContainers(TransitioningObjects.SwapToCalculator);
            }
        }
        else
        {

            DisableMultipleChoiceButtons();

            foreach (MultipleAnswer item in answers)
            {
                item.gameObject.SetActive(false);
            }

            if (both)
                calculator.SetActive(false);
            else
            {
                calculator.SetActive(true);
            }

        }
    }

    public void submitAnswer()
    {
        if (SelectedAnswer == null)
        {
            submit.interactable = false;
            return;
        }
        if (!MonsterManager)
        {
            MonsterManager = FindObjectOfType<MonsterManager>();
        }

        if (SelectedAnswer.getAnswer() == enemyAnswerNeeded)
        {
            MonsterManager.currentEnemy.MonsterHurt();
        }
        else
        {
            MonsterManager.currentEnemy.EnemyAttack();
        }

        submit.interactable = false;
        SelectedAnswer.GetComponent<Image>().color = Color.white;
        SelectedAnswer = null;

    }
}