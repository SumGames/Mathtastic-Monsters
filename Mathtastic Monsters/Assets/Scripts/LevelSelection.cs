using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public questionContainer[] containers;


    public questionContainer currentContainer;

    equipmentList list;


    classType currentSubject;
    public int currentLevel;

    int starsUnlocked;

    public Button starOne, starTwo, starThree;

    public Button NormalMode;

    public Button hardMode;

    public Button[] jumpButtons;

    public Button nextLevel, previousLevel;



    public float swipeTimeNeeded = 0.3f;
    public float minSwipeDistance = 50;

    float swipeStartTime;
    float swipeStartPosition;


    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        //if (touch.phase == TouchPhase.Began) { }
        if (Input.GetMouseButtonDown(0))
        {
            swipeStartTime = Time.time;
            swipeStartPosition = Input.mousePosition.x;

        }

        //else if (touch.phase == TouchPhase.Ended) { }
        else if (Input.GetMouseButtonUp(0))
        {
            float endTime = Time.time;
            float endPos = Input.mousePosition.x;

            float swipeTime = endTime - swipeStartTime;

            float swipeDistance = endPos - swipeStartPosition;

            bool swipeDirectionPositive;

            if (swipeDistance > 0)
            {
                swipeDirectionPositive = true;

            }
            else
            {
                swipeDirectionPositive = false;
                swipeDistance *= -1;
            }


            if (swipeTime > swipeTimeNeeded && swipeDistance > minSwipeDistance)
            {
                Debug.Log(swipeDistance);

                IncrementIndex(swipeDirectionPositive);
            }
        }
    


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                swipeStartTime = Time.time;
                swipeStartPosition = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                float endTime = Time.time;
                float endPos = touch.position.x;

                float swipeTime = endTime - swipeStartTime;

                float swipeDistance = endPos - swipeStartPosition;

                bool swipeDirectionPositive;

                if (swipeDistance > 0)
                {
                    swipeDirectionPositive = true;
                }
                else
                {
                    swipeDirectionPositive = false;
                    swipeDistance *= -1;
                }


                if (swipeTime > swipeTimeNeeded && swipeDistance > minSwipeDistance)
                {
                    IncrementIndex(swipeDirectionPositive);
                }
            }
        }
    }


    public void setContainer(questionContainer op)
    {
        currentContainer = op;
        currentSubject = op.type;
        currentLevel = 0;
        gameObject.SetActive(true);
        CheckStars(op.buttons[0]);

        SetButtons();

        op.gameObject.SetActive(false);
    }

    public void UseNormalButton()
    {
        currentContainer.buttons[currentLevel].buttonUsed();
        gameObject.SetActive(false);
    }
    public void useHardButton()
    {
        QuizButton button = currentContainer.buttons[currentLevel].hardMode;
        if (button == null)
            return;
        button.Hard = true;
        button.buttonUsed();
        gameObject.SetActive(false);
    }

    void SetButtons()
    {
        NormalMode.GetComponentInChildren<Text>().text = currentContainer.buttons[currentLevel].name;

        if (currentContainer.buttons[currentLevel].hardMode != null)
            hardMode.GetComponentInChildren<Text>().text = currentContainer.buttons[currentLevel].hardMode.name;



        for (int i = 0; i < 10; i++)
        {
            if (i <= currentContainer.completedQuestions)
                jumpButtons[i].interactable = true;
            else
                jumpButtons[i].interactable = false;
        }
    }

    public void SetStars(bool fullHealth, QuizButton a_button)
    {
        if (!list)
            list = FindObjectOfType<equipmentList>();

        currentLevel = a_button.quizIndex;

        if ((int)a_button.Operator > (int)classType.Calculi)
            currentSubject = classType.Calculi;
        else
            currentSubject = (classType)a_button.Operator;

        starsUnlocked = list.equip.StarsAcquired[(((int)currentSubject * 10) + currentLevel)];

        if (starsUnlocked == 0)
            starsUnlocked = 1;

        if (fullHealth && starsUnlocked < 2)
        {
            starsUnlocked = 2;

        }
        if (a_button.Hard)
        {
            starsUnlocked = 3;
        }

        list.equip.StarsAcquired[(((int)currentSubject * 10) + currentLevel)] = starsUnlocked;

        CheckStars(a_button);
    }


    public void CheckStars(QuizButton a_button)
    {
        if (!list)
            list = FindObjectOfType<equipmentList>();

        if ((int)a_button.Operator > (int)classType.Calculi)
            currentSubject = classType.Calculi;
        else
            currentSubject = (classType)a_button.Operator;


        starsUnlocked = list.equip.StarsAcquired[(((int)currentSubject * 10) + currentLevel)];


        starOne.interactable = (starsUnlocked >= 1);
        starTwo.interactable = (starsUnlocked >= 2);
        hardMode.interactable = ((starsUnlocked >= 2) && currentContainer.buttons[currentLevel].hardMode != null);

        starThree.interactable = (starsUnlocked >= 3);
        
    }

    public void ChangeIndex(int a_index)
    {
        currentLevel = a_index;
        SetButtons();
        CheckStars(currentContainer.buttons[a_index]);
    }

    void IncrementIndex(bool Positive)
    {
        if (Positive)
        {
            if (currentLevel == currentContainer.completedQuestions || currentLevel == 10)
            {
                Debug.Log("Can't go on!");
            }
            else
            {
                ChangeIndex((currentLevel + 1));
            }
        }
        else
        {
            if (currentLevel == 0)
            {
                Debug.Log("At the start");
            }
            else
            {
                ChangeIndex((currentLevel - 1));
            }


        }


    }
}