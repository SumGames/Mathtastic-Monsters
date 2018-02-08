using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalismanManager : MonoBehaviour
{

    public Sprite AddHalf;
    public Sprite AddFull;

    public Sprite SubHalf;
    public Sprite SubFull;

    public Sprite MultHalf;
    public Sprite MultFull;

    public Sprite DivHalf;
    public Sprite DivFull;

    public Sprite FortHalf;
    public Sprite FortFull;

    public Sprite defaultSprite;

    equipmentList list;


    public Talisman[] talismans;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void SetStaticTalismans()
    {
        if(!list)
        {
            list = FindObjectOfType<equipmentList>();
        }

        foreach (Talisman tal in talismans)
        {
            tal.SetStaticTalisman(list, this);
        }
    }


    internal Sprite ReturnSubject(classType subject, bool half)
    {
        Sprite returnSprite = null;

        switch (subject)
        {
            case classType.Addition:
                if (half)
                    returnSprite = AddHalf;
                else
                    returnSprite = AddFull;
                break;
            case classType.Subtraction:
                if (half)
                    returnSprite = SubHalf;
                else
                    returnSprite = SubFull;
                break;
            case classType.Multiplication:
                if (half)
                    returnSprite = MultHalf;
                else
                    returnSprite = MultFull;
                break;
            case classType.Division:
                if (half)
                    returnSprite = DivHalf;
                else
                    returnSprite = DivFull;
                break;
            case classType.Calculi:
                if (half)
                    returnSprite = FortHalf;
                else
                    returnSprite = FortFull;
                break;
            default:
                break;
        }
        if (returnSprite == null)
        {
            return defaultSprite;
        }
        else
            return returnSprite;
    }
}