﻿using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{

    public float health; //The current health of the monster, as set by the quizButton.
    internal float attack;

    public bool enemyPhase;


    public MonsterManager parent; //Game's manager. Mostly for states, but also check the current active quizButton.

    public Healthbars bar;

    internal Player player;


    internal ParentsStateManager manager;

    public GameObject monsterSpot;
    internal GameObject sprite;

    public questionManager questions;

    public multipleContainer multiple;


    internal Animator animator;

    internal MusicManager music;

    internal multipleContainer multipleContainer;


    // Use this for initialization
    void Start()
    {
        bar = FindObjectOfType<Healthbars>();
    }

    //Update healthbar as it changes.
    void Update()
    {
    }

    internal void abilityDamage(float damage)
    {

        health -= damage;
        CheckDeath();
    }

    internal void ScratchMonster()
    {
        health -= 1;
        CheckDeath();
    }

    //Monster's health is reduced by player's attack, possibly switching to won state.
    internal virtual void MonsterHurt()
    {        
        if (!enemyPhase)
        {
            health -= player.PlayerAttack();
        }
        else
            health -= player.PlayerCounter();

        bar.changeHealth(false, health);

        CheckDeath();

        enemyPhase = questions.MakeQuestion(parent.quizRunning);
    }

    //Player is hurt, and a new question is built.
    internal virtual void EnemyAttack()
    {

        if (enemyPhase)
        {
            player.DamagePlayer(attack);
            if (animator)
                animator.Play("Attack");
        }
        else
        {
            FindObjectOfType<combatFeedback>().DamageSet(SetFeedback.PlayerMissed);
        }
        enemyPhase = !enemyPhase;
        enemyPhase = questions.MakeQuestion(parent.quizRunning);
    }


    internal void SkipQuestion()
    {
        enemyPhase = !enemyPhase;
        enemyPhase = questions.MakeQuestion(parent.quizRunning);
    }

    public virtual void CheckDeath()
    {
        manager = FindObjectOfType<ParentsStateManager>();


        if (animator)
            animator.Play("Hurt");

        if (health <= 0)
        {
            manager.changeState(playStatus.Won);
            return;
        }

        if (player.GetPlayerHealth() <= 0)
        {
            manager.changeState(playStatus.Lost);
        }
    }

    internal void DestroyMonster()
    {
        if (sprite != null)
        {
            Destroy(sprite.gameObject);
        }

    }

    //Called only when a quiz begins. Loads a question AND sets health/attack.
    public virtual void loadMonster()
    {
        if (!multipleContainer)
            multipleContainer = FindObjectOfType<multipleContainer>();

        multipleContainer.SetAttacks(false, true);

        questions = FindObjectOfType<questionManager>();

        DestroyMonster();

        if (parent.quizRunning.monsterArt != null)
        {
            sprite = Instantiate(parent.quizRunning.monsterArt, monsterSpot.transform, false);
            sprite.transform.localScale = parent.quizRunning.monsterArt.transform.localScale;
            animator = sprite.GetComponent<Animator>();
        }
        if (parent == null)
            parent = GameObject.Find("MonsterManager").GetComponent<MonsterManager>();

        enemyPhase = false;

        enemyPhase = questions.MakeQuestion(parent.quizRunning);

        health = parent.quizRunning.MonsterHealth;
        attack = parent.quizRunning.MonsterAttack;

        multiple.DisableMultiple();


        bar.setMaxHealth(health, false);

        if (!music)
            music = FindObjectOfType<MusicManager>();

        if (music)
            music.SetCombatMusic(parent.quizRunning.Operator, parent.quizRunning.boss);

    }
}