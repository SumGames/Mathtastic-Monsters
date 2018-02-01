﻿using System.Collections;
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

    public Abacus abacus;

    public Calculator calculator;



    //multBoss;

    int Heads;

    internal int validNumbers;

    int multHealthOne = 4;
    int multHealthTwo = 4;
    int multHealthThree = 4;

    int multAttacks;


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

        if (animator)
            animator.Play("Attack");

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
            if (animator)
                animator.Play("Attack");

            depth = m_button.maxDepth;
            CreateQuestion();
            player.currentHealth -= m_button.MonsterAttack;
            CheckDeath();

            feedback.DamageSet(SetFeedback.PlayerHit);
            return;
        }
        //else player simply got any other answer wrong.

        if (animator)
            animator.Play("Attack");

        player.DamagePlayer(1);
        feedback.DamageSet(SetFeedback.PlayerHit);

        CreateSubtraction(true, false);
    }

    //Player attacks monster.
    internal override void MonsterHurt()
    {
        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();

        if (Operator == operators.Multiplication)
        {
            

            multiplicationHeads(1);
            return;
        }
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

        if (animator)
            animator.Play("Hurt");



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
            if (animator)
                animator.Play("Hurt");

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

        if (Operator == operators.Multiplication)
        {
            bar.setMaxHealth(health, false, true);
            validNumbers = 2;
            multHealthOne = 4;
            multHealthTwo = 4;
            multHealthThree = 4;
            Heads = 3;
            multAttacks = 3;
        }
        else
        {
            bar.setMaxHealth(health, false);
        }

        if (!music)
            music = FindObjectOfType<MusicManager>();

        if (music)
            music.SetCombatMusic(parent.quizRunning.Operator, parent.quizRunning.boss);


        CreateQuestion();

    }

    //Choose the type of question we're going to make from this type.
    public void CreateQuestion()
    {
        abacus.gameObject.SetActive(false);

        bossDivision.gameObject.SetActive(false);


        additionContainer.gameObject.SetActive(false);
        multiplicationContainer.gameObject.SetActive(false);

        switch (Operator)
        {
            case operators.Addition:
                CreateAddition();
                break;
            case operators.Subtraction:
                CreateSubtraction(true, true);
                break;
            case operators.Multiplication:
                multiplicationHeads(0);
                CreateMultiplication();
                break;
            case operators.Division:
                CreateDivision();
                break;

            case operators.AddSub:
            case operators.AddSubMult:
            case operators.AddSubMultDiv:
            case operators.Fortress:
                CreateMultipleChoice();
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

    internal void CreateMultiplication()
    {        
        if (multAttacks <= 0)
        {
            questions.MakeQuestion(m_button, true, OverRidePhases.EnemyDefend);
            multiplicationContainer.gameObject.SetActive(false);

        }
        else
        {
            multAttacks--;
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
                abacus.gameObject.SetActive(setActive);
                break;
            default:
                break;
        }
    }

    internal void CreateMultipleChoice()
    {
        Debug.Log("fort");

        abacus.gameObject.SetActive(true);


        questions.MakeQuestion(m_button, true, OverRidePhases.EnemyDefend);


        multipleContainer.DisableThisAndCalculator();
    }


    internal void multiplicationHeads(int damage)
    {
        if (damage > 0)
        {
            Debug.Log("Impact");

            if (animator)
                animator.Play("Hurt");

            feedback.DamageSet(SetFeedback.EnemyHit);
        }

        Heads = headCheck(damage);

        switch (Heads)
        {
            case 1:
                validNumbers = 2;
                m_button.levelTime = 30;
                m_button.enemPhaseTime = 30;
                break;
            case 2:
                validNumbers = 4;
                m_button.levelTime = 25;
                m_button.enemPhaseTime = 25;
                break;
            case 3:
                validNumbers = 6;
                m_button.levelTime = 20;
                m_button.enemPhaseTime = 20;
                break;
            default:
                health = -2;
                CheckDeath();
                return;
        }

        multAttacks = Heads;

        CreateMultiplication();
    }


    int headCheck(int damage)
    {
        if (multHealthThree > 0)
        {           
            multHealthThree -= damage;

            bar.SetEnemyBars(3, multHealthThree);

            if (multHealthThree > 0)
                return 3;
            else
                return 2;
        }
        if (multHealthTwo > 0)
        {
            multHealthTwo -= damage;

            bar.SetEnemyBars(2, multHealthTwo);

            if (multHealthTwo > 0)
                return 2;
            else
                return 1;
        }
        if (multHealthOne > 0)
        {
            multHealthOne -= damage;

            health = multHealthOne;

            bar.SetEnemyBars(1, multHealthOne);

            if (multHealthOne > 0)
                return 1;
        }
        return 0;
    }


    public void SubmitAbacus()
    {
        if (abacus.CalculateTotal().ToString() == calculator.answerNeeded)
        {
            MonsterHurt();
        }
        else
        {
            EnemyAttack();
        }
    }
}