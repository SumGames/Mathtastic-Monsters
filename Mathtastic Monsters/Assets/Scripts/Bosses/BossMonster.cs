using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMonster : Monster
{
    operators Operator;


    BossButton m_button;

    internal int answerNeeded;


    public Text questionText;


    public AdditionContainer additionContainer;

    combatFeedback feedback;

    public float depth = -5;

    public float playerTime;



    public GameObject subtractionBottom;

    public Vector3 highSpot;

    // Use this for initialization
    void Start()
    {
        bar = FindObjectOfType<Healthbars>();

        manager = FindObjectOfType<StateManager>();

    }

    //Update healthbar as it changes.
    void Update()
    {

    }

    internal override void EnemyAttack()
    {

        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();

        if (Operator == operators.Subtraction)
        {
            EnemyAttackSubtraction();
            return;
        }

        feedback.DamageSet(SetFeedback.PlayerHit);


        player.DamagePlayer(1);

        CheckDeath();


        CreateQuestion();


    }

    void EnemyAttackSubtraction()
    {
        if (depth == 0)
        {
            feedback.DamageSet(SetFeedback.PlayerMissed);
            depth = -5;
            CreateQuestion();
            return;
        }

        if (player.returnTimer() <= 0)
        {
            depth = -5;
            CreateQuestion();
            player.DamagePlayer(1);
            feedback.DamageSet(SetFeedback.PlayerHit);
            return;
        }
        player.DamagePlayer(1);
        feedback.DamageSet(SetFeedback.PlayerHit);

        CreateSubtraction(true, false);
    }


    internal override void MonsterHurt()
    {
        
        if (Operator == operators.Subtraction)
        {
            SubtractionAttacked();
            return;
        }

        if (Operator == operators.Addition)
        {
            m_button.enemyChoices++;
            m_button.enemyAnswerRange += 1;
        }

        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();

        feedback.DamageSet(SetFeedback.EnemyHit);

        --health;

        bar.changeHealth(false, health);

        CreateQuestion();

        CheckDeath();
    }

    void SubtractionAttacked()
    {
        if (depth >= 0)
        {
            health--;

            bar.changeHealth(false, health);

            depth = -5;

            CreateQuestion();
            CheckDeath();
            return;
        }
        else if (depth == -1)
        {
            playerTime = player.returnTimer();

            CreateSubtraction(false, false);

            depth = 0;
            return;
        }
        else
        {
            playerTime = player.returnTimer();

            depth++;

            CreateSubtraction(true, false);
            return;
        }

    }

    public override void loadMonster()
    {
        highSpot = monsterSpot.transform.position;

        questions = FindObjectOfType<questionManager>();

        if (!multipleContainer)
            multipleContainer = FindObjectOfType<multipleContainer>();

        multipleContainer.setAttacks(true, false);


        multipleContainer.DisableMultiple(true);

        m_button = (BossButton)parent.quizRunning;

        Operator = parent.quizRunning.Operator;


        if (Operator == operators.Addition)
        {
            m_button.enemyChoices = 2;
            m_button.enemyAnswerRange = 2;
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


    public void CreateQuestion()
    {
        additionContainer.gameObject.SetActive(false);

        switch (Operator)
        {
            case operators.Addition:
                CreateAddition();
                break;
            case operators.Subtraction:
                CreateSubtraction(true,true);

                break;
            case operators.Multiplication:
                break;
            case operators.Division:
                break;

            default:
                break;
        }


    }

    void CreateSubtraction(bool EnemyAttackingPhase, bool resetTime)
    {
        int over;

        if (EnemyAttackingPhase)
            over = 1;
        else
            over = 2;


        EnemyAttackingPhase = questions.MakeQuestion(m_button, resetTime, over);

        float positionY = Mathf.Lerp(highSpot.y, subtractionBottom.transform.position.y, ((depth * -1) / 5));

        monsterSpot.transform.position = new Vector3(monsterSpot.transform.position.x, positionY, monsterSpot.transform.position.z);

    }

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
        questionText.text = answerWords;
    }
}