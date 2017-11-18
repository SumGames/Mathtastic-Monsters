using UnityEngine;
using System;

[Serializable]
public class CurrentSaveData
{

    [SerializeField]
    internal float Modifier;
    [SerializeField]
    public float Score;
    [SerializeField]
    public int levels;

    [SerializeField]
    public int fightsBetweenBreaks;

    [SerializeField]
    public bool[] removedLimbs;




    [SerializeField]
    public int variableCount = 2; //Number of variables being summed.

    [SerializeField]
    public float levelTime = 0; //Amount of additional time added at level start.

    [SerializeField]
    public float minNumber; //Smallest possible number a variable can reach.

    [SerializeField]
    public float maxNumber; //Largest possible number a variable can reach.

    [SerializeField]
    public int Operator; //+, - or x. Division will be included later.

    [SerializeField]
    public int minAnswer = 1; //Lowest acceptable answer.

    [SerializeField]
    public int maxAnswer = 100000; //Highest possible answer.

    [SerializeField]
    public int MonsterHealth = 6; //The monster's health.

    [SerializeField]
    public int MonsterAttack = 1; //Damage the monster will inflict on hit.


    [SerializeField]
    public bool preventRounding;

    [SerializeField]
    public int secondNumberMin;

    [SerializeField]
    public int secondNumberMax;

    [SerializeField]
    public int enemPhaseTime = 8;

    [SerializeField]
    public int enemyChoices = 3;


    [SerializeField]
    public int enemyAnswerRange = 2;



    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void saveCurrentRun(endlessMonsterManager manager, endlessButton button)
    {
        Modifier = manager.Modifier;
        Score = manager.Score;
        levels = manager.levels;

        fightsBetweenBreaks = manager.fightsBetweenBreaks;
        //removedLimbs[] = 6;


        variableCount = button.variableCount;
        MonsterAttack = button.MonsterAttack;
        MonsterHealth = button.MonsterHealth;


        levelTime = button.levelTime;
        minNumber = button.minNumber;
        maxNumber = button.maxNumber;

        minAnswer = button.minAnswer;
        maxAnswer = button.maxAnswer;

        preventRounding = button.preventRounding;

        Operator = (int)button.Operator;

        secondNumberMin = button.secondNumberMin;
        secondNumberMax = button.secondNumberMax;

        enemPhaseTime = button.enemPhaseTime;
        enemyChoices = button.enemyChoices;

        enemyAnswerRange = button.enemyAnswerRange;
    }


    public void loadCurrentRun(endlessMonsterManager manager, endlessButton button)
    {
        manager.Modifier = Modifier;
        manager.Score = Score;
        manager.levels = levels;

        manager.fightsBetweenBreaks = fightsBetweenBreaks;
        //removedLimbs[] = 6;


        button.variableCount = variableCount;
        button.MonsterAttack = MonsterAttack;
        button.MonsterHealth = MonsterHealth;


        button.levelTime = levelTime;
        button.minNumber = minNumber;
        button.maxNumber = maxNumber;

        button.minAnswer = minAnswer;
        button.maxAnswer = maxAnswer;

        button.preventRounding = preventRounding;

        button.Operator = (operators)Operator;

        button.secondNumberMin = secondNumberMin;
        button.secondNumberMax = secondNumberMax;

        button.enemPhaseTime = enemPhaseTime;
        button.enemyChoices = enemyChoices;

        button.enemyAnswerRange = enemyAnswerRange;
    }


}
