using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public StoryManager storyManager;
    public ParticleSystem shards;
    internal float baseHealth = 6; //Player's starting health. prob affected by equipment.
    float maxHealth;
    public float currentHealth; //Health, set by maxHealth and lowered by enemy attacks.

    public int baseAttack = 1; //Amount of damage a player can inflict.
    internal float attackDamage;
    internal float counterDamage;

    float Timer; //Time left until attacked.
    public float resetTime; //Time players start with per question.

    internal MonsterManager parent; //Mostly checks and sets game state.

    internal Monster enemy; //so enemy will attack when you lose.

    //internal StateManager manager;


    float critMod = 2; //Multiplier of attack damage on crit.
    float critTime = 0.80f; //Percentage of time left needed to crit.


    //public Slider healthBar; //Visually represents health.

    public Healthbars Healthbar;


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

    public ParentsStateManager manager;

    public AudioSource[] sounds;
    public AudioSource getShards;
    public AudioSource victoryMusic;


    float counterTimeModifier;

    bool bossFighting;


    internal float attacksLanded;


    public Calculator calculator;

    public LevelSelection levelSelection;

    public TransitionManager transition;

    private void Start()
    {
        sounds = GetComponents<AudioSource>();
        getShards = sounds[0];
        victoryMusic = sounds[1];

    }

    //Set Health+time to full.
    public void ResetPlayer(bool a_boss)
    {
        bossFighting = a_boss;

        if (!list)
        {
            list = FindObjectOfType<equipmentList>();
        }

        if (avatar != null)
            Destroy(avatar);

        avatar = list.BuildCharacter(container);


        if (!abilities)
        {
            abilities = GetComponent<playerAbilities>();
            abilities.Begin();
        }

        abilities.SetupAbilities(a_boss);
        if (!a_boss)
            EndTurn(false);

        Frozen = 0;
        if (!calculator)
            calculator = FindObjectOfType<Calculator>();

        calculator.AddInput("Cancel");

        maxHealth = baseHealth * abilities.EquipmentHealth();

        attackDamage = baseAttack * abilities.EquipmentAttack();

        currentHealth = maxHealth;
        Healthbar.setMaxHealth(maxHealth, true);

        resetTime = parent.quizRunning.levelTime + abilities.EquipmentTime();

        counterTimeModifier = abilities.CounterTimeModify();
        counterDamage = baseAttack * abilities.EquipmentCounter();

        FindObjectOfType<TorsoPart>().Animate(Animations.Idle);

        parent.currentEnemy.loadMonster();

    }

    internal void EndTurn(bool a_enemy)
    {
        if (abilities)
        {
            foreach (abilityButton item in abilities.abilityButtons)
            {
                item.DisablePhase(a_enemy);
            }
        }
    }

    //Counts down time while game is playing. Tale damage if hits 0.
    void Update()
    {
        if (!list)
        {
            list = FindObjectOfType<equipmentList>();
        }


        //healthBar.value = currentHealth;

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

        if (manager.isPlaying() && (!transition || transition.transitionState == TransitionState.None) && (!storyManager || storyManager.phase == phases.None))
        {
            Timer -= Time.deltaTime;
            timeLeft.value = Timer;

            if (Timer < 0)
            {
                if (bossFighting)
                {
                    Debug.Log("Boss is Attacking Player");
                    parent.boss.EnemyAttack();
                }
                else
                {
                    enemy.EnemyAttack();
                }
            }
        }
    }

    //Calculate player's damage and return it.
    internal float PlayerAttack()
    {
        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();


        abilities.attacking++;

        float damage = attackDamage;


        if (Frozen > 0)
        {
            if (Frozen > 1)
            {
                damage = attackDamage * critMod;
                feedback.DamageSet(SetFeedback.EnemyCrit);
                abilities.Crits++;
            }
            else
            {
                feedback.DamageSet(SetFeedback.EnemyHit);
            }


            if (Frozen < 3)
                Frozen = 0;

            return damage;
        }

        if (Timer > greenZone)
        {
            damage *= critMod;
            feedback.DamageSet(SetFeedback.EnemyCrit);
            abilities.Crits++;
        }
        else
        {
            feedback.DamageSet(SetFeedback.EnemyHit);
        }

        return damage;
    }

    //Reduce the player's health, with a_damage being the enemy's attack.
    public void DamagePlayer(float a_damage)
    {
        abilities.attacking = 0;

        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();

        feedback.DamageSet(SetFeedback.PlayerHit);


        currentHealth -= a_damage * abilities.ReduceDamage();

        Healthbar.changeHealth(true, currentHealth);

        parent.currentEnemy.abilityDamage(a_damage * abilities.BounceDamage());


        parent.currentEnemy.CheckDeath();
    }

    internal float GetPlayerHealth()
    {
        return currentHealth;
    }


    internal float PlayerCounter()
    {
        if (feedback == null)
            feedback = FindObjectOfType<combatFeedback>();


        if (Timer > greenZone)
        {
            abilities.Counters++;
            feedback.DamageSet(SetFeedback.PlayerCountered);
            return attackDamage;
        }
        feedback.DamageSet(SetFeedback.PlayerDodged);
        return 0;
    }

    internal void SetTime(bool enemyPhase, float enemyTime)
    {
        if (resetTime == 0)
        {
            resetTime = 15;
        }



        if (enemyPhase)
        {
            Timer = enemyTime * counterTimeModifier;
            timeLeft.maxValue = Timer;
            timeLeft.value = Timer;
        }
        else
        {
            Timer = resetTime;
            timeLeft.maxValue = resetTime;
            timeLeft.value = resetTime;
        }


        greenZone = Timer * critTime;
        redZone = Timer * .25f;
    }

    internal float returnTimer()
    {
        return Timer;
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


        if (parent.quizRunning.Hard)
        {
            if (list.equip.StarsAcquired[3] == 3)
            {
                exp *= .25f;
            }            
        }
        else
        {
            int completed = parent.quizRunning.parent.getCompleted();
            if (parent.quizRunning.quizIndex == completed) //Level was not completed, unlock next.
            {
                parent.quizRunning.parent.incrementCompleted();

            }

            else //Divide experience to a quarter.
            {
                exp *= .25f;
            }
        }
        if (levelSelection)
            levelSelection.SetStars((currentHealth == maxHealth), parent.quizRunning);

        exp *= abilities.ReturnExpBoost();

        list.Save();

        if (enemy && enemy.multipleContainer)
            enemy.multipleContainer.DisableThisAndCalculator();


        getShards.volume = PlayerPrefs.GetFloat("Volume", 0.3f);
        victoryMusic.volume = PlayerPrefs.GetFloat("Volume", 0.3f);
        shards.Play(true);
        getShards.Play();
        victoryMusic.Play();

        return (int)exp; //Send back the calculated experience.        
    }

    internal void healPlayer(int pieces)
    {
        float healing = maxHealth / 10;
        healing *= pieces;

        currentHealth += healing;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

    }
}