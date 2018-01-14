using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum phases
{
    None,
    previous,
    next,
    enemy
}


public class StoryManager : MonoBehaviour
{
    public Calculator calculator;

    QuizButton nextLevel;


    string previousLevelWords;
    float previousTime;

    string nextLevelWords;
    float nextime;

    string enemyWords;
    float enemyTime;


    float timer;
    public Text textDisplay;
    float movementIncrement;


    public float objectSpot;
    public GameObject movingObject;


    internal phases phase;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (phase != phases.None)
        {
            timer += Time.deltaTime;

            objectSpot += (movementIncrement * Time.deltaTime);
            movingObject.transform.localPosition = new Vector3(objectSpot, 0, 0);
            CheckTransitionEnd();
        }
    }

    void CheckTransitionEnd()
    {
        switch (phase)
        {
            case phases.previous:
                if (timer > previousTime)
                {
                    phase = phases.next;
                    timer = 0;
                    SetMovementStartAndSpeed(phase);
                }
                break;
            case phases.next:
                if (timer > previousTime)
                {
                    phase = phases.enemy;
                    timer = 0;
                    textDisplay.text = enemyWords;
                    SetMovementStartAndSpeed(phase);
                }
                break;
            case phases.enemy:
                if (timer > enemyTime)
                {
                    EndMovement();

                }
                break;
            default:
                break;
        }
    }

    void EndMovement()
    {
        phase = phases.None;
        SetMovementStartAndSpeed(phase);
        textDisplay.text = "";

        calculator.AbleCalculator(true);

    }

    internal void StartTransition(QuizButton a_button, phases a_startingPhase)
    {
        if(a_startingPhase==phases.None)
        {
            EndMovement();
            return;
        }


        calculator.AbleCalculator(false);

        nextLevel = a_button;

        previousLevelWords = a_button.previousLevelWords;
        previousTime = a_button.previousTime;

        nextLevelWords = a_button.nextLevelProgress;
        nextime = a_button.nextime;

        enemyWords = a_button.enemyWords;
        enemyTime = a_button.enemyTime;

        phase = a_startingPhase;
        SetMovementStartAndSpeed(a_startingPhase);

    }
    void SetMovementStartAndSpeed(phases position)
    {
        if (position == phases.enemy || position == phases.None)
        {
            objectSpot = 0;
            movingObject.transform.localPosition = new Vector3(0, 0, 0);
        }
        if (position == phases.previous)
        {
            objectSpot = 0;
            movingObject.transform.localPosition = new Vector3(objectSpot, 0, 0);
            movementIncrement = -(1200 / previousTime);
            textDisplay.text = previousLevelWords;
        }
        else if (position == phases.next)
        {
            objectSpot = 1200;
            movingObject.transform.localPosition = new Vector3(objectSpot, 0, 0);
            movementIncrement = -(1200 / nextime);
            textDisplay.text = nextLevelWords;
        }
    }
}