using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum OverRidePhases
{
    Default,
    EnemyAttack,
    EnemyDefend

}


public class BossMonster : Monster
{
    operators Operator; //Our current operator affects not just question, but mechanic/gimmick.

    BossButton m_button;

    //We'll make a few answers without the calculator, so this lets us do this without it.
    internal int answerNeeded;
    public questionManager text;
    //Addition boss has a special multiple choice.
    public AdditionContainer additionContainer;

    combatFeedback feedback;

    float depth = -5;



    //The lowest point our monster can reach.
    public GameObject subtractionBottom;


    //The spot the Addition boss emerges, or any other boss hits.
    public Vector3 highSpot;


    //Used in addition with container.
    internal int bossSpacing;

    public BossDivision bossDivision;

    public MultiplicationContainer multiplicationContainer;

    // Use this for initialization
    void Start()
    {
        bar = FindObjectOfType<Healthbars>();

        manager = FindObjectOfType<ParentsStateManager>();
    }

    //Update healthbar as it changes.
    void Update()
    {

    }


    //Player was wrong or so slow, enemy gets an attack!
    internal override void EnemyAttack()
    {

        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();

        if (Operator == operators.Subtraction) //Player might not take damage, or enemy might retreat!
        {
            EnemyAttackSubtraction();
            return;
        }

        feedback.DamageSet(SetFeedback.PlayerHit);


        player.DamagePlayer(1);


        CheckDeath();

        Debug.Log("Player hit");


        CreateQuestion();


    }

    void EnemyAttackSubtraction()
    {
        if (depth == 0) //Player failed at surface. Player takes no damage, but Timer and enemy retreats.
        {
            feedback.DamageSet(SetFeedback.PlayerMissed);
            depth = m_button.maxDepth;
            CreateQuestion();
            return;
        }

        if (player.returnTimer() <= 0) //Timer hit 0. Player is damaged, enemy submerges and timer resets.
        {
            depth = m_button.maxDepth;
            CreateQuestion();
            player.currentHealth -= m_button.MonsterAttack;
            CheckDeath();

            feedback.DamageSet(SetFeedback.PlayerHit);
            return;
        }
        //else player simply got any other answer wrong.

        player.DamagePlayer(1);
        feedback.DamageSet(SetFeedback.PlayerHit);

        CreateSubtraction(true, false);
    }

    //Player attacks monster.
    internal override void MonsterHurt()
    {
        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();



        if (Operator == operators.Division && enemyPhase)
        {
            feedback.DamageSet(SetFeedback.PlayerDodged);
            CreateQuestion();
            return;
        }


        if (Operator == operators.Subtraction)
        {
            SubtractionAttacked();
            return;
        }

        //Addition boss gets harder with each phase.
        if (Operator == operators.Addition)
        {
            m_button.enemyChoices++;
            m_button.enemyAnswerRange += 1;
            bossSpacing += 2;
        }

        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();

        feedback.DamageSet(SetFeedback.EnemyHit);

        --health;

        bar.changeHealth(false, health);

        CreateQuestion();

        CheckDeath();
    }

    //Subtraction boss sometimes ignores damage, but just floats up.
    void SubtractionAttacked()
    {
        if (depth >= 0) //Depth is 0. We punch him in the face, and he retreats.
        {
            feedback.DamageSet(SetFeedback.EnemyHit);

            health--;

            bar.changeHealth(false, health);

            depth = m_button.maxDepth;

            CreateQuestion();
            CheckDeath();
            return;
        }

        else //No damage, enemy grows closer.
        {
            feedback.DamageSet(SetFeedback.PlayerDodged);

            depth++;

            CreateSubtraction(true, false);
            return;
        }

    }

    //Monster is initiated.
    public override void loadMonster()
    {
        highSpot = monsterSpot.transform.position;

        questions = FindObjectOfType<questionManager>();

        if (!multipleContainer)
            multipleContainer = FindObjectOfType<multipleContainer>();

        multipleContainer.SetAttacks(true, false);


        multipleContainer.DisableThisAndCalculator();

        m_button = (BossButton)parent.quizRunning;

        Operator = parent.quizRunning.Operator;


        if (Operator == operators.Addition) //Addition resets in difficulty.
        {
            m_button.enemyChoices = 2;
            m_button.enemyAnswerRange = 3;
            bossSpacing = 4;
        }

        if (sprite != null)
        {
            Destroy(sprite.gameObject);
        }
        if (parent.quizRunning.monsterArt != null)
        {
            sprite = Instantiate(parent.quizRunning.monsterArt, monsterSpot.transform, false);
            sprite.transform.localScale = parent.quizRunning.monsterArt.transform.localScale;
            animator = sprite.GetComponent<Animator>();
        }

        enemyPhase = false;


        health = parent.quizRunning.MonsterHealth;
        attack = parent.quizRunning.MonsterAttack;

        bar.setMaxHealth(health, false);


        if (!music)
            music = FindObjectOfType<MusicManager>();

        if (music)
            music.SetCombatMusic(parent.quizRunning.Operator, parent.quizRunning.boss);


        CreateQuestion();

    }

    //Choose the type of question we're going to make from this type.
    public void CreateQuestion()
    {
        bossDivision.gameObject.SetActive(false);


        additionContainer.gameObject.SetActive(false);
        multiplicationContainer.gameObject.SetActive(false);

        switch (Operator)
        {
            case operators.Addition:
                CreateAddition();
                break;
            case operators.Subtraction:
                CreateSubtraction(true,true);
                break;
            case operators.Multiplication:
                CreateMultiplication();
                break;
            case operators.Division:
                CreateDivision();
                break;

            default:
                break;
        }


    }

    //Enemy moves from bottom of ocean.
    //Most new questions won't actually reset the timer.
    void CreateSubtraction(bool EnemyAttackingPhase, bool resetTime)
    {
        OverRidePhases over;

        if (EnemyAttackingPhase)
            over = OverRidePhases.EnemyAttack;
        else
            over = OverRidePhases.EnemyDefend;


        EnemyAttackingPhase = questions.MakeQuestion(m_button, false, over);

        float positionY = Mathf.Lerp(highSpot.y, subtractionBottom.transform.position.y, ((depth * -1) / 3));

        monsterSpot.transform.position = new Vector3(monsterSpot.transform.position.x, positionY, monsterSpot.transform.position.z);

        if (resetTime)
        {
            Debug.Log("Set time to " + m_button.enemPhaseTime);
            player.SetTime(true, m_button.enemPhaseTime);

        }
    }

    //We're creating a simple question like a normal monster one, but we're going to answer it using the addition container instead of anything else.
    void CreateAddition()
    {        
        additionContainer.gameObject.SetActive(true);

        int[] numbers = new int[m_button.variableCount];
        for (int i = 0; i < m_button.variableCount; i++)
        {
            numbers[i] = -2;
        }

        int first = Random.Range(0, m_button.variableCount);

        numbers[first] = (int)UnityEngine.Random.Range(m_button.minNumber, (m_button.maxNumber + 1));

        answerNeeded = numbers[first];


        //Randomise as many numbers as required, within range.
        for (int i = 0; i < m_button.variableCount; i++)
        {
            if (numbers[i] == -2)
                numbers[i] = (int)UnityEngine.Random.Range(m_button.minNumber, (m_button.maxNumber + 1));
        }


        int result = numbers[0];
        for (int i = 1; i < m_button.variableCount; i++)
        {
            result += numbers[i];
        }

        //if Answer is too low/too high, or requires rounding to solve, we try again.
        if (result <= m_button.minAnswer || result >= m_button.maxAnswer)
        {
            CreateAddition();
            return;
        }

        additionContainer.MultipleAnswers(this, m_button);

        string answerWords;

        answerWords = "   ";

        if (first == 0)
        {
            answerWords += "__";
        }
        else
            answerWords += numbers[0].ToString("F0");

        for (int i = 1; i < m_button.variableCount; i++)
        {

            if (first == i)
                answerWords += "\n+ __";
            else
                answerWords += "\n+ " + numbers[i].ToString("F0");
        }

        answerWords += "\n= " + result.ToString();

        questions.GetComponent<Text>().text = answerWords;
        questions.questionNeeded = answerWords;
        
    }

    void CreateMultiplication()
    {

        enemyPhase = !enemyPhase;

        if (!enemyPhase)
        {
            //multipleContainer.DisableMultiple();
            questions.MakeQuestion(m_button, true, OverRidePhases.EnemyDefend);
        }
        else
        {
            player.SetTime(true, m_button.enemPhaseTime);


            multipleContainer.DisableThisAndCalculator();

            multiplicationContainer.gameObject.SetActive(true);
            multiplicationContainer.GenerateMultiplication(m_button, this);
        }
    }


    void CreateDivision()
    {
        enemyPhase = !enemyPhase;

        if (!enemyPhase)
        {
            questions.MakeQuestion(m_button, true, OverRidePhases.EnemyDefend);
        }
        else
        {
            questions.GetComponent<Text>().text = "";

            player.SetTime(true, m_button.enemPhaseTime);


            multipleContainer.DisableThisAndCalculator();

            bossDivision.gameObject.SetActive(true);
            bossDivision.GenerateDivision(m_button);
        }
    }

    internal void EnableBossSpecific(bool setActive)
    {
        switch (Operator)
        {
            case operators.Addition:
                additionContainer.gameObject.SetActive(setActive);

                break;
            case operators.Subtraction:
                break;
            case operators.Multiplication:
                multiplicationContainer.gameObject.SetActive(setActive);
                break;
            case operators.Division:
                bossDivision.gameObject.SetActive(setActive);
                break;
            case operators.AddSubMultDiv:
                break;
            default:
                break;
        }
    }
}