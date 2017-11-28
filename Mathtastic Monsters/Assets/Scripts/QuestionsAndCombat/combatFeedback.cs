using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SetImage
{
    EnemyHit,
    EnemyCrit,
    PlayerHit,
    PlayerDodged,
    PlayerMissed,
    PlayerCountered
}
public class combatFeedback : MonoBehaviour
{
    public Sprite attackHit;
    public Sprite attackCrit;
    public Sprite dodged;
    public Sprite Missed;
    public Sprite Countered;
    public Sprite playerHurt;
    public Image PlayerImage;
    public Image EnemyImage;
    public AudioSource[] sounds;
    public AudioSource attack;
    public AudioSource hurt;
    public AudioSource miss;
    public AudioSource crit;
    public float resetTimer = 3;
    float timer;
    public TorsoPart body;
    public ParticleSystem[] enemyhitParticle;
    public ParticleSystem[] enemycritParticle;
    public ParticleSystem[] playerhitParticle;
    public ParticleSystem[] enemymissedParticle;
    public ParticleSystem[] playermissedParticle;
    public ParticleSystem[] playercounteredParticle;
    public GameObject particleEnemyhit;
    public GameObject particleEnemycrit;
    public GameObject particlePlayerhit;
    public GameObject particleEnemymissed;
    public GameObject particlePlayermissed;
    public GameObject particlePlayercountered;
    void Start()
    {
        enemyhitParticle = particleEnemyhit.GetComponentsInChildren<ParticleSystem>();
        enemycritParticle = particleEnemycrit.GetComponentsInChildren<ParticleSystem>();
        playerhitParticle = particlePlayerhit.GetComponentsInChildren<ParticleSystem>();
        enemymissedParticle = particleEnemymissed.GetComponentsInChildren<ParticleSystem>();
        playermissedParticle = particlePlayermissed.GetComponentsInChildren<ParticleSystem>();
        playercounteredParticle = particlePlayercountered.GetComponentsInChildren<ParticleSystem>();
        sounds = GetComponents<AudioSource>();
        attack = sounds[0];
        hurt = sounds[1];
        miss = sounds[2];
        crit = sounds[3];      
        PlayerImage.enabled = false;
        EnemyImage.enabled = false;
    }
    void Update()
    {
        if (timer >= 0)
        {            
            timer -= Time.deltaTime;
        }
        else
        {
            PlayerImage.enabled = false;
            EnemyImage.enabled = false;
        }
    }
    internal void DamageSet(SetImage setImage)
    {
        if (!body)
            body = FindObjectOfType<TorsoPart>();
        timer = 3;
        switch (setImage)
        {
            case SetImage.EnemyHit:
                EnemyImage.sprite = attackHit;
                EnemyImage.enabled = true;
                PlayerImage.enabled = false;
                body.Animate(Animations.Attack);
                PlaySound(setImage);
                foreach (ParticleSystem item in enemyhitParticle)
                {
                    item.Play();
                }
                break;
            case SetImage.EnemyCrit:
                EnemyImage.sprite = attackCrit;
                EnemyImage.enabled = true;
                PlayerImage.enabled = false;
                body.Animate(Animations.Attack);
                PlaySound(setImage);
                foreach (ParticleSystem item in enemycritParticle)
                {
                    item.Play();
                }
                break;
            case SetImage.PlayerHit:
                PlayerImage.sprite = playerHurt;
                PlayerImage.enabled = true;
                EnemyImage.enabled = false;
                body.Animate(Animations.Hurt);
                PlaySound(setImage);
                foreach (ParticleSystem item in playerhitParticle)
                {
                    item.Play();
                }
                break;
            case SetImage.PlayerDodged:
                PlayerImage.sprite = dodged;
                PlayerImage.enabled = true;
                EnemyImage.enabled = false;
                PlaySound(setImage);
                foreach (ParticleSystem item in enemymissedParticle)
                {
                    item.Play();
                }
                break;
            case SetImage.PlayerMissed:
                EnemyImage.sprite = Missed;
                EnemyImage.enabled = true;
                PlayerImage.enabled = false;
                body.Animate(Animations.Hurt);
                PlaySound(setImage);
                foreach (ParticleSystem item in playermissedParticle)
                {
                    item.Play();
                }
                break;
            case SetImage.PlayerCountered:
                body.Animate(Animations.Attack);
                EnemyImage.sprite = Countered;
                PlayerImage.enabled = false;
                EnemyImage.enabled = true;
                PlaySound(setImage);
                foreach (ParticleSystem item in playercounteredParticle)
                {
                    item.Play();
                }
                break;
            default:
                break;
        }
    }
    void PlaySound(SetImage a_sound)
    {
        AudioSource playing = null;
        switch (a_sound)
        {
            case SetImage.EnemyHit:
                playing = attack;
                break;
            case SetImage.EnemyCrit:
                playing = crit;
                break;
            case SetImage.PlayerHit:
                playing = hurt;
                break;
            case SetImage.PlayerDodged:
                playing = miss;
                break;
            case SetImage.PlayerMissed:
                playing = miss;
                break;
            case SetImage.PlayerCountered:
                playing = attack;
                break;
            default:
                break;
        }
        if (playing != null)
        {
            playing.volume = PlayerPrefs.GetFloat("Volume", 0.6f);
            playing.Play();
        }
    }
}