using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum modifierType
{
    none,
    monsterHealth,
    monsterAttack,
    YourAttackTime,
    MonsterAttackTime,
    boostAnswer,
    numberofCounterAnswers,
    difficultyJump,

    LessBreaks,
    RemoveLimb
}


public class EndlessModifierButton : MonoBehaviour
{
    bool used;

    //The modifier points we'll get in return for using this.
    public float modifierChange;


    //The type of stat we;re changing, and the amount.
    public modifierType modOne;
    public float modOneIntensity;

    //Some buttons might change two.
    public modifierType modTwo;
    public float modTwoIntensity;

    public bool locked = false;

    internal endlessMonsterManager endlessMonster;


    // Use this for initialization
    void Start()
    {
        endlessMonster = FindObjectOfType<endlessMonsterManager>();

        GetComponentInChildren<Text>().text = DisplayText();

    }

    // Update is called once per frame
    void Update()
    {

    }


    //Display enum name, and the amount.
    string DisplayText()
    {
        string nameplate = "Mod += " + modifierChange.ToString();
        nameplate += "\n" + modOne.ToString() + " " + modOneIntensity.ToString();

        if (modTwo != modifierType.none)
        {
            nameplate += "\n" + modTwo.ToString() + " " + modTwoIntensity.ToString();

        }

        return nameplate;
    }

    //We clicked on this button.
    public void buttonUsed()
    {
        endlessMonster.NextLevel(this);
    }

    //Can't have duplicate buttons, can't have negative modifier if our modifier is 0.
    internal bool checkIfLocked(endlessMonsterManager manager)
    {

        if (locked)
            return true;
        if (modifierChange < 0 && ((modifierChange * -1) > manager.Modifier))
        {
            locked = true;
            return true;
        }
        return false;
    }
}