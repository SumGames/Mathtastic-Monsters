//State manager. Called on scene containing: Login, Main menu. Subject selection, Questions/Combat.
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StateManager : ParentsStateManager
{

    public Player player;
    public Monster enemy;

    public MonsterManager monsterM;

    public GameObject enemyContainer;

    public GameObject splashContainer;
//    public GameObject login;
    public GameObject startContainer; //Container of the start screen.

    public GameObject subjectContainer; //Container for level select.
    public GameObject additionSelection; //The container where additions selection buttons are.
    public GameObject subtractionSelection; //Above, for subtraction.
    public GameObject multiplicationSelection; //Above, for division
    public GameObject divisionSelection; //Above, for divisoon
    public GameObject mathfortressSelection; //Above, for Math Fortress

    public GameObject combatContainer; //Container which has player, calculator and enemy.
    public GameObject resetContainer; // The back and restart buttons, that appear after a level finishes.

    public GameObject retreatObject;

    Text gameInstruction; //Tells player what to do.



    public Button[] subjectButtons; //Links to the subject buttons. Since we don't want players progressing until they beat early levels.

    public Next nextButton;

    public multipleContainer container;

    public loginManager login;

    //Start off by linking every internal object to each other.
    void Start()
    {
        base.Find();


        login.Begin(this, list);


        gameInstruction = GameObject.Find("Helper").GetComponent<Text>();


        player.parent = monsterM;
        player.enemy = enemy;
        player.manager = this;

        enemy.manager = this;
        enemy.parent = monsterM;
        enemy.player = player;


        monsterM.stateManager = this;
        monsterM.monster = enemy;
        monsterM.player = player;

        list = FindObjectOfType<equipmentList>();

        if (list.playerName == "")
        {
            changeState(playStatus.Splash);
            backgrounds.startBack(playStatus.Splash);
        }
        else
        {
            changeState(playStatus.subjectSelect);
            backgrounds.startBack(playStatus.subjectSelect);
        }        
    }


    //Revert back to lesson's subject. 
    public void backToSubject()
    {
        if (!monsterM.quizRunning)
        {
            changeState(playStatus.subjectSelect);
        }

        switch (monsterM.quizRunning.Operator)
        {
            case operators.Addition:
                changeState(playStatus.Addition);
                break;
            case operators.Subtraction:
                changeState(playStatus.Subtraction);
                break;
            case operators.Multiplication:
                changeState(playStatus.Multiplication);
                break;
            default:
                break;
        }
    }

    //Change the game's state, closing/opening containers and changing text.
    public override void changeState(playStatus newState)
    {
        base.changeState(newState);

        disableObjects();

        gameState = newState;
        switch (gameState)
        {
            /*
            case playStatus.Login:
                list.Save();
                gameInstruction.text = "";
                login.SetActive(true);
                break;
             */
            case playStatus.Start:
                gameInstruction.text = "";
                startContainer.SetActive(true);
                break;
            case playStatus.subjectSelect:
                checkLevelsAvailable();
                monsterM.quizRunning = null;
                subjectContainer.SetActive(true);
                gameInstruction.text = "Select a subject!";
                break;
            case playStatus.Addition:
                awakenSubject(additionSelection);
                additionSelection.SetActive(true);
                gameInstruction.text = "Select a level!";                
                break;
            case playStatus.Subtraction:
                awakenSubject(subtractionSelection);
                subtractionSelection.SetActive(true);
                gameInstruction.text = "Select a level!";
                break;
            case playStatus.Multiplication:
                awakenSubject(multiplicationSelection);
                multiplicationSelection.SetActive(true);
                gameInstruction.text = "Select a level!";
                break;
            case playStatus.Division:
                awakenSubject(divisionSelection);
                divisionSelection.SetActive(true);
                gameInstruction.text = "Select a level!";
                break;
            case playStatus.MathFortress:
                awakenSubject(multiplicationSelection);
                mathfortressSelection.SetActive(true);
                gameInstruction.text = "Select a level!";
                break;
            case playStatus.playing:
                player.gameObject.SetActive(true);
                enemyContainer.gameObject.SetActive(true);
                combatContainer.SetActive(true);
                retreatObject.SetActive(true);
                container.gameObject.SetActive(true);
                gameInstruction.text = "Click on buttons to answer questions!";
                break;
            case playStatus.Lost:
                combatContainer.SetActive(true);
                gameInstruction.text = "You Lost, don;t worry! You can try again :)";
                FindObjectOfType<TorsoPart>().Animate(Animations.Dead);
                resetContainer.SetActive(true);
                container.gameObject.SetActive(false);
                break;

            case playStatus.Won:
                nextButton.ready();
                combatContainer.SetActive(true);
                gameInstruction.text = "You won! I knew you could do it";
                enemyContainer.gameObject.SetActive(false);
                resetContainer.SetActive(true);
                int exp = player.CalculateExperience();
                list.equip.shards += exp;
                gameInstruction.text = "You won, earning " + exp.ToString("F0") + " Monster Shards!";
                container.gameObject.SetActive(false);
                break;
            case playStatus.MyMonster:
                SceneManager.LoadScene(1);
                break;

            case playStatus.Options:
                SceneManager.LoadScene(2);
                break;

            case playStatus.Splash:
                gameInstruction.text = "";
                splashContainer.SetActive(true);
                break;

            case playStatus.ArenaHome:
                SceneManager.LoadScene(5);
                break;
            default:
                break;
        }
    }


    void awakenSubject(GameObject subject)
    {
        questionContainer sub = subject.GetComponent<questionContainer>();
        sub.Awaken();
    }

    //If the previous 
    //list.equip.completedLevels[] is an array starting at addition.
    //SubjectButtons[] is an array starting at subtraction, as addition will never be disabled.
    void checkLevelsAvailable()
    {

        subjectButtons[0].interactable = true;

        //For Mult, checking Add.
        if (list.equip.completedLevels[0] >= 5)
            subjectButtons[1].interactable = true;
        else
            subjectButtons[1].interactable = false;

        //For Div, checking mult.
        if (list.equip.completedLevels[1] >= 5)
            subjectButtons[2].interactable = true;
        else
            subjectButtons[2].interactable = false;

        //For Mult, checking sub.
        if (list.equip.completedLevels[0] > 9 && list.equip.completedLevels[1] > 9 && list.equip.completedLevels[2] > 9)
            subjectButtons[3].interactable = true;
        else
            subjectButtons[3].interactable = false;
    }


    //Disables all objects by default so it doesn't have to be done manually.
    void disableObjects()
    {
        combatContainer.SetActive(false);
        subjectContainer.SetActive(false);
        resetContainer.SetActive(false);
        additionSelection.SetActive(false);
        subtractionSelection.SetActive(false);
        multiplicationSelection.SetActive(false);
        divisionSelection.SetActive(false);
        mathfortressSelection.SetActive(false);
        //login.SetActive(false);
        splashContainer.SetActive(false);
        nextButton.gameObject.SetActive(false);
        retreatObject.SetActive(false);
    }
}