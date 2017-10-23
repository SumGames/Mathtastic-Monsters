using UnityEngine;
using UnityEngine.UI;

public class TutorialMonster : MonoBehaviour
{
    float health; //The current health of the monster, as set by the quizButton.
    float attack;


    public Slider healthBar; //visually display health

    public TutorialPlayer player;


    public GameObject monsterSpot;
    public GameObject sprite;


    public TutorialCalculator calculator;


    bool phase = true;

    public TutmultipleContainer container;


    //Update healthbar as it changes.
    void Update()
    {
        healthBar.value = health;
    }

    //Monster's health is reduced by player's attack, possibly switching to won state.
    internal void MonsterHurt()
    {
        health -= player.PlayerAttack();

        CheckDeath();


        MakeQuestion();
    }

    //Player is hurt, and a new question is built.
    internal void EnemyAttack()
    {
        player.DamagePlayer(attack);
        MakeQuestion();

        FindObjectOfType<StepManager>().ProgressTutorial();
    }


    void CheckDeath()
    {
        if (health < 0)
        {

            FindObjectOfType<StepManager>().playerWon();

            return;
        }
    }


    //Called only when a quiz begins. Loads a question AND sets health/attack.
    public void LoadMonster()
    {


        MakeQuestion();

        health = 6;
        attack = 1;

        healthBar.maxValue = health;
    }

    //Uses given values to calculate a random sum and its components, then store and display them.
    internal void MakeQuestion()
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
            MakeQuestion();
            return;
        }

        string answerNeeded = answer.ToString("F0");


        phase = !phase;
        container.SetMultiple(answer, phase);


        string answerWords;

        answerWords = "   ";
        answerWords += numbers[0].ToString("F0");

        for (int i = 1; i < 2; i++)
        {
            answerWords += "\n" + oper + numbers[i].ToString("F0");

            answer += numbers[i];
        }

        answerWords += "\n= ";

        calculator.SetAnswer(answerNeeded, answerWords);
    }
}