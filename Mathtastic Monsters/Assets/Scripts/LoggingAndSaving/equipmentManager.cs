using UnityEngine;
using System;

[Serializable]
public class equipmentManager
{
    [SerializeField]
    public int[] completedLevels;


    [SerializeField]
    public int[] equippedParts;


    [SerializeField]
    public bool[] torsoAvailability;

    [SerializeField]
    public bool[] headAvailability;

    [SerializeField]
    public bool[] leftArmAvailability;

    [SerializeField]
    public bool[] rightArmAvailability;

    [SerializeField]
    public bool[] leftLegAvailability;

    [SerializeField]
    public bool[] rightLegAvailability;


    [SerializeField]
    public int shards;

    public bool tutorialComplete;


    public int[] StarsAcquired;


    public void setEquipped(partType a_type, int index)
    {
        equippedParts[(int)a_type] = index;
    }


    public void setCompletedLevels(int completed, int type)
    {
        completedLevels[type] = completed;
    }

    public int getCompletedLevels(int type)
    {
        return completedLevels[type];
    }

    public int GetTotalStars()
    {
        int star = 0;

        foreach (int item in StarsAcquired)
        {
            star += item;
        }


        return star;
    }
}