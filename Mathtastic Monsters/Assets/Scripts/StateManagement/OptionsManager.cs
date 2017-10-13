//State manager. Only called in options scene.
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsManager : ParentsStateManager
{

    public GameObject optionsSelection; //Container for options select
    public GameObject instructionsSelection; //Above, for instructions
    public GameObject parentsSelection; //Above, for parents screen
    public GameObject creditsSelection; //Above, for team credits


    Text gameInstruction; //Tells player what to do.


    MusicManager music;

    // Use this for initialization
    void Start()
    {
        base.Find();

        gameInstruction = GameObject.Find("Helper").GetComponent<Text>();
        list = FindObjectOfType<equipmentList>();

        FindObjectOfType<MusicManager>().setSlider();

        changeState(playStatus.Options);

        backgrounds.startBack(playStatus.Options);

    }

    public void changeVolume(Slider used)
    {
        if (music == null)
        {
            music = FindObjectOfType<MusicManager>();
            return;
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
            case playStatus.Options:
                optionsSelection.SetActive(true);
                gameInstruction.text = "Select an Option!";
                break;
            case playStatus.Instructions:
               instructionsSelection.SetActive(true);
                gameInstruction.text = "Learn how to play!";
                break;
            case playStatus.Parents:
                parentsSelection.SetActive(true);
                gameInstruction.text = "Parents and Teachers section";
                break;
            case playStatus.Credits:
                creditsSelection.SetActive(true);
                gameInstruction.text = "The wonderful Team at 'Sum Games'!";
                break;
            case playStatus.subjectSelect:
                SceneManager.LoadScene(0);
                break;
            default:
                break;
        }
    }


    //Disables all objects by default so it doesn't have to be done manually.
    void disableObjects()
    {
        optionsSelection.SetActive(false);
        instructionsSelection.SetActive(false);
        parentsSelection.SetActive(false);
        creditsSelection.SetActive(false);
    }
}
