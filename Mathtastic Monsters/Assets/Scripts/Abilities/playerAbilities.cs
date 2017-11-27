using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class playerAbilities : MonoBehaviour 
{
    public abilityButton[] abilityButtons;


    AbilitiesManager manager;

    Dictionary<abilityTypes, int> abilities; //A list of abilities with charges >1, and their charge count.

    Player player;
    public Monster enemy;

    public int Crits;
    public int Counters;

    public int attacking;


	// Use this for initialization
	internal void Begin () 
    {
        player = gameObject.GetComponent<Player>();

        enemy = player.enemy;

        abilities = new Dictionary<abilityTypes, int>(4);

        manager = FindObjectOfType<AbilitiesManager>();
        manager.setAbilities();

	}

    //Called every time a monster fight starts.
    internal void setupAbilities(bool a_boss)
    {

        Counters = 0;
        Crits = 0;

        //Reset ability buttons.
        foreach (abilityButton  item in abilityButtons)
        {
            item.resetButton();
        }

        abilities.Clear(); //Empty dictionary.

        if (a_boss)
        {
            Debug.Log("Stopped for boss");
            return;
        }
        for (int i = 1, j = 0; i < manager.abilities.Length && j < abilityButtons.Length; i++)
        {
            //If an ability's charges is over 0, add it to a button and set up that button.
            if (manager.abilities[i] > 0)
            {
                abilities.Add((abilityTypes)i, manager.abilities[i]);
                abilityButtons[j].setUpButton((abilityTypes)i, manager.abilities[i], this);
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
            item.setButtonActive();
        }

    }

    //Return a float that will multiply with our exp received to boost it.
    internal float returnExpBoost()
    {
        float returning = 1;

        if (abilities.ContainsKey(abilityTypes.Mastery) && abilities[abilityTypes.Mastery] >= 3)
        {
            int mastery = abilities[abilityTypes.Mastery] % 3;

            returning += (Crits * (.10f * mastery));

            returning += (Counters * (.10f * mastery));

            Counters = 0;
            Crits = 0;
        }

        if (abilities.ContainsKey(abilityTypes.Scavenger))
        {
            returning += (abilities[abilityTypes.Scavenger] * .20f);
        }
       
        return returning;
    }


    //If we're wearing four healthy pieces, increase health by 10.
    internal float equipmentHealth()
    {
        float returning = 1;

        if (abilities.ContainsKey(abilityTypes.ArmourUp))
        {
            returning += (abilities[abilityTypes.ArmourUp] * .10f);
        }

        return returning;
    }
	
    //Boost our speed relative to amount of pieces with speed.
    internal float equipmentTime()
    {
        int returning = 0;

        if (abilities.ContainsKey(abilityTypes.SuperSpeed))
        {
            returning = 2 * abilities[abilityTypes.SuperSpeed];
        }
        return returning;
    }

    //Increase our attack if Fury power is over 3.
    internal float equipmentAttack()
    {
        float returning = 1;

        if (abilities.ContainsKey(abilityTypes.BoulderFist))
        {
            returning += (.05f * abilities[abilityTypes.BoulderFist]);

        }
        return returning;
    }

    internal float equipmentCounter()
    {
        float returning = 1;

        if (abilities.ContainsKey(abilityTypes.SandSlice))
        {
            returning += (0.2f * abilities[abilityTypes.SandSlice]);
        }


        return returning;
    }

    internal float counterTimeModify()
    {
        float returning = 1;

        if (abilities.ContainsKey(abilityTypes.SandSlice))
        {
            returning += (0.05f * abilities[abilityTypes.SandSlice]);

        }


        return returning;
    }

    internal float reduceDamage()
    {
        float returning = 1;

        if (abilities.ContainsKey(abilityTypes.BarkSkin))
        {
            returning -= (0.05f * abilities[abilityTypes.BarkSkin]);
        }

        return returning;
    }


    internal float bounceDamage()
    {
        float returning = 0;

        if (abilities.ContainsKey(abilityTypes.SlimeSkin))
        {
            returning += (0.1f * abilities[abilityTypes.SlimeSkin]);
        }

        return returning;
    }


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
                enemy.abilityDamage(abilities[abilityTypes.StorePower] * 0.05f * player.attackDamage);

                break;
            case abilityTypes.FireStorm:
                enemy.abilityDamage((player.attackDamage * (0.35f * abilities[abilityTypes.FireStorm])));
                break;
            case abilityTypes.Burn:
                FindObjectOfType<multipleContainer>().removeSingle();
                break;
            default:
                break;
        }

    }
}
