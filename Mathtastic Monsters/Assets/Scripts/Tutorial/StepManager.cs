using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StepManager : MonoBehaviour
{
    public Text lillyText;
    public Image lillySprite;
    int tutorialStage;


    public GameObject levelSelect;

    public Button myButton;


    bool victory;

    public TutorialMonster TutorialMonster;
    public TutorialCalculator tutorialCalculator;
    public TutorialPlayer player;

    public GameObject combatContainer;
    public GameObject calculator;


    equipmentList list;
    public GameObject prefabbedList;

    public GameObject backPrefab;
    GameObject backs;

    internal backgroundManager backgrounds;


    public Text QuestionText;
    public Text InputText;


    public Button AttackChoice;
    public Button ConfirmAttack;

    public GameObject multContainer;

    public Text[] multWrong;
    public Button multRight;
    public Button multSubmit;

    public GameObject[] outlinesAndArrows;

    public Button toAddition;

    public GameObject winContainer;
    public WinAnimation winAnimation;


    public GameObject[] combatStuff;

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


        SetStep(1);
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
                winContainer.SetActive(false);
                multContainer.SetActive(false);
                enableOutlines(-1);

                combatContainer.SetActive(false);
                lillyText.text = "Hi there, " + list.playerName + "!\nWelcome to the world of Monstertopia!";
                toAddition.interactable = false;
                levelSelect.SetActive(true);
                myButton.interactable = true;
                break;

            case 2:
                lillyText.text = "My name is Lilly! I need your help to beat Lord Calculi! He has stolen math from the world and you are my last hope!";
                break;
            case 3:
                enableOutlines(0);
                toAddition.interactable = true;
                myButton.interactable = false;
                lillyText.text = "Theres a monster on my island! Come over quick!";
                levelSelect.SetActive(true);

                break;
            case 4:
                enableOutlines(-1);
                lillyText.text = "I've frozen the monster for now. But I don't have much time. Let me explain.";
                combatContainer.SetActive(true);
                calculator.SetActive(false);
                player.ResetPlayer();
                TutorialMonster.LoadMonster();

                player.Frozen = 3;
                tutorialCalculator.ButtonsActive(false);
                levelSelect.SetActive(false);
                myButton.interactable = true;

                TutorialMonster.MakeQuestion(2);
                QuestionText.text = "4 + 2 = ";
                break;
            case 5:
                calculator.SetActive(true);
                lillyText.text = "You'll need to use the Monsterlator. Let me tell you how it works!";
                enableOutlines(1);
                break;
            case 6:
                lillyText.text = "This is your health. You'll be defeated if it's empty.";
                enableOutlines(6);
                break;
            case 7:

                lillyText.text = "This is the enemy monsters health. Empty it to defeat it!";
                enableOutlines(4);
                break;
            case 8:
                lillyText.text = "This is the timer. You must answer the math question before it runs out! Or you may get hurt";
                enableOutlines(2);
                break;
            case 9:
                lillyText.text = "This is the enemy question. Answering it will weaken the monster!";
                enableOutlines(7);
                break;
            case 10:
                myButton.interactable = false;
                AttackChoice.interactable = true;
                lillyText.text = "Tapping on numbers will put your answer in the Monsterlator. Tap the answer to 4 + 2 now!";
                QuestionText.text = "4 + 2 = ";
                enableOutlines(1);                
                break;
            case 11:
                AttackChoice.gameObject.SetActive(false);
                ConfirmAttack.interactable = true;

                lillyText.text = "Tapping attack will submit your answer!";
                QuestionText.text = "4 + 2 = ";
                InputText.text = "6";
                enableOutlines(3);
                break;
            case 12:
                myButton.interactable = true;
                ConfirmAttack.gameObject.SetActive(false);
                QuestionText.text = "";
                InputText.text = "";
                lillyText.text = "Good work! answer quickly while the timer is green and you'll deal double damage!";
                enableOutlines(-1);
                break;

            case 13:
                multContainer.SetActive(true);
                calculator.SetActive(false);

                QuestionText.text = "4\n+5=";
                lillyText.text = "Once you've attacked, the enemy will attack with a multiple choice question!";
                multRight.gameObject.SetActive(true);
                multRight.interactable = false;
                multSubmit.gameObject.SetActive(true);
                multSubmit.interactable = false;

                TutorialMonster.MakeQuestion(1);
                enableOutlines(1);
                QuestionText.text = "4 + 5 = ";
                for (int i = 0; i < multWrong.Length; i++)
                {
                    multWrong[i].text = i.ToString();
                }
                break;

            case 14:
                enableOutlines(5);
                multContainer.SetActive(true);
                QuestionText.text = "4 + 5 = ";
                multRight.interactable = true;                
                myButton.interactable = false;
                lillyText.text = "To evade, tap the button with the correct answer!";
                break;

            case 15:
                enableOutlines(3);
                multContainer.SetActive(true);
                multRight.interactable = false;
                multSubmit.interactable = true;
                lillyText.text = "Good! Now tap the Defend button to confirm your answer and block the attack!";
                break;

            case 16:
                enableOutlines(-1);
                QuestionText.text = "";
                multRight.gameObject.SetActive(false);
                multSubmit.gameObject.SetActive(false);
                myButton.interactable = true;
                lillyText.text = "Yay! By answering questions in the enemy turn, you avoid damage! Answering quickly will result in a counter!";
                break;
            case 17:
                multContainer.SetActive(false);
                calculator.SetActive(true);
                lillyText.text = "I can't hold the freeze much longer.I'll lower it when you're ready!";
                break;

            case 18:
                lillyText.text = "Answer the monsters questions to lower its health!!";
                TutorialMonster.MakeQuestion(2);
                player.Frozen = 0;
                tutorialCalculator.ButtonsActive(true);
                myButton.interactable = false;
                break;
            case 19:
                lillyText.text = "Hero! Please be more careful! I had to freeze the beast again. I don't have much power left!";
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
                winContainer.SetActive(true);
                winAnimation.GiveShards(20);
                myButton.interactable = true;
                lillyText.text = "The monster has fallen! Good work, Hero!";

                foreach (GameObject item in combatStuff)
                {
                    item.SetActive(false);
                }

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
                lillyText.text = "Since you're here, why don't I show you how to make parts using your shards!";
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

    void enableOutlines(int index)
    {
        for (int i = 0; i < outlinesAndArrows.Length; i++)
        {
            if (i == index)
                outlinesAndArrows[i].SetActive(true);
            else
                outlinesAndArrows[i].SetActive(false);
        }
    }

}