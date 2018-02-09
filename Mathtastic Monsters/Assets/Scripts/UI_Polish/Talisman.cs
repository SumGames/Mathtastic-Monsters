using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talisman : MonoBehaviour
{
    Image MainButton;
    Image HalfButton;

    public classType myType;

    TalismanManager talismanManager;


    internal void SetChangingTalisman(equipmentList list, TalismanManager talismanManager, classType a_type)
    {
        if (!MainButton)
        {
            MainButton = GetComponent<Image>();
            HalfButton = GetComponentsInChildren<Image>()[1];
        }

        myType = a_type;

        if (myType == classType.Calculi)
        {
            gameObject.SetActive(false);
            return;
        }

        MainButton.sprite = talismanManager.ReturnSubject(myType, false);
        HalfButton.sprite = talismanManager.ReturnSubject(myType, true);

        int index = list.equip.completedLevels[(int)myType];
        SetActivity(index);
    }

    internal void SetStaticTalisman(equipmentList list, TalismanManager talismanManager)
    {
        if (!MainButton)
        {
            MainButton = GetComponent<Image>();
            HalfButton = GetComponentsInChildren<Image>()[1];
        }

        if (MainButton.sprite == null)
            MainButton.sprite = talismanManager.ReturnSubject(myType, false);
        if (MainButton.sprite == null)
            MainButton.sprite = talismanManager.ReturnSubject(myType, true);

        int index = list.equip.completedLevels[(int)myType];

        SetActivity(index);

    }

    public void SetActivity(int index)
    {
        int progress = index;
        if (progress > 9)
        {
            MainButton.color = new Color(1, 1, 1, 1);
            HalfButton.gameObject.SetActive(false);
        }
        else if (progress > 4)
        {
            HalfButton.color = new Color(1, 1, 1, 1);
            HalfButton.gameObject.SetActive(true);
            MainButton.color = new Color(.2f, .2f, .2f, 1);
        }
        else
        {
            HalfButton.gameObject.SetActive(false);
            MainButton.color = new Color(.2f, .2f, .2f, 1);
        }
    }
}