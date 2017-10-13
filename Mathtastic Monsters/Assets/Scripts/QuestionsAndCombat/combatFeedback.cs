using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SetImage
{
    hit,
    crit,
    hurt,
    miss,
    Counter
}

public class combatFeedback : MonoBehaviour
{
    public Sprite attackHit;
    public Sprite attackCrit;
    public Sprite dodged;
    public Sprite Countered;

    public Sprite playerHurt;

    public Image PlayerImage;
    public Image EnemyImage;

    public float resetTimer = 3;

    float timer;

    // Use this for initialization
    void Start()
    {
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
        timer = 3;

        switch (setImage)
        {
            case SetImage.hit:
                EnemyImage.sprite = attackHit;
                EnemyImage.enabled = true;
                break;
            case SetImage.crit:
                EnemyImage.sprite = attackCrit;
                EnemyImage.enabled = true;
                break;
            case SetImage.hurt:
                PlayerImage.sprite = playerHurt;
                PlayerImage.enabled = true;
                break;
            case SetImage.miss:
                PlayerImage.sprite = dodged;
                PlayerImage.enabled = true;
                break;
            case SetImage.Counter:
                EnemyImage.sprite = Countered;
                PlayerImage.enabled = true;
                break;
            default:
                break;
        }
    }
}