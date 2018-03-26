using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum abilityTypes
{
    None,
    Scavenger,
    Dodge,
    BarkSkin,
    SuperSpeed,
    SlimeSkin,
    Freeze,
    Burn,
    BoulderFist,
    FireStorm,
    Mastery,
    SandSlice,
    Hourglass,
    StorePower,
    ArmourUp,
    DoubleStrike,
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
        abilities = new int[(int)abilityTypes.TimeLord];
    }

    //In the customisation scene, tell players what their equipped abilities will do.
    public string displayPower(abilityTypes a_type)
    {
        string a_string = "";

        switch (a_type)
        {
            case abilityTypes.None:
                break;
            case abilityTypes.Scavenger:
                a_string += a_type.ToString() + ": Collect more shards per monster part!";
                break;
            case abilityTypes.Dodge:
                a_string += a_type.ToString() + ": Use when defending to dodge an enemy attack, more parts? More chances to dodge!";
                break;
            case abilityTypes.BarkSkin:
                a_string += a_type.ToString() + ": Take less damage per monster part!";
                break;
            case abilityTypes.SuperSpeed:
                a_string += a_type.ToString() + ": Get more time to answer questions with each monster part!";
                break;
            case abilityTypes.SlimeSkin:
                a_string += a_type.ToString() + ": Return damage when you're hit, more parts? More damage!";
                break;
            case abilityTypes.Freeze:
                a_string += a_type.ToString() + ": Freeze the timer bar during the attack phase! More parts? More freeze!";
                break;
            case abilityTypes.Burn:
                a_string += a_type.ToString() + ": Remove one multiple choice answer, one use per monster part!";
                break;
            case abilityTypes.BoulderFist:
                a_string += a_type.ToString() + ": Your attack and critical attacks are stronger per monster part!";
                break;
            case abilityTypes.FireStorm:
                a_string += a_type.ToString() + ": Hurl a large firestorm at the enemy monster for big damage! More parts? More damage!";
                break;
            case abilityTypes.Mastery:
                a_string += a_type.ToString() + ": Get bonus shards when you do a critical attack or a counterattack! More parts? More shards!";
                break;
            case abilityTypes.SandSlice:
                a_string += a_type.ToString() + ": Your countertattack is stronger per monster part!.";
                break;
            case abilityTypes.Hourglass:
                a_string += a_type.ToString() + ": Heal yourself right away, more parts? More health!";
                break;
            case abilityTypes.StorePower:
                a_string += a_type.ToString() + ": Store power away with each question you get right then unleash it at the enemy for massive damage! ";
                break;
            case abilityTypes.ArmourUp:
                a_string += a_type.ToString() + ": Start with more health, more parts? More health!";
                break;
            case abilityTypes.DoubleStrike:
                a_string += a_type.ToString() + ": Get a chance to have a second attack! With more parts your chance is greater!";
                break;
            case abilityTypes.TimeLord:
                a_string += a_type.ToString() + ": The Ruler of Math! Time stands still and you are all powerful!";
                break;
            default:
                break;
        }
        return a_string;
    }

    //Set up abiltiies for display. 
    //First empty the array, then check every equipped part for an ability.
    //Finally, display every single ability's description along with its charges.
    public void setAbilities(equipmentList a_list = null)
    {
        if (!list && a_list)
        {
            Start();
        }

        Array.Clear(abilities, 0, abilities.Length);

        int[] parts = new int[6];

        CheckPart(list.currentTorsoPrefab);
        CheckPart(list.currentHeadPrefab);

        CheckPart(list.currentLeftArmPrefab);
        CheckPart(list.currentRightArmPrefab);

        CheckPart(list.currentLeftLegPrefab);
        CheckPart(list.currentRightLegPrefab);


    }

    //Check if the part has an ability, and increment it.
    void CheckPart(GameObject a_part)
    {
        if (a_part == null)
            return;

        ItemPart part = a_part.GetComponent<ItemPart>();

        abilities[(int)part.ability]++;

        return;
    }
}