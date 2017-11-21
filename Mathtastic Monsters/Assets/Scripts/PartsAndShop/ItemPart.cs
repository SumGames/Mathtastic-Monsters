using UnityEngine;


//Class is set as a public variable on items, as it'll tell players what they need to win to unlock them.
public enum classType
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Calculi,
    None,
}

public class ItemPart : MonoBehaviour
{
    public Vector3 Scale = new Vector3(1, 1, 1);


    //Can we equip this item?
    public bool owned;

    //How many shards will it cost.
    public int cost;


    public classType typeRequired;
    public int levelsNeeded;

    public abilityTypes ability;

    //Returns true if number of levels completed in a subject is greater than required.
    public bool checkAvailable(equipmentManager numbered)
    {
        if (typeRequired == classType.None)
        {
            return true;
        }

        int completed = numbered.completedLevels[(int)typeRequired];
        if (completed >= levelsNeeded)
        {
            return true;
        }
        return false;
    }

    //Displays subject name and number needed.
    public string setAvailableText()
    {
        return "Beat " + typeRequired.ToString() + " Level " + levelsNeeded.ToString();
    }
}
