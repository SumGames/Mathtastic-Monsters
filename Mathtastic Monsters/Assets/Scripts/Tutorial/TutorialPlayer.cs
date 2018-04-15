using UnityEngine;
using UnityEngine.UI;

public class TutorialPlayer : MonoBehaviour
{

    public float baseHealth = 6; //Player's starting health. prob affected by equipment.
    float currentHealth; //Health, set by maxHealth and lowered by enemy attacks.

    public int attack = 1; //Amount of damage a player can inflict.


    float Timer; //Time left until attacked.
    public float resetTime = 15;

    public TutorialMonster enemy; //so enemy will attack when you lose.



    public Slider healthBar; //Visually represents health.


    public Slider timeLeft; //Visually represents time.

    internal float greenZone; //At over the crit timer, the bar is green.
    float redZone; //When under 25% left, it turns red.
    //At all other times, turn yellow.
    public Image bar; //Display of the timer bar. Used to change its colour.



    public GameObject container;
    GameObject avatar;

    equipmentList list;

    public int Frozen;


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

        if (list)
            avatar = list.BuildCharacter(container);


        currentHealth = healthBar.maxValue = baseHealth;

        resetTime = 40;

        Timer = timeLeft.maxValue = resetTime;

        greenZone = resetTime * .80f;
        redZone = resetTime * .25f;

        FindObjectOfType<TorsoPart>().Animate(Animations.Idle);
    }

    //Counts down time while game is playing. Tale damage if hits 0.
    void Update()
    {
        if (!list)
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

        Timer -= Time.deltaTime;
        timeLeft.value = Timer;

        if (Timer < 0)
            enemy.EnemyAttack();
    }

    //Calculate player's damage and return it.
    internal float PlayerAttack()
    {
        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();        

        float damage = attack;


        if (Timer > greenZone)
        {
            damage *= 2;
            feedback.DamageSet(SetFeedback.EnemyCrit);
        }
        else
        {
            feedback.DamageSet(SetFeedback.EnemyHit);
        }
        Timer = resetTime;
        return damage;
    }

    internal void SetTime(bool enemyPhase, float enemyTime)
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

        greenZone = Timer * .8f;
        redZone = Timer * .25f;
    }


    //Reduce the player's health, with a_damage being the enemy's attack.
    public void DamagePlayer(float a_damage)
    {

        Timer = resetTime;

        currentHealth -= a_damage;


        /* Heal player*/

    }
}