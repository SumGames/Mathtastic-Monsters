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

    // Use this for initialization
    void Start()
    {
        myButton = gameObject.GetComponent<Button>();
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
                subjectSelect.SetActive(false);
                levelSelect.SetActive(false);
                combatContainer.SetActive(false);

                lillyText.text = "";
                exposition.SetActive(true);
                myButton.interactable = true;

                foreach (GameObject item in OutlineGlows)
                    item.SetActive(false);

                break;
            case 2:
                lillyText.text = "This way to tutorias!\nHurry!";
                exposition.SetActive(false);
                subjectSelect.SetActive(true);
                myButton.interactable = false;
                break;
            case 3:
                lillyText.text = "Over here, hero! The beast has found me!";
                subjectSelect.SetActive(false);
                levelSelect.SetActive(true);
                break;
            case 4:
                lillyText.text = "Hello, player! The beast is frozen, but it won't last long. Let me explain.";
                combatContainer.SetActive(true);
                calculator.SetActive(false);
                player.ResetPlayer();
                TutorialMonster.LoadMonster();

                player.Frozen = 3;
                tutorialCalculator.ButtonsActive(false);
                levelSelect.SetActive(false);
                myButton.interactable = true;

                TutorialMonster.MakeQuestion(2);
                break;
            case 5:
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
                calculator.SetActive(true);
                break;
            case 10:
                lillyText.text = "Tapping on numbers writes your answer";
                OutlineGlows[3].SetActive(false);
                OutlineGlows[4].SetActive(true);
                break;
            case 11:
                lillyText.text = "Hitting attack will submit your answer!";
                OutlineGlows[4].SetActive(false);
                OutlineGlows[5].SetActive(true);
                break;
            case 12:
                lillyText.text = "Hitting Cancel will delete your answer!";
                OutlineGlows[5].SetActive(false);
                OutlineGlows[6].SetActive(true);
                break;


            case 13:
                lillyText.text = "When your gauge hits 0, the enemy will attack.";
                TutorialMonster.MakeQuestion(1);
                OutlineGlows[6].SetActive(false);
                break;

            case 14:
                lillyText.text = "To evade, hit the button with the Correct Answer";
                break;

            case 15:
                lillyText.text = "I'll now lower the freeze barrier when you're ready. Good luck, Hero!";
                break;

            case 16:
                lillyText.text = "Answer the monster's questions to lower its health!!";
                TutorialMonster.MakeQuestion(2);
                player.Frozen = 0;
                tutorialCalculator.ButtonsActive(true);
                myButton.interactable = false;
                break;
            case 17:
                lillyText.text = "Hero! Please be more careful! I had to freeze the beast again.";
                myButton.interactable = true;
                player.Frozen = 3;
                tutorialCalculator.ButtonsActive(false);
                break;

            case 18:
                if (!victory)
                {
                    SetStep(15);
                    //TutorialMonster.LoadMonster();
                    player.ResetPlayer();
                    return;
                }
                myButton.interactable = true;
                tutorialCalculator.ButtonsActive(false);
                lillyText.text = "The beast has fallen! Good work, Hero!";
                TutorialMonster.gameObject.SetActive(false);

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
        SetStep(18);
        player.Frozen = 3;
        tutorialCalculator.ButtonsActive(false);
    }
}