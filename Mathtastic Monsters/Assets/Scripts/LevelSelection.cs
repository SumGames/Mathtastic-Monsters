using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public questionContainer[] containers; //The containers that store all buttons for addition, etc.
    public questionContainer currentContainer; //The one the player selected.
    equipmentList list;
    classType currentSubject;
    public int currentLevel;
    int starsUnlocked; //How many stars we have for this level.
    public Button starOne, starTwo, starThree; //Stars light up if we have their star.
    public Button Medallion;
    public Button NormalMode; //Clicked on to start normal mode.
    public Button hardMode; //If we have two stars, click on to start the normal mode's "hard mode" button.
    public Button[] jumpButtons; //An array of buttons we can jump between.
    public GameObject goldstarParticle;
    public GameObject silverstarParticle;
    public GameObject bronzestarParticle;
    float swipeTimeNeeded = 0.3f;
    float minSwipeDistance = 50;
    float swipeStartTime; //Used to calculate how long we held on.
    float swipeStartPosition;//Where we started moving from. Only need X coord here.
                             // Use this for initialization

    public SelectionImages selectionImages;

    void Start()
    {

    }
    void Update()
    {
        //Starting a mouse swipe.
        if (Input.GetMouseButtonDown(0))
        {
            swipeStartTime = Time.time;
            swipeStartPosition = Input.mousePosition.x;
        }
        //Ending mouse swipe
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
        //Handle touch swipe.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) //Start touch swipe.
            {
                swipeStartTime = Time.time;
                swipeStartPosition = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Ended) //touch ended
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
    //Open up the script by setting up variables relative to selection.
    public void setContainer(questionContainer op)
    {
        currentContainer = op;
        currentSubject = op.type;
        currentLevel = 0;
        gameObject.SetActive(true);
        CheckStars(op.buttons[0]);
        SetButtons();
        op.gameObject.SetActive(false);

        selectionImages.SetSprite(currentSubject);

    }
    //Start playing with the button's level..
    public void UseNormalButton()
    {
        currentContainer.buttons[currentLevel].buttonUsed(phases.next);
        gameObject.SetActive(false);
    }
    //Star our normal button's hard mode.
    public void useHardButton()
    {
        QuizButton button = currentContainer.buttons[currentLevel].hardMode;
        button.quizIndex = currentContainer.buttons[currentLevel].quizIndex;
        if (button == null)
            return;
        button.Hard = true;
        button.buttonUsed(phases.None);
        gameObject.SetActive(false);
    }
    //Disable buttons, add names as required.
    void SetButtons()
    {
        NormalMode.GetComponentInChildren<Text>().text = currentContainer.buttons[currentLevel].name;
        for (int i = 0; i < 10; i++)
        {
            if (i <= currentContainer.completedQuestions)
                jumpButtons[i].interactable = true;
            else
                jumpButtons[i].interactable = false;
        }
    }
    //Called from player if we won.
    //If we won, 1 star.
    //If we were at 100% health, 2 stars.
    //If it was hardmode, 3.
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
    //Use our current star levels to light up stars, buttons.
    public void CheckStars(QuizButton a_button)
    {
        if (!list)
            list = FindObjectOfType<equipmentList>();
        if ((int)a_button.Operator > (int)classType.Calculi)
            currentSubject = classType.Calculi;
        else
            currentSubject = (classType)a_button.Operator;
        starsUnlocked = list.equip.StarsAcquired[(((int)currentSubject * 10) + currentLevel)];
        if (currentLevel == 4 || currentLevel == 9)
        {
            Medallion.gameObject.SetActive(true);
            hardMode.gameObject.SetActive(false);
            starOne.gameObject.SetActive(false);
            starTwo.gameObject.SetActive(false);
            starThree.gameObject.SetActive(false);
            bronzestarParticle.gameObject.SetActive(false);
            silverstarParticle.gameObject.SetActive(false);
            goldstarParticle.gameObject.SetActive(false);
            Medallion.interactable = (starsUnlocked >= 1);
            return;
        }
        Medallion.gameObject.SetActive(false);
        hardMode.gameObject.SetActive(true);
        starOne.gameObject.SetActive(true);
        starTwo.gameObject.SetActive(true);
        starThree.gameObject.SetActive(true);
        starOne.interactable = (starsUnlocked >= 1);
        starTwo.interactable = (starsUnlocked >= 2);
        hardMode.interactable = ((starsUnlocked >= 2) && currentContainer.buttons[currentLevel].hardMode != null);

        if (currentContainer.buttons[currentLevel].hardMode != null && hardMode.interactable)
        {
            hardMode.GetComponentInChildren<Text>().text = currentContainer.buttons[currentLevel].hardMode.name;
        }
        else
        {
            hardMode.GetComponentInChildren<Text>().text = "Complete Normal at full health";
        }

        starThree.interactable = (starsUnlocked >= 3);
        bronzestarParticle.SetActive(starOne.interactable);
        silverstarParticle.SetActive(starTwo.interactable);
        goldstarParticle.SetActive(starThree.interactable);
    }
    //Clicked on a button, so we go up by one.
    public void ChangeIndex(int a_index)
    {
        currentLevel = a_index;
        SetButtons();
        CheckStars(currentContainer.buttons[a_index]);
    }
    //Swiped Right for true, left for false.
    //Add/Remove 1 from index as needed.
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