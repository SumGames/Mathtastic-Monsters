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
    QuizButton button;
    public BossMonster boss;

    public GameObject monsterSpot;

    public Calculator calculator;


    string previousLevelWords;
    float previousTime;

    string nextLevelWords;
    public float nextime;

    string enemyWords;
    float enemyTime;


    float timer;
    public Text textDisplay;
    float movementIncrement;


    public float objectSpot;
    public GameObject movingObject;


    internal phases phase;

    public GameObject textBackground;

    public GameObject calculatorParent;

    public TransitionManager transitionManager;

    public Button[] calcButtons;

    backgroundManager background;

    public Healthbars healthbars;


    // Use this for initialization
    void Start()
    {

        phase = phases.None;
        textDisplay.gameObject.SetActive(false);
        textBackground.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!background)
            background = FindObjectOfType<backgroundManager>();

        if (phase != phases.None)
        {
            if (button.boss)
            {
                boss.EnableBossSpecific(false);
            }

            if (monsterSpot.transform.localPosition.x != 448)
                monsterSpot.transform.localPosition = new Vector3(448, monsterSpot.transform.localPosition.y, monsterSpot.transform.localPosition.z);

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
                if (timer > nextime)
                {
                    phase = phases.enemy;
                    timer = 0;
                    textDisplay.text = enemyWords;
                    SetMovementStartAndSpeed(phase);
                    healthbars.wait = false;
                }
                break;
            case phases.enemy:
                if (timer > enemyTime)
                {
                    healthbars.wait = false;
                    EndMovement();
                }
                break;
            default:
                break;
        }
    }

    void EndMovement()
    {
        if (!background)
            background = FindObjectOfType<backgroundManager>();
        if (background.currentScroll)
            background.currentScroll.ScrollAll(false);

        phase = phases.None;
        SetMovementStartAndSpeed(phase);

        textDisplay.gameObject.SetActive(false);
        textBackground.SetActive(false);

        if (button.boss)
        {
            boss.EnableBossSpecific(true);
        }


        calculator.AbleCalculator(true);

    }

    internal void StartTransition(QuizButton a_button, phases a_startingPhase)
    {
        button = a_button;

        if (a_startingPhase == phases.None)
        {
            EndMovement();
            return;
        }

        textBackground.SetActive(true);
        textDisplay.gameObject.SetActive(true);

        timer = 0;

        calculator.AbleCalculator(false);

        previousLevelWords = a_button.previousLevelString;
        previousTime = a_button.previousTime;

        nextLevelWords = a_button.nextLevelString;
        nextime = a_button.nextime;

        enemyWords = a_button.enemyString;
        enemyTime = a_button.enemyTime;
        SetMovementStartAndSpeed(a_startingPhase);

        phase = a_startingPhase;


        if(FindObjectOfType<equipmentList>().skipping)
        {
            enemyTime = 0;
            previousTime = 0;
            nextime = 0;
            healthbars.skipRefill();
        }

    }
    void SetMovementStartAndSpeed(phases position)
    {
        if (!background)
            background = FindObjectOfType<backgroundManager>();

        if (position == phases.enemy || position == phases.None)
        {
            if (background.currentScroll)
                background.currentScroll.ScrollAll(false);

            objectSpot = 0;
            movingObject.transform.localPosition = new Vector3(0, 0, 0);
            movementIncrement = 0;
            monsterSpot.gameObject.SetActive(true);

            calculatorParent.SetActive(true);
            //transitionManager.TransitionContainers(TransitioningObjects.SwapToCalculator);
        }
        if (position == phases.None)
        {
            foreach (Button item in calcButtons)
            {
                item.interactable = true;
            }
        }
        else
        {
            foreach (Button item in calcButtons)
            {
                item.interactable = false;
            }
        }

        if (position == phases.previous)
        {
            if (background.currentScroll)
                background.currentScroll.ScrollAll(true);

            if (previousTime == 0)
            {
                movementIncrement = 0;
                return;
            }
            monsterSpot.gameObject.SetActive(false);

            objectSpot = 0;
            movingObject.transform.localPosition = new Vector3(objectSpot, 0, 0);
            movementIncrement = -(1200 / previousTime);
            textDisplay.text = previousLevelWords;

            calculatorParent.SetActive(false);
        }
        else if (position == phases.next)
        {
            if (background.currentScroll)
                background.currentScroll.ScrollAll(true);

            if (nextime == 0)
            {
                movementIncrement = 0;
                return;
            }

            monsterSpot.gameObject.SetActive(true);

            objectSpot = 1200;
            movingObject.transform.localPosition = new Vector3(1200, 0, 0);
            movementIncrement = -(1200 / nextime);
            textDisplay.text = nextLevelWords;

            calculatorParent.SetActive(false);
        }
    }
}