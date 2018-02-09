using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StepManager : MonoBehaviour
{
    public Text lillyText;
    public Image lillySprite;
    int tutorialStage;

    public GameObject exposition;
    public GameObject subjectSelect;
    public GameObject levelSelect;

    public Button myButton;


    bool victory;

    public TutorialMonster TutorialMonster;
    public TutorialCalculator tutorialCalculator;
    public TutorialPlayer player;

    public GameObject combatContainer;
    public GameObject calculator;

    public GameObject[] OutlineGlows;

    equipmentList list;
    public GameObject prefabbedList;

    public GameObject backPrefab;
    GameObject backs;

    internal backgroundManager backgrounds;


    public Text QuestionText;
    public Text InputText;


    public Button AttackChoice;
    public Button ConfirmAttack;

    public Text[] multWrong;
    public Button multRight;
    public Button multSubmit;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;

        list = FindObjectOfType<equipmentList>();
        if (list == null)
        {
            GameObject adding = Instantiate(prefabbedList, null, false);
            list = adding.GetComponent<equipmentList>();
            adding.name = "List";

            StateManager check = gameObject.GetComponent<StateManager>();

            if (!check)
                list.startGame("Guest", true);
        }

        GameObject can = GameObject.Find("Canvas");
        backs = Instantiate(backPrefab, can.transform, false);

        backgrounds = backs.GetComponent<backgroundManager>();

        backgrounds.startBack(playStatus.subjectSelect);




        myButton = gameObject.GetComponent<Button>();
        SetStep(2);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ProgressTutorial()
    {
        tutorialStage++;

        SetStep(tutorialStage);
    }

    void SetStep(int a_step)
    {
        tutorialStage = a_step;

        switch (a_step)
        {
            case 1:
                lillyText.text = "";
                exposition.SetActive(true);
                myButton.interactable = true;

                break;
            case 2:
                foreach (GameObject item in OutlineGlows)
                    item.SetActive(false);

                subjectSelect.SetActive(false);
                levelSelect.SetActive(false);
                combatContainer.SetActive(false);
                lillyText.text = "Hello, Hero!\n Can you come find me? I'm waiting on my island.";
                exposition.SetActive(false);
                subjectSelect.SetActive(true);
                myButton.interactable = false;
                break;
            case 3:
                lillyText.text = "Hurry! I hear a monster growing closer!";
                subjectSelect.SetActive(false);
                levelSelect.SetActive(true);
                FindObjectOfType<equipmentList>().RemoveHead();
                break;
            case 4:
                lillyText.text = "Hello, player! The beast is frozen. Let me explain.";
                combatContainer.SetActive(true);
                calculator.SetActive(false);
                player.ResetPlayer();
                TutorialMonster.LoadMonster();

                player.Frozen = 3;
                tutorialCalculator.ButtonsActive(false);
                levelSelect.SetActive(false);
                myButton.interactable = true;

                TutorialMonster.MakeQuestion(2);
                QuestionText.text = "4 \n+2\n=";
                break;
            case 5:
                QuestionText.text = "4 \n+2\n=";
                lillyText.text = "This is your health bar. We'll lose if it runs out.";
                OutlineGlows[0].SetActive(true);
                break;
            case 6:
                lillyText.text = "This is the enemy's health. Empty it to win!";
                OutlineGlows[0].SetActive(false);
                OutlineGlows[1].SetActive(true);
                break;
            case 7:
                lillyText.text = "This is the timer. The monster will attack when it hits 0.";
                OutlineGlows[1].SetActive(false);
                OutlineGlows[2].SetActive(true);
                break;
            case 8:
                lillyText.text = "This is its question. Answering it will weaken the beast";
                OutlineGlows[2].SetActive(false);
                OutlineGlows[3].SetActive(true);
                break;
            case 9:
                lillyText.text = "But answering questions requires the relic! The monsterlator!";
                QuestionText.text = "4 \n+2\n=";
                calculator.SetActive(true);
                break;
            case 10:
                myButton.interactable = false;
                AttackChoice.interactable = true;
                lillyText.text = "Tapping on numbers writes your answer";
                QuestionText.text = "4 \n+2\n=";
                OutlineGlows[3].SetActive(false);
                OutlineGlows[4].SetActive(true);
                break;
            case 11:
                AttackChoice.gameObject.SetActive(false);
                ConfirmAttack.interactable = true;

                lillyText.text = "Hitting attack will submit your answer!";
                QuestionText.text = "4 \n+2\n=";
                InputText.text = "6";
                OutlineGlows[4].SetActive(false);
                OutlineGlows[5].SetActive(true);
                break;
            case 12:
                myButton.interactable = true;
                ConfirmAttack.gameObject.SetActive(false);
                QuestionText.text = "";
                InputText.text = "";
                lillyText.text = "Good work! Note that answering while the timer is in the Green zone will result in double damage!";
                OutlineGlows[5].SetActive(false);
                break;


            case 13:
                QuestionText.text = "4\n+5=";
                lillyText.text = "Once you've attacked, the enemy will attack with a multiple choice question!";
                multRight.gameObject.SetActive(true);
                multRight.interactable = false;
                multSubmit.gameObject.SetActive(true);
                multSubmit.interactable = false;

                TutorialMonster.MakeQuestion(1);
                OutlineGlows[6].SetActive(false);
                QuestionText.text = "4\n+5=";
                for (int i = 0; i < multWrong.Length; i++)
                {
                    multWrong[i].text = i.ToString();
                }
                break;

            case 14:
                QuestionText.text = "4\n+5=";
                multRight.interactable = true;                
                myButton.interactable = false;
                lillyText.text = "To evade, hit the button with the Correct Answer.";
                break;

            case 15:
                multRight.interactable = false;
                multSubmit.interactable = true;
                lillyText.text = "Good! Now tap the Defend to confirm your answer and block the attack!";
                break;

            case 16:
                multRight.gameObject.SetActive(false);
                multSubmit.gameObject.SetActive(false);
                myButton.interactable = true;
                lillyText.text = "Yay! By answering questions in this phase, you avoid damage! Answering quickly will result in a counter!";
                break;
            case 17:
                lillyText.text = "I'll now lower the freeze barrier when you're ready.";
                break;

            case 18:
                lillyText.text = "Answer the monster's questions to lower its health!!";
                TutorialMonster.MakeQuestion(2);
                player.Frozen = 0;
                tutorialCalculator.ButtonsActive(true);
                myButton.interactable = false;
                break;
            case 19:
                lillyText.text = "Hero! Please be more careful! I had to freeze the beast again.";
                myButton.interactable = true;
                player.Frozen = 3;
                tutorialCalculator.ButtonsActive(false);
                break;

            case 20:
                if (!victory)
                {
                    SetStep(18);
                    player.ResetPlayer();
                    return;
                }
                myButton.interactable = true;
                tutorialCalculator.ButtonsActive(false);
                lillyText.text = "The beast has fallen! Good work, Hero!";
                if (!list.equip.tutorialComplete)
                {
                    list.equip.shards += 20;
                    lillyText.text += "\nYou also got 20 shards!";
                    list.equip.tutorialComplete = true;
                }                
                TutorialMonster.gameObject.SetActive(false);
                TutorialMonster.healthBar.gameObject.SetActive(false);

                break;

            case 21:
                lillyText.text = "Since you're here, why don't I show you how to make parts?";
                break;
            default:
                Destroy(gameObject);
                SceneManager.LoadScene(4);
                break;
        }
    }

    internal void playerWon()
    {
        victory = true;
        SetStep(20);
        player.Frozen = 3;
        tutorialCalculator.ButtonsActive(false);
    }
}