using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class abilityButton : MonoBehaviour 
{
    abilityTypes thisButton; //This button's current ability.

    //Active buttons are disabled if charges left is less than needed.
    int chargesLeft; 
    int chargesNeeded;

    Button m_button;

    Text m_text; //On button, Displays ability name and uses left.

    playerAbilities abilities; //reference to player's ability manager.


    //Begins setting up button.

    //buttontype is the avility this button will be responsible for.
    //Charges if how many uses it has.
    //a_abilities is a reference to the ability manager.
    internal void setUpButton(abilityTypes buttonType,int charges,playerAbilities a_abilities)
    {
        abilities = a_abilities;

        thisButton = buttonType;
        chargesLeft = charges;

        setButtonActive();
    }

    //If type is set to none, everything will be considered default.
    internal void resetButton()
    {
        thisButton = abilityTypes.None;
    }

    //Command the ability manager to use the ability, remove the charges, and check if the ability is still usable.
    public void useButton()
    {
        abilities.useButton(thisButton);
        chargesLeft -= chargesNeeded;
        setButtonActive();
    }

    //Set info using the ability's type.
    //This determines if the ability is usable, it's cost, it's name, etc.
    internal void setButtonActive()
    {
        if (!m_button)
        {
            m_button = gameObject.GetComponent<Button>();
            m_text = GetComponentInChildren<Text>();
        }

        m_text.text = thisButton.ToString();

        chargesNeeded = 0;
        m_button.interactable = true;
        gameObject.SetActive(true);

        switch (thisButton)
        {
            case abilityTypes.None:
                gameObject.SetActive(false);
                return;
            case abilityTypes.Greedy:
                m_button.interactable = false;
                return;
            case abilityTypes.Speed:
                m_button.interactable = false;
                return;
            case abilityTypes.Freeze:
                if (chargesLeft > 2)
                    chargesLeft = 2;
                chargesNeeded = 1;
                break;
            case abilityTypes.Evaporate:
                if (chargesLeft > 3)
                    chargesLeft = 3;
                chargesNeeded = 1;
                break;
            case abilityTypes.Ooze:
                m_button.interactable = false;
                return;
            case abilityTypes.Fury:
                m_button.interactable = false;
                return;
            case abilityTypes.Cryoblast:
                chargesNeeded = 2;
                break;
            case abilityTypes.Disintegrate:
                chargesNeeded = 2;
                break;
            case abilityTypes.Angelic:
                m_button.interactable = false;
                return;
            case abilityTypes.Hellfire:
                m_button.interactable = false;
                return;
            case abilityTypes.TimeLord:
                m_button.interactable = false;
                return;
            default:
                break;
        }

        //If not enough charges left, disable it.
        if (chargesLeft < chargesNeeded)
            m_button.interactable = false;


        //If charges are left, tell us how many uses left.
        if (chargesLeft > 0 && chargesNeeded > 0)
        {
            int uses = chargesLeft / chargesNeeded;
            m_text.text += "(" + uses.ToString() + ")";
        }
    }
}
