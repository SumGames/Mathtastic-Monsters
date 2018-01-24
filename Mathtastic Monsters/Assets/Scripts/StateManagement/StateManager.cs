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

    public Slider slider;


    //Start off by linking every internal object to each other.
    void Start()
    {
        base.Find();
        login.Begin(this, list);
        list = FindObjectOfType<equipmentList>();
        changeState(playStatus.Splash);
        backgrounds.startBack(playStatus.Splash);
        slider.value = PlayerPrefs.GetFloat("Volume", 0.6f);
    }

    public void MuteVolume()
    {
        slider.value = 0;
        PlayerPrefs.SetFloat("Volume", 0);
        music.musicSource.volume = 0;
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


    public void changeVolume(Slider used)
    {
        if (music == null)
        {
            music = FindObjectOfType<MusicManager>();
        }
        if (music.musicSource == null)
            music.musicSource = gameObject.GetComponent<AudioSource>();

        PlayerPrefs.SetFloat("Volume", used.value);
        music.musicSource.volume = used.value;
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

            case playStatus.Options:
                SceneManager.LoadScene(3);
                break;
            case playStatus.Splash:
                splashContainer.SetActive(true);
                break;
            case playStatus.ArenaHome:
                SceneManager.LoadScene(6);
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