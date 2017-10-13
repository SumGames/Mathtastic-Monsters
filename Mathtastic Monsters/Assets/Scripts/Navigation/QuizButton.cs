using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizButton : MonoBehaviour
{
    public int quizIndex;
    internal questionContainer parent;

    public int variableCount = 2; //Number of variables being summed.

    public float levelTime = 0; //Amount of additional time added at level start.

    public float minNumber; //Smallest possible number a variable can reach.
    public float maxNumber; //Largest possible number a variable can reach.

    public char Operator; //+, - or x. Division will be included later.

    public int minAnswer = 1; //Lowest acceptable answer.
    public int maxAnswer = 100000; //Highest possible answer.

    public int difficulty; //The amount of base experience given for completing the level.

    public int MonsterHealth = 6; //The monster's health.
    public int MonsterAttack = 1; //Damage the monster will inflict on hit.


    MonsterManager p_manager; //A link to the quizManager so it can tell it to start.


    public bool rounding;

    public int secondNumberMin;
    public int secondNumberMax;

    public GameObject monsterArt;


    public int enemPhaseTime = 8;
    public int enemyChoices = 3;
    public int enemyAnswerRange = 2;

    // Use this for initialization
    void Start()
    {
        p_manager = GameObject.Find("MonsterManager").GetComponent<MonsterManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((!parent) || quizIndex <= parent.getCompleted())
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }


    //Call the quizManager to start a quiz using this button as the basis.
    public void buttonUsed()
    {
        p_manager.startLevel(this);
    }
}
