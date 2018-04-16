using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    //The lowest point our monster can reach.
    public GameObject subtractionBottom;


    //Used in addition with container.
    internal int bossSpacing;

    public BossDivision bossDivision;

    public MultiplicationContainer multiplicationContainer;

    public Calculator calculator;


    //multBoss;
    int Heads;
    internal int validNumbers;
    int multHealthOne = 4;
    int multHealthTwo = 4;
    int multHealthThree = 4;
    int multAttacks;


    public SubtractionContainer subtractionContainer;

    public Fortress Fortress;
    public GameObject DefendSide;
    public Abacus abacus;


    public GameObject ArrowContainer;
    public GameObject ArrowStart;
    int AbacusAttempts;
    public GameObject[] Arrows;
    bool ArrowsFired;

    hydraTestAnimation hydraAnimation;

    // Use this for initialization
    void Start()
    {
        bar = FindObjectOfType<Healthbars>();

        manager = FindObjectOfType<ParentsStateManager>();
    }

    //Update healthbar as it changes.
    void Update()
    {
        if (ArrowsFired)
        {
            abacus.gameObject.SetActive(false);

            if (ArrowContainer.transform.localPosition.x < 0)
            {
                ArrowContainer.transform.localPosition += new Vector3(350 * Time.deltaTime, 0, 0);
            }
            else
            {
                ArrowsFired = false;

                CalculusDamage(AbacusAttempts);
                ArrowContainer.gameObject.SetActive(false);
            }
        }

    }

    //Player was wrong or so slow, enemy gets an attack!
    internal override void EnemyAttack()
    {
        if (ArrowsFired)
            return;

        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();


        if (animator)
            animator.Play("Attack");

        feedback.DamageSet(SetFeedback.PlayerHit);


        player.DamagePlayer(1);

        sprite.transform.localPosition = startingPosition;
        sprite.transform.localRotation = startingRotation;


        CheckDeath(true);

        CreateQuestion();
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

        if (animator)
            animator.Play("Hurt");

        sprite.transform.localPosition = startingPosition;
        sprite.transform.localRotation = startingRotation;


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

        CheckDeath(true);
    }

    //Monster is initiated.
    public override void loadMonster()
    {
        ArrowsFired = false;
        ArrowContainer.SetActive(false);

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

        if (Operator > operators.Division && m_button.quizIndex < 9)
        {
            Fortress.SetPatience(5);
        }
        else
        {
            Fortress.patience.gameObject.SetActive(false);
        }

        if (sprite != null)
        {
            Destroy(sprite.gameObject);
        }
        if (parent.quizRunning.monsterArt != null)
        {
            sprite = Instantiate(parent.quizRunning.monsterArt, monsterSpot.transform, false);
            sprite.transform.localScale = parent.quizRunning.monsterArt.transform.localScale;
            animator = sprite.GetComponentInChildren<Animator>();
            startingPosition = sprite.transform.localPosition;
            startingRotation = sprite.transform.localRotation;
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

        if (Operator == operators.Subtraction)
        {
            subtractionContainer.SetUpSubtraction(health);
        }


        CreateQuestion();

    }

    //Choose the type of question we're going to make from this type.
    public void CreateQuestion()
    {
        abacus.gameObject.SetActive(false);

        bossDivision.gameObject.SetActive(false);


        additionContainer.gameObject.SetActive(false);
        multiplicationContainer.gameObject.SetActive(false);

        subtractionContainer.gameObject.SetActive(false);

        Fortress.gameObject.SetActive(false);

        switch (Operator)
        {
            case operators.Addition:
                CreateAddition();
                break;
            case operators.Subtraction:
                CreateSubtraction(true);
                break;
            case operators.Multiplication:
                multiplicationHeads(0);
                CreateMultiplication();
                break;
            case operators.Division:
                CreateDivision();
                break;

            default:
                CreateCalculi();
                break;
        }


    }

    //Enemy moves from bottom of ocean.
    //Most new questions won't actually reset the timer.
    internal void CreateSubtraction(bool EnemyAttackingPhase)
    {
        if (EnemyAttackingPhase)
        {
            multipleContainer.DisableThisAndCalculator();
            subtractionContainer.gameObject.SetActive(true);
            subtractionContainer.GenerateSubtraction(m_button, health);
        }
        player.SetTime(true, m_button.enemPhaseTime, true);

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
            answerWords += "_";
        }
        else
            answerWords += numbers[0].ToString("F0");

        for (int i = 1; i < m_button.variableCount; i++)
        {

            if (first == i)
                answerWords += " + _ ";
            else
                answerWords += " + " + numbers[i].ToString("F0");
        }

        answerWords += " = " + result.ToString();


        questions.questionNeeded = answerWords;

        multipleContainer.DisableThisAndCalculator();
    }

    internal void CreateMultiplication()
    {
        if (multAttacks <= 0)
        {
            questions.MakeQuestion(m_button, true);
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
            questions.MakeQuestion(m_button, true);
        }
        else
        {       
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
            default:
                Fortress.gameObject.SetActive(setActive);
                break;
        }
    }

    internal void CreateCalculi()
    {
        Fortress.gameObject.SetActive(true);

        ArrowContainer.gameObject.SetActive(false);

        if (!enemyPhase)
        {
            DefendSide.SetActive(false);

            enemyPhase = true;

            questions.MakeQuestion(m_button, true);

            if (m_button.quizIndex < 9)
            {
                abacus.gameObject.SetActive(false);
                return;
            }
            else if (m_button.quizIndex < 13)
            {

                ArrowContainer.gameObject.SetActive(true);
                setArrows(3);

                abacus.gameObject.SetActive(true);
                abacus.ResetAbacus(false, questions.answer);

                multipleContainer.DisableThisAndCalculator();
                return;
            }
            else
            {
                setArrows(3);
                ArrowContainer.gameObject.SetActive(true);

                abacus.gameObject.SetActive(true);
                abacus.ResetAbacus(true, questions.answer);

                multipleContainer.DisableThisAndCalculator();
                return;
            }
        }
        else
        {
            multipleContainer.DisableThisAndCalculator();

            questions.questionNeeded = "";


            DefendSide.SetActive(true);
            abacus.gameObject.SetActive(false);



            enemyPhase = false;
            Fortress.CalculateBODMAS(m_button, 0);

            player.SetTime(true, m_button.levelTime);
        }
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
                CheckDeath(true);
                return;
        }

        multAttacks = Heads;

        CreateMultiplication();
    }


    int headCheck(int damage)
    {
        if(!hydraAnimation)
        {
            hydraAnimation = FindObjectOfType<hydraTestAnimation>();
        }


        if (multHealthThree > 0)
        {
            multHealthThree -= damage;

            bar.SetEnemyBars(3, multHealthThree);


            if (multHealthThree > 0)
                return 3;
            else
            {
                if (hydraAnimation)
                    hydraAnimation.KillOne();

                return 2;

            }
        }
        if (multHealthTwo > 0)
        {
            if (hydraAnimation)
                hydraAnimation.KillOne();

            multHealthTwo -= damage;

            bar.SetEnemyBars(2, multHealthTwo);

            if (multHealthTwo > 0)
                return 2;
            else
            {
                if (hydraAnimation)
                    hydraAnimation.killTwo();

                return 1;
            }
        }
        if (multHealthOne > 0)
        {
            if (hydraAnimation)
                hydraAnimation.killTwo();

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
            ArrowsFired = true;
        }
        else
        {
            if (!setArrows((AbacusAttempts - 1)))
            {
                EnemyAttack();
            }
        }
    }

    bool setArrows(int arrows)
    {
        AbacusAttempts = arrows;

        if (arrows <= 0)
            return false;

        ArrowContainer.gameObject.SetActive(true);
        ArrowContainer.transform.localPosition = ArrowStart.transform.localPosition;

        for (int i = 0; i < Arrows.Length; i++)
        {
            if (i < arrows)
            {
                Arrows[i].gameObject.SetActive(true);
            }
            else
                Arrows[i].gameObject.SetActive(false);
        }

        return true;
    }

    internal void CalculusDamage(int inflicted)
    {
        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();


        if (animator)
            animator.Play("Hurt");

        sprite.transform.localPosition = startingPosition;
        sprite.transform.localRotation = startingRotation;


        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();

        feedback.DamageSet(SetFeedback.EnemyHit);

        health -= inflicted;

        bar.changeHealth(false, health);

        CreateQuestion();

        CheckDeath(true);

    }
}