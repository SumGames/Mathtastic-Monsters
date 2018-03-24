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
                a_string += a_type.ToString() + ": Collect more shards per piece";
                break;
            case abilityTypes.Dodge:
                a_string += a_type.ToString() + ": Use during enemy turn to avoid it's attack. (1 charge per 2 pieces)";
                break;
            case abilityTypes.BarkSkin:
                a_string += a_type.ToString() + ": Take less damage per piece equipped.";
                break;
            case abilityTypes.SuperSpeed:
                a_string += a_type.ToString() + ": Gain more time per piece equipped.";
                break;
            case abilityTypes.SlimeSkin:
                a_string += a_type.ToString() + ": Return some damage when you're hit.";
                break;
            case abilityTypes.Freeze:
                a_string += a_type.ToString() + ": Freeze Timer until end of player's phase. Attack cannot crit. (1 charge per 2 pieces)";
                break;
            case abilityTypes.Burn:
                a_string += a_type.ToString() + ": Remove one multiple choice answer. One use per piece.";
                break;
            case abilityTypes.BoulderFist:
                a_string += a_type.ToString() + ": Increase damage, and crits, per piece.";
                break;
            case abilityTypes.FireStorm:
                a_string += a_type.ToString() + ": Damage enemy. Only one use, but intensity scales with pieces equipped.";
                break;
            case abilityTypes.Mastery:
                a_string += a_type.ToString() + ": Gain extra shards for each counter and crit. Increases in strength per 3 pieces.";
                break;
            case abilityTypes.SandSlice:
                a_string += a_type.ToString() + ": Increase counter damage, and enemy timer, per piece.";
                break;
            case abilityTypes.Hourglass:
                a_string += a_type.ToString() + ": Restore your health. Only one use, but intensity scales with pieces equipped.";
                break;
            case abilityTypes.StorePower:
                a_string += a_type.ToString() + ": One use. Damage increases, per piece, as you answer questions. ";
                break;
            case abilityTypes.ArmourUp:
                a_string += a_type.ToString() + ": Gain health per piece.";
                break;
            case abilityTypes.DoubleStrike:
                a_string += a_type.ToString() + ": Gain a second player turn when 5 pieces equipped..";
                break;
            case abilityTypes.TimeLord:
                a_string += a_type.ToString() + ": Time does not move. All attacks crit. Requires entire set.";
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