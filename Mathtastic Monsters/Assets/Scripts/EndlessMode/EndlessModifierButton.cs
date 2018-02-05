using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum modifierType
{
    none,
    monsterHealth, //in
    monsterAttack, //in 
    YourAttackTime, //in
    MonsterAttackTime, //in
    boostAnswer,
    numberofCounterAnswers, //in
    difficultyJump, //in
    LessBreaks,
    RemoveLimb //ininin
}


public class EndlessModifierButton : MonoBehaviour
{
    bool used;

    //The modifier points we'll get in return for using this.
    public float modifierChange;


    //The type of stat we;re changing, and the amount.
    public modifierType modOne;
    public int modOneIntensity;

    //Some buttons might change two.
    public modifierType modTwo;
    public int modTwoIntensity;

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
        nameplate += NamePlate();
        return nameplate;
    }

    string NamePlate()
    {
        string returning = "";

        if (modOne != modifierType.RemoveLimb)
        {
            returning += "\n" + modOne.ToString() + " " + modOneIntensity.ToString();
        }
        else
            returning += "\n" + gameObject.name;
        if (modTwo != modifierType.none)
        {
            returning += "\n" + modTwo.ToString() + " " + modTwoIntensity.ToString();
        }
        return returning;
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