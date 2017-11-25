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
    Monster enemy;


    int oozeLimit = 0; //Will we use the slime ability every three turns or two?

    //How many turns passed since slime ability was used.
    int turns = 0;

	// Use this for initialization
	internal void Begin () 
    {
        player = gameObject.GetComponent<Player>();
        enemy = FindObjectOfType<Monster>();

        abilities = new Dictionary<abilityTypes, int>(4);

        manager = FindObjectOfType<AbilitiesManager>();
        manager.setAbilities();

	}

    //Called every time a monster fight starts.
    internal void setupAbilities(bool a_boss)
    {

        //Reset ability buttons.
        foreach (abilityButton  item in abilityButtons)
        {
            item.resetButton();
            item.setButtonActive();
        }

        abilities.Clear(); //Empty dictionary.

        if (a_boss)
            return;


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

        if (abilities.ContainsKey(abilityTypes.Ooze) && abilities[abilityTypes.Ooze] >= 3)
            oozeLimit = 3;

        if (abilities.ContainsKey(abilityTypes.Hellfire) && abilities[abilityTypes.Hellfire] >= 3)
            oozeLimit = 2;

        //Set the buttons active if they have abiltiies on them. Disables them if they didn't.
        foreach (abilityButton item in abilityButtons)
        {
            item.setButtonActive();
        }

    }

    //Return a float that will multiply with our exp received to boost it.
    internal float returnExpBoost()
    {
        if (!abilities.ContainsKey(abilityTypes.Greedy))
            return 1;

        float returning = 1 + (abilities[abilityTypes.Greedy] * .25f);

        return returning;

    }

    //At turn's end, check if we can use slime ability.
    internal void turnEnded()
    {
        if (oozeLimit == 0) return;
        turns++;
        if(turns>=oozeLimit)
        {
            enemy.ScratchMonster();
            turns -= oozeLimit;
        }
    }

    //If we're wearing four healthy pieces, increase health by 10.
    internal int equipmentHealth()
    {
        int returning = 0;

        if (abilities.ContainsKey(abilityTypes.Angelic) && abilities[abilityTypes.Angelic] == 4)
        {
            returning += 10;
        }

        return returning;
    }
	
    //Boost our speed relative to amount of pieces with speed.
    internal int equipmentTime()
    {
        int returning = 0;

        if (abilities.ContainsKey(abilityTypes.Speed))
        {
            returning = 2 * abilities[abilityTypes.Speed];
        }
        return returning;

    }

    //Increase our attack if Fury power is over 3.
    internal int equipmentAttack()
    {
        if (abilities.ContainsKey(abilityTypes.Fury) && abilities[abilityTypes.Fury] >= 3)
        {
            return 1;
        }
        return 0;
    }


	// Update is called once per frame
	void Update () 
    {
		
	}


    public void useButton(abilityTypes a_type)
    {
        switch (a_type)
        {
            case abilityTypes.Freeze:
                player.Frozen = 1;
                break;
            case abilityTypes.Evaporate:
                enemy.SkipQuestion(false);
                break;
            case abilityTypes.Cryoblast:
                player.Frozen = 2;
                break;
            case abilityTypes.Disintegrate:
                enemy.SkipQuestion(true);
                break;
            default:
                break;
        }

    }
}
