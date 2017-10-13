////////////////////////////////////////////////////////////
//Author: Colin Butt
//Date Created: 01-Nov-16
//Brief: Contains the music clips found on different screnes
////////////////////////////////////////////////////////////


using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{

    public AudioClip[] music; //Available music clips that will be played by the audiosource.

    public AudioSource musicSource; //The listener that contains the current music clip that players in the scene.

    public AudioSource click;


    // Use this for initialization
    void Start()
    {        
        setMusic(playStatus.Start);


        bool haskey = PlayerPrefs.HasKey("Volume");
        if (haskey)
        {
            musicSource.volume = PlayerPrefs.GetFloat("Volume", 0);
        }
    }

    internal void setSlider()
    {
        FindObjectOfType<Slider>().value = PlayerPrefs.GetFloat("Volume", 0);
    }

    //Select a music clip, and check if the clip is different. If so, swap it in and play it.
    public void setMusic(playStatus newState)
    {
        if (musicSource == null)
            musicSource = gameObject.GetComponent<AudioSource>();


        AudioClip adding = null;

        switch (newState)
        {
            case playStatus.Start:
                adding = music[0];
                break;
            case playStatus.subjectSelect:
                adding = music[1];
                break;
            case playStatus.Addition:
                adding = music[2];
                break;
            case playStatus.Subtraction:
                adding = music[3];
                break;
            case playStatus.Multiplication:
                adding = music[4];
                break;
            case playStatus.Division:
                adding = music[5];
                break;
            case playStatus.MathFortress:
                adding = music[6];
                break;
            case playStatus.playing:
                adding = music[7];
                break;
            case playStatus.Won:
                adding = music[8];
                break;
            case playStatus.Lost:
                adding = music[9];
                break;
            case playStatus.MyMonster:
                adding = music[10];
                break;
            case playStatus.MonsterCustomisation:
                adding = music[11];
                break;
            case playStatus.LillyHome:
                adding = music[12];
                break;
            case playStatus.Options:
                adding = music[13];
                break;
            case playStatus.Instructions:
                adding = music[14];
                break;
            case playStatus.Parents:
                adding = music[15];
                break;
            case playStatus.Credits:
                adding = music[16];
                break;
            case playStatus.Login:
                adding = music[17];
                break;
            default:
                break;
        }
        if (adding != musicSource.clip && adding != null)
        {
            musicSource.clip = adding;
            musicSource.Play();
        }
    }
}