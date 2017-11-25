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

    // Use this for initialization
    void Start()
    {
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

        feedback.DamageSet(SetImage.hurt);

        player.DamagePlayer(1);

        CreateQuestion();

        CheckDeath();
    }

    internal override void MonsterHurt()
    {
        if (Operator == operators.Addition)
        {
            m_button.enemyChoices++;
            m_button.enemyAnswerRange += (int)(m_button.minNumber / 2);
        }

        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();

        feedback.DamageSet(SetImage.hit);

        --health;

        CreateQuestion();

        CheckDeath();
    }


    public override void loadMonster()
    {

        FindObjectOfType<multipleContainer>().DisableMultiple(true);

        m_button = (BossButton)parent.quizRunning;

        m_button.enemyChoices = 2;

        Operator = parent.quizRunning.Operator;

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

        healthBar.maxValue = health;

        healthBar.value = health;

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
                break;
            case operators.Multiplication:
                break;
            case operators.Division:
                break;
            case operators.Fortress:
                break;
            case operators.AdditionSubtraction:
                break;
            default:
                break;
        }


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