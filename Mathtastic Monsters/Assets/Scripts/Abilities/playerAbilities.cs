using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerAbilities : MonoBehaviour
{
    public abilityButton[] abilityButtons;


    AbilitiesManager manager;

    Dictionary<abilityTypes, int> abilities; //A list of abilities with charges >1, and their charge count.

    Player player;
    public Monster enemy;

    public int Crits; //Number of crits we got since starting fight.
    public int Counters; //Number of counters since star.

    public int attacking; //Number of attacks since in a row since last taken damage. Used in store power.

    public Sprite[] Icons;


    //initialization, called manually by player when it's ready.
    internal void Begin()
    {
        player = gameObject.GetComponent<Player>();

        enemy = player.enemy;

        abilities = new Dictionary<abilityTypes, int>(4);

        manager = FindObjectOfType<AbilitiesManager>();
        manager.setAbilities();
    }

    internal void AbleAbilities(bool Able)
    {
        for (int i = 0; i < abilityButtons.Length; i++)
        {
            if (abilityButtons[i].thisButton != abilityTypes.None)
                abilityButtons[i].gameObject.SetActive(Able);
        }
    }


    //Called every time a monster fight starts. If fight is a boss, everything is turned off.
    internal void SetupAbilities(bool a_boss)
    {
        attacking = 0;

        Counters = 0;
        Crits = 0;

        //Reset ability buttons.
        foreach (abilityButton item in abilityButtons)
        {
            item.ResetButton();
        }

        abilities.Clear(); //Empty dictionary.

        if (a_boss) //Abilities are left disabled during a boss fight.
        {
            return;
        }
        for (int i = 1, j = 0; i < manager.abilities.Length && j < abilityButtons.Length; i++)
        {
            //If an ability's charges is over 0, add it to a button and set up that button.
            if (manager.abilities[i] > 0)
            {
                abilities.Add((abilityTypes)i, manager.abilities[i]);
                abilityButtons[j].SetUpButton((abilityTypes)i, manager.abilities[i], this);
                j++;
            }
        }


        //Turn on permanent frozen if timelord set worn.
        if (abilities.ContainsKey(abilityTypes.TimeLord) && abilities[abilityTypes.TimeLord] == 4)
        {
            player.Frozen = 3;
        }


        //Set the buttons active if they have abiltiies on them. Disables them if they didn't.
        foreach (abilityButton item in abilityButtons)
        {
            item.SetButtonActive();
        }

    }

    //Return a float that will multiply with our exp received to boost it.
    internal float ReturnExpBoost()
    {
        float returning = 1;

        if (abilities.ContainsKey(abilityTypes.Mastery))
        {
            float mastery = abilities[abilityTypes.Mastery] * 0.02f;

            returning += (Crits * mastery);

            returning += (Counters * mastery);
        }

        if (abilities.ContainsKey(abilityTypes.Scavenger)) //If we have at least 1 scavenger part.
        {
            returning += (abilities[abilityTypes.Scavenger] * .20f);
        }

        return returning;
    }


    //If we're wearing healthy pieces, we get some health per part.
    internal float EquipmentHealth()
    {
        float returning = 1;

        if (abilities.ContainsKey(abilityTypes.ArmourUp))
        {
            returning += (abilities[abilityTypes.ArmourUp] * .10f);
        }

        return returning;
    }

    //Boost our speed relative to amount of pieces with speed.
    internal float EquipmentTime()
    {
        int returning = 0;

        if (abilities.ContainsKey(abilityTypes.SuperSpeed))
        {
            returning = 2 * abilities[abilityTypes.SuperSpeed];
        }
        return returning;
    }

    //Increase our attack if Fury power is over 3.
    internal float EquipmentAttack()
    {
        float returning = 1;

        if (abilities.ContainsKey(abilityTypes.BoulderFist))
        {
            returning += (.05f * abilities[abilityTypes.BoulderFist]);

        }
        return returning;
    }
    //Boost counter damage if we have sand parts.
    internal float EquipmentCounter()
    {
        float returning = 1;

        if (abilities.ContainsKey(abilityTypes.SandSlice))
        {
            returning += (0.2f * abilities[abilityTypes.SandSlice]);
        }


        return returning;
    }

    //If we have sandslice parts, we'll gain more counter time.
    internal float CounterTimeModify()
    {
        float returning = 1;

        if (abilities.ContainsKey(abilityTypes.SandSlice))
        {
            returning += (0.05f * abilities[abilityTypes.SandSlice]);

        }


        return returning;
    }

    //If we have barkskin parts, we'll take less damage.
    internal float ReduceDamage()
    {
        float returning = 1;

        if (abilities.ContainsKey(abilityTypes.BarkSkin))
        {
            returning -= (0.05f * abilities[abilityTypes.BarkSkin]);
        }

        return returning;
    }

    //If we have Slime parts and take damage, the enemy will take damage themselves.
    internal float BounceDamage()
    {
        float returning = 0;

        if (abilities.ContainsKey(abilityTypes.SlimeSkin))
        {
            returning += (0.1f * abilities[abilityTypes.SlimeSkin]);
        }

        return returning;
    }

    //Return true if we had Five of this piece equipped.
    internal bool DoubleStrike()
    {
        return (abilities.ContainsKey(abilityTypes.DoubleStrike) && abilities[abilityTypes.DoubleStrike] >= 5);
    }


    //For active buttons, called when we clicked on them in scene.
    public void useButton(abilityTypes a_type)
    {
        switch (a_type)
        {
            case abilityTypes.Freeze:
                player.Frozen = 1;
                break;
            case abilityTypes.Dodge:
                enemy.SkipQuestion();
                break;

            case abilityTypes.StorePower:
                enemy.abilityDamage(abilities[abilityTypes.StorePower] * 0.05f * player.attackDamage * attacking);

                break;
            case abilityTypes.FireStorm:
                float damage = (player.attackDamage * (0.35f * abilities[abilityTypes.FireStorm]));
                enemy.abilityDamage(damage);
                Debug.Log("Fireblast  " + damage);
                break;
            case abilityTypes.Burn:
                FindObjectOfType<multipleContainer>().RemoveSingle();
                break;

            case abilityTypes.Hourglass:
                player.healPlayer(abilities[abilityTypes.Hourglass]);
                break;
            default:
                break;
        }

    }
}