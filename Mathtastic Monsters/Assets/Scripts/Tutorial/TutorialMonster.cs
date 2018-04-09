using UnityEngine;
using UnityEngine.UI;

public class TutorialMonster : MonoBehaviour
{
    public float health; //The current health of the monster, as set by the quizButton.
    float attack;


    public Healthbars healthBar; //visually display health

    public TutorialPlayer player;


    public GameObject monsterSpot;


    public TutorialCalculator calculator;


    bool enemyAttackPhase;

    public TutmultipleContainer container;

    combatFeedback feedback;

    public GameObject MonsterPrefab;
    Animator Animator;

    //Update healthbar as it changes.
    void Update()
    {        
    }

    //Monster's health is reduced by player's attack, possibly switching to won state.
    internal void MonsterHurt()
    {
        if (!enemyAttackPhase)
        {
            health -= player.PlayerAttack();

            Animator.Play("Hurt");

            CheckDeath();
        }

        MakeQuestion();
    }

    //Player is hurt, and a new question is built.
    internal void EnemyAttack()
    {
        if(!feedback)
        {
            feedback = FindObjectOfType<combatFeedback>();
        }

        if (enemyAttackPhase)
        {
            player.DamagePlayer(attack);

            Animator.Play("Attack");

            FindObjectOfType<StepManager>().ProgressTutorial();
        }
        else
        {
            feedback.DamageSet(SetFeedback.PlayerMissed);
        }
        MakeQuestion();
    }


    void CheckDeath()
    {
        healthBar.changeHealth(false, health);

        if (health <= 0)
        {
            Animator.Play("Death");

            FindObjectOfType<StepManager>().playerWon();
            return;
        }
    }


    //Called only when a quiz begins. Loads a question AND sets health/attack.
    public void LoadMonster()
    {
        if (!healthBar)
            healthBar = FindObjectOfType<Healthbars>();

        GameObject Mon = Instantiate(MonsterPrefab, monsterSpot.transform);
        Animator = Mon.GetComponent<Animator>();


        MakeQuestion();

        health = 6;
        attack = 1;

        healthBar.setMaxHealth(health, false);
    }

    //Uses given values to calculate a random sum and its components, then store and display them.
    internal void MakeQuestion(int newPhase=0)
    {


        int[] numbers = new int[2];
        int answer = -2;
        string oper = "";

        for (int i = 0; i < 2; i++)
        {
            numbers[i] = (int)UnityEngine.Random.Range(1, (4 + 1));
        }


        answer = numbers[0];
        for (int i = 1; i < 2; i++)
        {
            answer += numbers[i];
            oper = "+ ";
        }

        //if Answer is too low/too high, or requires rounding to solve, we try again.
        if (answer <= 1 || answer >= 9)
        {
            MakeQuestion(newPhase);
            return;
        }

        string answerNeeded = answer.ToString("F0");


        if (newPhase == 0)
            enemyAttackPhase = !enemyAttackPhase;
        else if (newPhase == 1)
            enemyAttackPhase = true;
        else
            enemyAttackPhase = false;

        container.SetMultiple(answer, enemyAttackPhase);

        string answerWords;

        answerWords = "   ";
        answerWords += numbers[0].ToString("F0");

        for (int i = 1; i < 2; i++)
        {
            answerWords += " " + oper + numbers[i].ToString("F0");

            answer += numbers[i];
        }

        answerWords += " = ";

        calculator.SetAnswer(answerNeeded, answerWords);
    }
}