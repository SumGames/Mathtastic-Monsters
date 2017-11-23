using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SetImage
{
    hit,
    crit,
    hurt,
    YouDodgeD, //When attack misses enemy.
    YouNissed,
    Counter
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

    // Use this for initialization
    void Start()
    {
        sounds = GetComponents<AudioSource>();
        attack = sounds[0];
        hurt = sounds[1];
        miss = sounds[2];
        crit = sounds[3];
        

        PlayerImage.enabled = false;
        EnemyImage.enabled = false;

    }

    // Update is called once per frame
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

        body.Animate(Animations.Idle);

        switch (setImage)
        {
            case SetImage.hit:
                EnemyImage.sprite = attackHit;
                EnemyImage.enabled = true;
                PlayerImage.enabled = false;
                body.Animate(Animations.Attack);
                attack.Play();
                break;
            case SetImage.crit:
                EnemyImage.sprite = attackCrit;
                EnemyImage.enabled = true;
                PlayerImage.enabled = false;
                body.Animate(Animations.Attack);
                crit.Play();
                break;
            case SetImage.hurt:
                PlayerImage.sprite = playerHurt;
                PlayerImage.enabled = true;
                EnemyImage.enabled = false;
                body.Animate(Animations.Hurt);
                hurt.Play();
                break;
            case SetImage.YouDodgeD:
                PlayerImage.sprite = dodged;
                PlayerImage.enabled = true;
                EnemyImage.enabled = false;                
                miss.Play();
                break;
            case SetImage.YouNissed:
                EnemyImage.sprite = Missed;
                EnemyImage.enabled = true;
                PlayerImage.enabled = false;
                body.Animate(Animations.Hurt);
                miss.Play();
                break;
            case SetImage.Counter:
                body.Animate(Animations.Attack);
                EnemyImage.sprite = Countered;
                PlayerImage.enabled = false;
                EnemyImage.enabled = true;
                attack.Play();
                break;
            default:
                break;
        }
    }
}