using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public questionContainer[] containers; //The containers that store all buttons for addition, etc.
    public questionContainer currentContainer; //The one the player selected.
    equipmentList list;
    internal classType currentSubject;
    public int currentLevel;
    int starsUnlocked; //How many stars we have for this level.
    public Button starOne, starTwo, starThree; //Stars light up if we have their star.

    public GameObject goldstarParticle;
    public GameObject silverstarParticle;
    public GameObject bronzestarParticle;
    float swipeTimeNeeded = 0.3f;
    float minSwipeDistance = 50;
    float swipeStartTime; //Used to calculate how long we held on.
    float swipeStartPosition;//Where we started moving from. Only need X coord here.
                             // Use this for initialization

    public TalismanManager talismanManager;

    public Talisman medal;

    public ButtonSelection[] subjectGroups;
    ButtonSelection selectedSelection;

    bool HardMode;
    public Button StartButton;

    public Button NormalMode;
    public Button BossButton;
    public Button HardButton;

    public Sprite[] NormalSprite;
    public Sprite[] HardSprite;

    public Sprite Generic;



    public GameObject SealContainer;
    public Text SealText;


    void Start()
    {
        SetHardMode(false);
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
        op.gameObject.SetActive(false);

        SetButtons();

        ChangeIndex(currentLevel);

    }

    //Start playing with the button's level..
    public void StartGame()
    {
        if (!HardMode)
        {
            currentContainer.buttons[currentLevel].buttonUsed(phases.next);
            gameObject.SetActive(false);
        }
        else
        {
            QuizButton button = currentContainer.buttons[currentLevel].hardMode;
            button.quizIndex = currentContainer.buttons[currentLevel].quizIndex;
            if (button == null)
                return;
            button.Hard = true;
            button.buttonUsed(phases.None);
            gameObject.SetActive(false);
        }
    }

    public void SetHardMode(bool a_hard)
    {
        bool hard = a_hard;

        if (hard)
        {
            starsUnlocked = list.equip.StarsAcquired[(((int)currentSubject * 10) + currentLevel)];

            if (!((starsUnlocked >= 2) || currentContainer.buttons[currentLevel].hardMode == null))
            {
                hard = false;
            }
        }

        if (!hard)
        {
            HardMode = false;
            NormalMode.GetComponent<Image>().sprite = NormalSprite[1];
            HardButton.GetComponent<Image>().sprite = HardSprite[0];


            if (selectedSelection.buttonLabel.Length > currentLevel)
            {
                StartButton.GetComponent<Image>().sprite = selectedSelection.buttonLabel[currentLevel];
            }
            else
            {
                StartButton.GetComponent<Image>().sprite = Generic;
            }
        }
        else
        {
            NormalMode.GetComponent<Image>().sprite = NormalSprite[0];
            HardButton.GetComponent<Image>().sprite = HardSprite[1];
            HardMode = true;

            if (selectedSelection.buttonLabelHard.Length > currentLevel)
            {
                StartButton.GetComponent<Image>().sprite = selectedSelection.buttonLabelHard[currentLevel];
            }
            else
            {
                StartButton.GetComponent<Image>().sprite = Generic;
            }
        }

    }

    //Disable buttons, add names as required.
    void SetButtons()
    {
        SetHardMode(HardMode);

        SetupGroups((int)currentSubject);

        for (int i = 0; i < selectedSelection.buttons.Length; i++)
        {
            if (i <= currentContainer.completedQuestions)
                selectedSelection.buttons[i].interactable = true;
            else
                selectedSelection.buttons[i].interactable = false;
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

        if (a_button.boss)
            return;

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

        SetButtons();
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


        SetupGroups((int)currentSubject);

        starsUnlocked = list.equip.StarsAcquired[(((int)currentSubject * 10) + currentLevel)];
        if (currentLevel == 4 || currentLevel == 9 || currentLevel == 14)
        {
            SetHardMode(false);

            medal.gameObject.SetActive(true);

            medal.SetChangingTalisman(list, talismanManager, currentSubject);

            BossButton.gameObject.SetActive(true);
            NormalMode.gameObject.SetActive(false);
            HardButton.gameObject.SetActive(false);
            starOne.gameObject.SetActive(false);
            starTwo.gameObject.SetActive(false);
            starThree.gameObject.SetActive(false);
            bronzestarParticle.gameObject.SetActive(false);
            silverstarParticle.gameObject.SetActive(false);
            goldstarParticle.gameObject.SetActive(false);
            return;
        }
        NormalMode.gameObject.SetActive(true);
        BossButton.gameObject.SetActive(false);
        medal.gameObject.SetActive(false);
        HardButton.gameObject.SetActive(true);
        starOne.gameObject.SetActive(true);
        starTwo.gameObject.SetActive(true);
        starThree.gameObject.SetActive(true);
        starOne.interactable = (starsUnlocked >= 1);
        starTwo.interactable = (starsUnlocked >= 2);
        HardButton.interactable = ((starsUnlocked >= 2) && currentContainer.buttons[currentLevel].hardMode != null);

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

        if (currentSubject == classType.Calculi)
        {
            Debug.Log("checking");

            bool seal = FindObjectOfType<CombatStateManager>().CanContinueFort(currentLevel);


                if (!seal)
            {
                SealContainer.SetActive(true);


                if (currentLevel < 5)
                {
                    SealText.text = "Defeat Add+Sub. Also, don't cheat.Good Advice Colin :)<3";
                    return;
                }
                else if (currentLevel < 10)
                {
                    SealText.text = "Save Mt.Multiplication from Multisaurus to progress further!";
                }
                else
                {
                    SealText.text = "Save the Division Dunes from Divisor to face Lord Calculi once and for all!";
                }
            }
            else
            {
                SealContainer.SetActive(false);
            }
        }
        else
        {
            SealContainer.SetActive(false);
        }
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

    void SetupGroups(int subject)
    {
        for (int i = 0; i < subjectGroups.Length; i++)
        {
            subjectGroups[i].gameObject.SetActive(false);
        }
        subjectGroups[subject].gameObject.SetActive(true);

        selectedSelection = subjectGroups[subject];

    }
}