using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class abilityButton : MonoBehaviour 
{
    abilityTypes thisButton; //This button's current ability.

    //Active buttons are disabled if charges left is less than needed.
    public int chargesLeft; 
    public int chargesNeeded;

    Button m_button;

    Text m_text; //On button, Displays ability name and uses left.

    playerAbilities abilities; //reference to player's ability manager.


    //Begins setting up button.

    //buttontype is the avility this button will be responsible for.
    //Charges if how many uses it has.
    //a_abilities is a reference to the ability manager.
    internal void SetUpButton(abilityTypes buttonType,int charges,playerAbilities a_abilities)
    {
        abilities = a_abilities;

        thisButton = buttonType;
        chargesLeft = charges;

        SetButtonActive();
    }

    //If type is set to none, everything will be considered default.
    internal void ResetButton()
    {
        thisButton = abilityTypes.None;
        gameObject.SetActive(false);
    }

    //Command the ability manager to use the ability, remove the charges, and check if the ability is still usable.
    public void useButton()
    {
        abilities.useButton(thisButton);
        chargesLeft -= chargesNeeded;
        SetButtonActive();
    }


    public void DisablePhase(bool enemyPhase)
    {
        switch (thisButton)
        {
            case abilityTypes.Dodge:
                if (enemyPhase && chargesLeft >= chargesNeeded)
                    m_button.interactable = true;
                else
                    m_button.interactable = false;
                break;

            case abilityTypes.Freeze:
                if (!enemyPhase && chargesLeft >= chargesNeeded)
                    m_button.interactable = true;
                else
                    m_button.interactable = false;

                break;
            case abilityTypes.Burn:
                if (enemyPhase && chargesLeft >= chargesNeeded)
                    m_button.interactable = true;
                else
                {
                    m_button.interactable = false;

                }
                break;
            case abilityTypes.StorePower:
                if (!enemyPhase && chargesLeft >= chargesNeeded)
                    m_button.interactable = true;
                else
                    m_button.interactable = false;
                break;
            default:
                break;
        }
    }

    //Set info using the ability's type.
    //This determines if the ability is usable, it's cost, it's name, etc.
    internal void SetButtonActive()
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
            case abilityTypes.Scavenger:
                m_button.interactable = false;
                return;
            case abilityTypes.Dodge:
                chargesNeeded = 2;
                break;
            case abilityTypes.BarkSkin:
                m_button.interactable = false;
                return;
            case abilityTypes.SuperSpeed:
                m_button.interactable = false;
                return;
            case abilityTypes.SlimeSkin:
                m_button.interactable = false;
                return;
            case abilityTypes.Freeze:
                chargesNeeded = 2;
                break;
            case abilityTypes.Burn:
                chargesNeeded = 2;
                break;
            case abilityTypes.BoulderFist:
                m_button.interactable = false;
                return;
            case abilityTypes.FireStorm:
                chargesNeeded = chargesLeft;
                break;
            case abilityTypes.Mastery:
                m_button.interactable = false;
                return;
            case abilityTypes.SandSlice:
                m_button.interactable = false;
                return;
            case abilityTypes.Hourglass:
                chargesNeeded = chargesLeft;
                break;
            case abilityTypes.StorePower:
                chargesNeeded = chargesLeft;
                break;
            case abilityTypes.ArmourUp:
                m_button.interactable = false;
                return;
            case abilityTypes.DoubleStrike:
                m_button.interactable = false;
                return;
            case abilityTypes.TimeLord:
                m_button.interactable = false;
                return;
            default:
                break;
        }

        //If not enough charges left, disable it.
        if (chargesLeft < chargesNeeded || chargesNeeded == 0)
            m_button.interactable = false;


        //If charges are left, tell us how many uses left.
        if (chargesLeft > 0 && chargesNeeded > 0)
        {
            int uses = chargesLeft / chargesNeeded;
            m_text.text += "(" + uses.ToString() + ")";
        }
    }
}
