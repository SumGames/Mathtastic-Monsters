using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public float baseHealth = 6; //Player's starting health. prob affected by equipment.
    float maxHealth;
    float currentHealth; //Health, set by maxHealth and lowered by enemy attacks.

    public int baseAttack = 1; //Amount of damage a player can inflict.
    internal int attack;

    public float baseTimeGiven = 15;

    float Timer; //Time left until attacked.
    public float resetTime; //Time players start with per question.

    internal MonsterManager parent; //Mostly checks and sets game state.

    internal Monster enemy; //so enemy will attack when you lose.

    internal StateManager manager;


    float critMod = 2; //Multiplier of attack damage on crit.
    float critTime = 0.80f; //Percentage of time left needed to crit.


    public Slider healthBar; //Visually represents health.


    public Slider timeLeft; //Visually represents time.

    float greenZone; //At over the crit timer, the bar is green.
    float redZone; //When under 25% left, it turns red.
    //At all other times, turn yellow.
    public Image bar; //Display of the timer bar. Used to change its colour.



    public GameObject container;
    GameObject avatar;

    equipmentList list;

    playerAbilities abilities;

    internal int Frozen;


    combatFeedback feedback;


    //Set Health+time to full.
    public void ResetPlayer()
    {
        if (!list)
        {
            list = FindObjectOfType<equipmentList>();
        }

        if (avatar != null)
            Destroy(avatar);

        avatar = list.buildCharacter(container);


        if (!abilities)
        {
            abilities = GetComponent<playerAbilities>();
            abilities.Begin();
        }
        abilities.setupAbilities();


        Frozen = 0;

        FindObjectOfType<Calculator>().AddInput("Cancel");

        maxHealth = baseHealth + abilities.equipmentHealth();

        attack = baseAttack + abilities.equipmentAttack();

        currentHealth = healthBar.maxValue = maxHealth;

        resetTime = parent.quizRunning.levelTime + baseTimeGiven + abilities.equipmentTime();

        Timer = timeLeft.maxValue = resetTime;
    }

    //Counts down time while game is playing. Tale damage if hits 0.
    void Update()
    {
        if(!list)
        {
            list = FindObjectOfType<equipmentList>();
        }


        healthBar.value = currentHealth;

        if (Frozen > 0)
        {
            bar.color = Color.cyan;
            return;
        }


        if (Timer > greenZone)
            bar.color = Color.green;
        else if (Timer < redZone)
            bar.color = Color.red;
        else
            bar.color = Color.yellow;

        if (manager.getGameState() == playStatus.playing)
        {
            Timer -= Time.deltaTime;
            timeLeft.value = Timer;

            if (Timer < 0)
                enemy.EnemyAttack();
        }
    }

    //Calculate player's damage and return it.
    internal float PlayerAttack()
    {
        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();

        abilities.turnEnded();

        float damage = attack;

        feedback.DamageSet(SetImage.hit);


        if (Frozen > 0)
        {
            Timer = resetTime;
            if (Frozen > 1)
            {
                damage = attack * critMod;
                feedback.DamageSet(SetImage.crit);
            }


            if (Frozen < 3)
                Frozen = 0;

            return damage;
        }
        
        if (Timer > greenZone)
        {
            damage *= critMod;
            feedback.DamageSet(SetImage.crit);
        }

        Timer = resetTime;
        return damage;
    }

    //Reduce the player's health, with a_damage being the enemy's attack.
    public void DamagePlayer(float a_damage)
    {
        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();

        feedback.DamageSet(SetImage.hurt);

        abilities.turnEnded();

        Timer = resetTime;

        currentHealth -= a_damage;

        if (currentHealth <= 0)
        {
            manager.changeState(playStatus.Lost);
        }
    }


    internal int playerCounter()
    {
        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();


        if (Timer > greenZone)
        {
            feedback.DamageSet(SetImage.Counter);
            return attack;
        }
        feedback.DamageSet(SetImage.miss);
        return 0;
    }

    internal void setTime(bool enemyPhase, float enemyTime)
    {
        if (enemyPhase)
        {
            Timer = enemyTime;
            timeLeft.maxValue = enemyTime;
        }
        else
        {
            Timer = resetTime;
            timeLeft.maxValue = resetTime;

        }

        greenZone = Timer * critTime;
        redZone = Timer * .25f;
    }

    //Calculate exp modifier based on health remaining and if the quiz has been completed before.
    internal int CalculateExperience()
    {
        float exp = parent.quizRunning.difficulty;

        if (currentHealth == maxHealth)
        {
            exp *= 2;
        }
        else if (currentHealth >= (maxHealth * 0.75))
        {
            exp *= 1.5f;
        }

        int completed = parent.quizRunning.parent.getCompleted();

        if (parent.quizRunning.quizIndex == completed) //Level was not completed, unlock next.
        {
            parent.quizRunning.parent.incrementCompleted();
        }
        else //Divide experience to a quarter.
        {
            exp *= .25f;
        }

        exp *= abilities.returnExpBoost();

        return (int)exp; //Send back the calculated experience.
    }
}