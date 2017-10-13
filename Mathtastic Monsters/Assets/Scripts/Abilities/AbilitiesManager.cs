using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum abilityTypes
{
    None,
    Greedy,
    Speed,
    Freeze,
    Evaporate,
    Ooze,
    Fury,
    Cryoblast,
    Disintegrate,
    Angelic,
    Hellfire,
    TimeLord
}
public class AbilitiesManager : MonoBehaviour
{

    equipmentList list;

    public int[] abilities;

    // Use this for initialization
    void Start()
    {
        list = gameObject.GetComponent<equipmentList>();
        abilities = new int[12];
    }

    //In the customisation scene, tell players what their equipped abilities will do.
    public string displayPower(abilityTypes a_type)
    {
        string a_string = "";

        switch (a_type)
        {
            case abilityTypes.None:
                break;
            case abilityTypes.Greedy:
                a_string += "/1. Passive:Gain 25% more shards per piece.(Max = 4)";
                break;
            case abilityTypes.Speed:
                a_string += "/1. Passive:Gain two more seconds per question per piece(Max = 4)";
                break;
            case abilityTypes.Freeze:
                a_string += "/1. Active: Freezes time until the question is answered.  (Max = 2)";
                break;
            case abilityTypes.Evaporate:
                a_string += "/1. Active: Skip current question.(Max = 2)";
                break;
            case abilityTypes.Ooze:
                a_string += "/3. Passive: Every (3rd) question deal 1 damage.";
                break;
            case abilityTypes.Fury:
                a_string += "/3. Passive: Adds 1 additional damage to each attack.";
                break;
            case abilityTypes.Cryoblast:
                a_string += "/2. Active: Freeze time until question is answered. Deal 2x damage on answer";
                break;
            case abilityTypes.Disintegrate:
                a_string += "/2. Active: Skp current question and inflict normal damage";
                break;
            case abilityTypes.Angelic:
                a_string += "/4. Passive: Gain 10 health";
                break;
            case abilityTypes.Hellfire:
                a_string += "/2. Passive: Every (2nd) question deal 1 damage.";
                break;
            case abilityTypes.TimeLord:
                a_string += "/4. Passive: Time does not move. All attacks crit";
                break;
            default:
                break;
        }
        return a_string;
    }

    //Set up abiltiies for display. 
    //First empty the array, then check every equipped part for an ability.
    //Finally, display every single ability's description along with its charges.
    public string setAbilities()
    {
        Array.Clear(abilities, 0, abilities.Length);

        checkPart(list.currentTorsoPrefab);
        checkPart(list.currentHeadPrefab);

        checkPart(list.currentLeftArmPrefab);
        checkPart(list.currentRightArmPrefab);

        checkPart(list.currentLeftLegPrefab);
        checkPart(list.currentRightLegPrefab);

        string returning = "";


        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i] > 0)
            {
                returning += abilities[i].ToString() + displayPower((abilityTypes)i) + "\n";
            }
        }
        return returning;
    }

    //Check if the part has an ability, and increment it.
    void checkPart(GameObject a_part)
    {
        if (a_part == null)
            return;

        ItemPart part = a_part.GetComponent<ItemPart>();

        abilities[(int)part.ability]++;
    }
}