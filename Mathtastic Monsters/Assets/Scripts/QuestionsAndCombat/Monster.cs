using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    float health; //The current health of the monster, as set by the quizButton.
    float attack;

    public bool enemyPhase;


    internal MonsterManager parent; //Game's manager. Mostly for states, but also check the current active quizButton.

    public Slider healthBar; //visually display health

    internal Player player;


    internal StateManager manager;

    public GameObject monsterSpot;
    GameObject sprite;

    internal questionManager questions;

    public multipleContainer multiple;

    // Use this for initialization
    void Start()
    {
        healthBar = GameObject.Find("EnemyHealth").GetComponent<Slider>();
    }

    //Update healthbar as it changes.
    void Update()
    {
        healthBar.value = health;
    }

    internal void ScratchMonster()
    {
        health -= 1;

        Debug.Log("Scratch");

        CheckDeath();
    }

    //Monster's health is reduced by player's attack, possibly switching to won state.
    internal void MonsterHurt()
    {
        if (!enemyPhase)
            health -= player.PlayerAttack();
        else
            health -= player.playerCounter();

        CheckDeath();

        enemyPhase = !enemyPhase;
        questions.makeQuestion(parent.quizRunning, enemyPhase);
    }

    //Player is hurt, and a new question is built.
    internal void EnemyAttack()
    {
        if (enemyPhase)
            player.DamagePlayer(attack);

        enemyPhase = !enemyPhase;
        questions.makeQuestion(parent.quizRunning,enemyPhase);
    }


    internal void SkipQuestion(bool damage)
    {
        Debug.Log("Skip");

        if(damage)
        {
            Debug.Log("Hurt");
            health -= (player.attack);
            CheckDeath();
        }
        questions.makeQuestion(parent.quizRunning,enemyPhase);
    }

    void CheckDeath()
    {
        if (health < 0)
        {
            manager.changeState(playStatus.Won);
            return;
        }
    }


    //Called only when a quiz begins. Loads a question AND sets health/attack.
    public void loadMonster()
    {
        questions = FindObjectOfType<questionManager>();

        if (sprite != null)
        {
            Destroy(sprite.gameObject);
        }
        if (parent.quizRunning.monsterArt != null)
        {
            sprite = Instantiate(parent.quizRunning.monsterArt, monsterSpot.transform, false);
            sprite.transform.localScale = parent.quizRunning.monsterArt.transform.localScale;
        }
        parent = GameObject.Find("MonsterManager").GetComponent<MonsterManager>();

        enemyPhase = false;

        questions.makeQuestion(parent.quizRunning, enemyPhase);


        health = parent.quizRunning.MonsterHealth;
        attack = parent.quizRunning.MonsterAttack;

        healthBar.maxValue = health;
    }
}