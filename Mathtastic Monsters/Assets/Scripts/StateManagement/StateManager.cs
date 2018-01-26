//State manager. Called on scene containing: Login, Main menu. Subject selection, Questions/Combat.
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StateManager : ParentsStateManager
{
    public MonsterManager monsterM;

    public GameObject enemyContainer;

    public GameObject splashContainer;
//    public GameObject login;
    public GameObject startContainer; //Container of the start screen.


    Text gameInstruction; //Tells player what to do.



    public Next nextButton;

    public loginManager login;

    public MusicManager music;


    //Start off by linking every internal object to each other.
    void Start()
    {
        base.Find();
        login.Begin(this, list);
        list = FindObjectOfType<equipmentList>();
        changeState(playStatus.Splash);
        backgrounds.startBack(playStatus.Splash);

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
            //need to set proper integer here.;

            case operators.Addition:
                changeState(playStatus.Addition);                
                break;
            case operators.Subtraction:
                changeState(playStatus.Subtraction);
                break;
            case operators.Multiplication:
                changeState(playStatus.Multiplication);
                break;
            case operators.Division:
                changeState(playStatus.Division);
                break;
            default:
                changeState(playStatus.MathFortress);
                break;                
        }
        FindObjectOfType<LevelSelection>().ChangeIndex(monsterM.quizRunning.quizIndex);
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
                startContainer.SetActive(true);
                break;
            case playStatus.subjectSelect:
                SceneManager.LoadScene(1);
                break;
            case playStatus.MyMonster:
                SceneManager.LoadScene(2);
                break;

            case playStatus.Splash:
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


    //Disables all objects by default so it doesn't have to be done manually.
    void disableObjects()
    {
        splashContainer.SetActive(false);


    }
}