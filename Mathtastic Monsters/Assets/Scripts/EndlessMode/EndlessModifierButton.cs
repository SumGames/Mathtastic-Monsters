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

    public float modifierChange;

    public modifierType modOne;
    public float modOneIntensity;

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

    public void buttonUsed()
    {
        endlessMonster.NextLevel(this);
    }

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