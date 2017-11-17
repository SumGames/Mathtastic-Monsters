using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum modifierType
{
    none,
    monsterHealth,
    monsterAttack,
    AttackTime,
    LessBreaks,
    RemoveLimb,
    counterTime,
    boostAnswer,
    boostGenerated
}

public class EndlessModifierButton : MonoBehaviour
{
    bool used;

    public float modifierChange;

    public modifierType modOne;
    public float modOneIntensity;

    public modifierType modTwo;
    public float modTwoIntensity;


    // Use this for initialization
    void Start ()
    {
        GetComponentInChildren<Text>().text = DisplayText();
	}
	
	// Update is called once per frame
	void Update ()
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
        FindObjectOfType<endlessMonsterManager>().NextLevel(this);
    }
}
