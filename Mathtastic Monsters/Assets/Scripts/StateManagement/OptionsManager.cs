//State manager. Only called in options scene.
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{

    public GameObject optionsSelection; //Container for options select
    public GameObject SoundsOptions; //Under Parents.
    public GameObject creditsSelection; //Above, for team credits
    public GameObject otherOptions;

    MusicManager music;

    public Toggle toggle;

    public playStatus optionState;

    public Slider MusicSlider;
    public Slider VolumeSlider;


    GameObject starParticle;
    GameObject gold;


    // Use this for initialization
    void Start()
    {
        music = FindObjectOfType<MusicManager>();

        MusicSlider.value = PlayerPrefs.GetFloat("Music", 0.3f);
        VolumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.3f);

        gold = GameObject.Find("BronzeWin");
        if(gold)
        {
            if (gold.activeSelf)
                gold.SetActive(false);
            else
                gold = null;
        }

        starParticle = GameObject.Find("BronzeParticle");
        if (starParticle)
        {
            if (starParticle.activeSelf)
                starParticle.SetActive(false);
            else
                starParticle = null;
        }

        changeState(playStatus.Options);


        SetTransition();

        Time.timeScale = 0.01f;

        StateManager state = FindObjectOfType<StateManager>();

        if (state)
        {
            Destroy(Tut.gameObject);
        }
    }

    public void changeVolume(Slider used)
    {
        if (used == MusicSlider)
        {
            if (music == null)
            {
                music = FindObjectOfType<MusicManager>();
            }
            PlayerPrefs.SetFloat("Music", used.value);
            music.musicSource.volume = used.value;
        }
        else if(used == VolumeSlider)
        {
            if (music == null)
            {
                music = FindObjectOfType<MusicManager>();
            }
            if (music.musicSource == null)
                music.musicSource = gameObject.GetComponent<AudioSource>();

            PlayerPrefs.SetFloat("Volume", used.value);
            music.click.volume = used.value;


        }
    }

    public void MuteVolume()
    {
        VolumeSlider.value = 0;
        PlayerPrefs.SetFloat("Volume", 0);
        music.click.volume = 0;

        MusicSlider.value = 0;
        PlayerPrefs.SetFloat("Music", 0);
        music.musicSource.volume = 0;
    }

    public GameObject Tut;

    public void LoadTutorial()
    {
        SceneManager.LoadScene(3);


    }


    //Change the game's state, closing/opening containers and changing text.
    public void changeState(playStatus newState)
    {
        DisableObjects();

        optionState = newState;
        switch (optionState)
        {
            case playStatus.Options:
                optionsSelection.SetActive(true);
                break;
            case playStatus.SoundControl:
                SoundsOptions.SetActive(true);
                break;
            case playStatus.OtherOptions:
                otherOptions.SetActive(true);
                break;
            case playStatus.Credits:
                creditsSelection.SetActive(true);
                break;
            default:
                if (starParticle)
                    starParticle.gameObject.SetActive(true);

                if (gold)
                    gold.gameObject.SetActive(true);
                Destroy(gameObject);
                Time.timeScale = 1;
                break;
        }
    }


    //Disables all objects by default so it doesn't have to be done manually.
    void DisableObjects()
    {
        optionsSelection.SetActive(false);
        otherOptions.SetActive(false);
        SoundsOptions.SetActive(false);
        creditsSelection.SetActive(false);
    }

    public void TransitionsOn(Toggle a_trans)
    {
        int value = PlayerPrefs.GetInt("Transitions", 1);
       
        if (toggle.isOn)
        {
            value = 1;
        }
        else
        {
            value = 0;
        }

        PlayerPrefs.SetInt("Transitions", value);

    }

    void SetTransition()
    {
        int value = PlayerPrefs.GetInt("Transitions", 1);

        if (value == 1)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }
}
