using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class abilityButton : MonoBehaviour
{
    internal abilityTypes thisButton; //This button's current ability.

    //Active buttons are disabled if charges left is less than needed.
    public int chargesLeft;
    public int chargesNeeded;

    Button m_button;


    playerAbilities abilities; //reference to player's ability manager.


    public int turnsSinceCharged;
    public int chargeTimeNeeded;

    //Begins setting up button.

    //buttontype is the avility this button will be responsible for.
    //Charges if how many uses it has.
    //a_abilities is a reference to the ability manager.
    internal void SetUpButton(abilityTypes buttonType, int charges, playerAbilities a_abilities)
    {
        abilities = a_abilities;

        thisButton = buttonType;
        chargesLeft = charges;

        SetButtonActive();

        GetComponent<Image>().sprite = abilities.Icons[((int)thisButton - 1)];

        if (buttonType == abilityTypes.Dodge || buttonType == abilityTypes.Freeze)
        {
            SetChargingButton(charges);
        }
        else
        {
            turnsSinceCharged = 0;
            chargeTimeNeeded = 0;
        }

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

    //Most active buttons do not work in some phases.
    //This checks if they're active and of that type, and if so, changes their status.
    public void DisablePhase(bool enemyPhase)
    {
        switch (thisButton)
        {
            case abilityTypes.Dodge:
                if (!enemyPhase)
                    IncrementCharge();
                SetChargingActive(enemyPhase);
                break;
            case abilityTypes.Freeze:
                if (!enemyPhase)
                    IncrementCharge();
                SetChargingActive(!enemyPhase);
                break;
            case abilityTypes.Burn:
                if (enemyPhase && chargesLeft >= chargesNeeded)
                {
                    m_button.interactable = true;
                }
                else
                {
                    m_button.interactable = true;
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
        }

        chargesNeeded = 0;
        gameObject.SetActive(true);

        ColorBlock block;

        switch (thisButton)
        {
            case abilityTypes.None:
                gameObject.SetActive(false);
                return;
            case abilityTypes.Scavenger:
                m_button.interactable = false;
                block = m_button.colors;
                block.normalColor = Color.gray;
                block.disabledColor = Color.gray;
                m_button.colors = block;
                return;
            case abilityTypes.Dodge:
                chargesNeeded = 1;
                break;
            case abilityTypes.BarkSkin:
                m_button.interactable = false;
                block = m_button.colors;
                block.normalColor = Color.gray;
                block.disabledColor = Color.gray;
                m_button.colors = block;
                return;
            case abilityTypes.SuperSpeed:
                m_button.interactable = false;
                block = m_button.colors;
                block.normalColor = Color.gray;
                block.disabledColor = Color.gray;
                m_button.colors = block;
                return;
            case abilityTypes.SlimeSkin:
                m_button.interactable = false;
                block = m_button.colors;
                block.normalColor = Color.gray;
                block.disabledColor = Color.gray;
                m_button.colors = block;
                return;
            case abilityTypes.Freeze:
                chargesNeeded = 1;
                break;
            case abilityTypes.Burn:
                chargesNeeded = 1;
                m_button.interactable = true;
                break;
            case abilityTypes.BoulderFist:
                m_button.interactable = false;
                block = m_button.colors;
                block.normalColor = Color.gray;
                block.disabledColor = Color.gray;
                m_button.colors = block;
                return;
            case abilityTypes.FireStorm:
                chargesNeeded = chargesLeft;
                m_button.interactable = true;
                break;
            case abilityTypes.Mastery:
                m_button.interactable = false;
                block = m_button.colors;
                block.normalColor = Color.gray;
                block.disabledColor = Color.gray;
                m_button.colors = block;
                return;
            case abilityTypes.SandSlice:
                m_button.interactable = false;
                block = m_button.colors;
                block.normalColor = Color.gray;
                block.disabledColor = Color.gray;
                m_button.colors = block;
                return;
            case abilityTypes.Hourglass:
                chargesNeeded = chargesLeft;
                m_button.interactable = true;
                break;
            case abilityTypes.StorePower:
                chargesNeeded = chargesLeft;
                m_button.interactable = true;

                break;
            case abilityTypes.ArmourUp:
                m_button.interactable = false;
                block = m_button.colors;
                block.normalColor = Color.gray;
                block.disabledColor = Color.gray;
                m_button.colors = block;
                return;
            case abilityTypes.DoubleStrike:
                m_button.interactable = false;
                block = m_button.colors;
                block.normalColor = Color.gray;
                block.disabledColor = Color.gray;
                m_button.colors = block;
                return;
            case abilityTypes.TimeLord:
                m_button.interactable = false;
                block = m_button.colors;
                block.normalColor = Color.gray;
                block.disabledColor = Color.gray;
                m_button.colors = block;
                return;
            default:
                break;
        }
        //If not enough charges left, disable it.
        if (chargesLeft < chargesNeeded || chargesNeeded == 0)
            m_button.interactable = false;
    }

    void SetChargingButton(int chargSpeed)
    {
        Debug.Log("Charge ");

        switch (chargSpeed)
        {
            case 1:
                chargeTimeNeeded = 8;
                break;
            case 2:
                chargeTimeNeeded = 7;
                break;
            case 3:
                chargeTimeNeeded = 6;
                break;
            case 4:
                chargeTimeNeeded = 5;
                break;
            case 5:
                chargeTimeNeeded = 4;
                break;
            case 6:
                chargeTimeNeeded = 3;
                break;
            default:
                break;
        }

        chargesLeft = chargesNeeded;
    }

    void IncrementCharge()
    {
        int usesLeft = chargesLeft / chargesNeeded;

        turnsSinceCharged++;
        if (turnsSinceCharged >= chargeTimeNeeded)
        {
            usesLeft++;
            turnsSinceCharged = 0;

            chargesLeft = usesLeft * chargesNeeded;

        }
    }

    void SetChargingActive(bool setActive)
    {
        int usesLeft = chargesLeft / chargesNeeded;

        if (setActive && usesLeft > 0)
        {
            m_button.interactable = true;
        }
        else
        {
            m_button.interactable = false;
            m_button.interactable = true;            
            m_button.interactable = false;
        }

    }
}