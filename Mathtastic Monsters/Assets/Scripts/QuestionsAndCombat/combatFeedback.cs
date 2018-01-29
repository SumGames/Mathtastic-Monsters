using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SetFeedback
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
    public AudioSource[] sounds;
    public AudioSource attack;
    public AudioSource hurt;
    public AudioSource miss;
    public AudioSource crit;
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
    public float resetTimer = 3;
    float timer;
    public TorsoPart body;
    void Start()
    {
        if (particleEnemyhit)
        {
            enemyhitParticle = particleEnemyhit.GetComponentsInChildren<ParticleSystem>();
            enemycritParticle = particleEnemycrit.GetComponentsInChildren<ParticleSystem>();
            playerhitParticle = particlePlayerhit.GetComponentsInChildren<ParticleSystem>();
            enemymissedParticle = particleEnemymissed.GetComponentsInChildren<ParticleSystem>();
            playermissedParticle = particlePlayermissed.GetComponentsInChildren<ParticleSystem>();
            playercounteredParticle = particlePlayercountered.GetComponentsInChildren<ParticleSystem>();
        }
        sounds = GetComponents<AudioSource>();
        attack = sounds[0];
        hurt = sounds[1];
        miss = sounds[2];
        crit = sounds[3];      
    }
    void Update()
    {
        if (timer >= 0)
        {            
            timer -= Time.deltaTime;
        }
    }
    internal void DamageSet(SetFeedback setFeedback)
    {
        if (!body)
            body = FindObjectOfType<TorsoPart>();
        timer = 3;
        switch (setFeedback)
        {
            case SetFeedback.EnemyHit:
                body.Animate(Animations.Attack);
                PlaySound(setFeedback);
                foreach (ParticleSystem item in enemyhitParticle)
                {
                    item.Play();
                }
                break;
            case SetFeedback.EnemyCrit:
                body.Animate(Animations.Attack);
                PlaySound(setFeedback);
                foreach (ParticleSystem item in enemycritParticle)
                {
                    item.Play();
                }
                break;
            case SetFeedback.PlayerHit:
                body.Animate(Animations.Hurt);
                PlaySound(setFeedback);
                foreach (ParticleSystem item in playerhitParticle)
                {
                    item.Play();
                }
                break;
            case SetFeedback.PlayerDodged:
                PlaySound(setFeedback);
                foreach (ParticleSystem item in enemymissedParticle)
                {
                    item.Play();
                }
                break;
            case SetFeedback.PlayerMissed:
                body.Animate(Animations.Hurt);
                PlaySound(setFeedback);
                foreach (ParticleSystem item in playermissedParticle)
                {
                    item.Play();
                }
                break;
            case SetFeedback.PlayerCountered:
                body.Animate(Animations.Attack);
                PlaySound(setFeedback);
                foreach (ParticleSystem item in playercounteredParticle)
                {
                    item.Play();
                }
                break;
            default:
                break;
        }
    }
    void PlaySound(SetFeedback a_sound)
    {
        AudioSource playing = null;
        switch (a_sound)
        {
            case SetFeedback.EnemyHit:
                playing = attack;
                break;
            case SetFeedback.EnemyCrit:
                playing = crit;
                break;
            case SetFeedback.PlayerHit:
                playing = hurt;
                break;
            case SetFeedback.PlayerDodged:
                playing = miss;
                break;
            case SetFeedback.PlayerMissed:
                playing = miss;
                break;
            case SetFeedback.PlayerCountered:
                playing = attack;
                break;
            default:
                break;
        }
        if (playing != null)
        {
            playing.volume = PlayerPrefs.GetFloat("Volume", 0.3f);
            playing.Play();
        }
    }
}