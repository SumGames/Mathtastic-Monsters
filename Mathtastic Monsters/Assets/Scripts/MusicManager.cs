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


    public AudioClip[] combatMusic = new AudioClip[6];
    public AudioClip[] bossMusic = new AudioClip[6];


    public float pitchLowest = 0.8f;

    public float pitchMidd = 1;

    public float pitchUpper = 1.25f;

    // Use this for initialization
    void Start()
    {
        setMusic(playStatus.Start);


        musicSource.volume = PlayerPrefs.GetFloat("Music", 0.3f);
        click.volume = PlayerPrefs.GetFloat("Volume", 0.3f);
    }

    public void AdjustPitch(int Value)
    {
        if (Value < 0)
        {
            musicSource.pitch = pitchLowest;
        }
        else if (Value == 0)
            musicSource.pitch = 1;
        else if (Value == 1)
        {
            musicSource.pitch = pitchMidd;
        }
        else
        {
            musicSource.pitch = pitchUpper;
        }


    }


    //Select a music clip, and check if the clip is different. If so, swap it in and play it.
    public void setMusic(playStatus newState)
    {
        if (musicSource == null)
            musicSource = gameObject.GetComponent<AudioSource>();


        musicSource.pitch = 1;


        AudioClip adding = null;

        musicSource.loop = true;

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
                musicSource.Stop();
                return;
            case playStatus.Won:
                musicSource.Stop();
                break;
            case playStatus.Lost:
                musicSource.loop = false;
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
            case playStatus.Credits:
                adding = music[16];
                break;
            case playStatus.Login:
                adding = music[17];
                break;
            case playStatus.Splash:
                adding = music[0];
                break;
            case playStatus.ArenaHome:
                adding = music[0];
                break;
            case playStatus.ArenaStart:
                adding = music[1];
                break;
            case playStatus.ArenaCombat:
                musicSource.Stop();
                return;
            case playStatus.ArenaContinue:
                adding = music[8];
                break;
            case playStatus.ArenaLost:
                adding = music[9];
                break;
            case playStatus.ArenaLeaderBoard:
                break;
            default:
                break;
        }
        if (adding != null && adding != musicSource.clip)
        {
            musicSource.clip = adding;
            musicSource.Play();
        }

    }

    internal void SetCombatMusic(operators a_op, bool a_boss)
    {
        AudioClip adding = null;

        int op;

        if ((int)a_op > (int)operators.Division)
        {
            op = (int)operators.Fortress;
        }
        else
        {
            op = (int)a_op;
        }

        if (a_boss)
        {
            adding = combatMusic[op];
        }
        else
            adding = bossMusic[op];

        if (adding != null)
        {
            musicSource.clip = adding;
            musicSource.Play();
        }

    }
}