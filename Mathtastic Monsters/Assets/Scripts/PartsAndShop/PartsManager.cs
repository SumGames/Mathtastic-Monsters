using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum partType
    {
        Torso, //0
        Head, //1
        LeftArm,
        RightArm,
        LeftLeg,
        RightLeg
    }

public class PartsManager : MonoBehaviour
{
    internal equipmentList list;


    partType currentType; //Type of object we want.
    List<GameObject> currentList; //The list our object is in.
    int currentIndex; //Where in array we're looking at.
    GameObject currentPart; //The object, found at the index inside the list.
    bool currentOwned; //Do we actually own this object?



    public GameObject parentOfTorso;

    public TorsoPart TorsoEquipped;

    public ItemPart headEquipped;

    public ItemPart leftArmEquipped;
    public ItemPart rightArmEquipped;

    public ItemPart leftLegEquipped;
    public ItemPart rightLegEquipped;


    public Text displayCurrent; //Display the name of the currentPart.
    public Button equipItemButton; //Turns off if we don't own this item.


    public Text previewAbilities;
    AbilitiesManager abilities;
    bool changed = false;

    monsterSteps tutorial;


    // Use this for initialization
    internal void Begin()
    {
        abilities = FindObjectOfType<AbilitiesManager>();

        list.buildCharacter(parentOfTorso, this);

        currentType = partType.Torso;
        getPart();

        tutorial = FindObjectOfType<monsterSteps>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorial && tutorial.tutorialStage < 15)
        {
            equipItemButton.interactable = false;
            return;
        }

        if (changed)
        {
            previewAbilities.text = abilities.setAbilities();
            changed = false;
        }
        if (currentPart == null)
        {
            displayCurrent.text = "";
            return;
        }
        if (!currentOwned)
        {
            equipItemButton.interactable = false;
            displayCurrent.text = currentPart.gameObject.name + " is locked";
        }
        else
        {
            equipItemButton.interactable = true;
            displayCurrent.text = currentPart.gameObject.name;
        }
    }

    public void setType(int type)
    {
        currentType = (partType)type;
        currentIndex = 0;
        getPart();
    }

    public void changeIndex(bool plus)
    {

        if (plus)
        {
            if (currentIndex >= (currentList.Count - 1))
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }

            ItemPart part = currentList[currentIndex].GetComponent<ItemPart>();
            if (!part.owned)
            {
                changeIndex(true);
                return;
            }

        }
        else
        {
            if (currentIndex == 0)
            {
                currentIndex = (currentList.Count - 1);
            }
            else
            {
                currentIndex--;
            }

            ItemPart part = currentList[currentIndex].GetComponent<ItemPart>();
            if (!part.owned)
            {
                changeIndex(false);
                return;
            }

        }
        getPart();
    }


    public void addLimb()
    {
        changed = true;

        GameObject adding;

        if (currentPart == null)
            return;


        if (currentType == partType.Torso)
        {
            addingTorso();
            return;
        }

        if (TorsoEquipped == null)
        {
            return;
        }

        switch (currentType)
        {
            case partType.Head:
                if (headEquipped != null && currentPart.name == headEquipped.gameObject.name)
                {
                    Destroy(headEquipped.gameObject);
                    headEquipped = null;

                    list.changeEquip(null, partType.Head, -2);
                    return;
                }
                if (headEquipped != null)
                {
                    Destroy(headEquipped.gameObject);
                }

                adding = Instantiate(currentPart, TorsoEquipped.headPivot.transform, false);
                headEquipped = adding.GetComponent<ItemPart>();
                list.changeEquip(currentPart, partType.Head, currentIndex);
                adding.name = currentPart.name;
                break;

            case partType.LeftArm:
                if (leftArmEquipped != null && currentPart.name == leftArmEquipped.gameObject.name)
                {
                    Destroy(leftArmEquipped.gameObject);
                    leftArmEquipped = null;
                    list.changeEquip(null, partType.LeftArm, -2);
                    return;
                }
                if (leftArmEquipped != null)
                {
                    Destroy(leftArmEquipped.gameObject);
                }
                adding = Instantiate(currentPart, TorsoEquipped.leftArmPivot.transform, false);
                leftArmEquipped = adding.GetComponent<ItemPart>();
                adding.name = currentPart.name;
                list.changeEquip(currentPart, partType.LeftArm, currentIndex);
                adding = null;
                break;

            case partType.RightArm:
                if (rightArmEquipped != null && currentPart.name == rightArmEquipped.gameObject.name)
                {
                    Destroy(rightArmEquipped.gameObject);
                    rightArmEquipped = null;
                    list.changeEquip(null, partType.RightArm, -2);
                    return;
                }
                if (rightArmEquipped != null)
                {
                    Destroy(rightArmEquipped.gameObject);
                }
                adding = Instantiate(currentPart, TorsoEquipped.rightArmPivot.transform, false);
                rightArmEquipped = adding.GetComponent<ItemPart>();
                adding.name = currentPart.name;
                list.changeEquip(currentPart, partType.RightArm, currentIndex);
                adding = null;
                break;

            case partType.LeftLeg:
                if (leftLegEquipped != null && currentPart.name == leftLegEquipped.gameObject.name)
                {
                    Destroy(leftLegEquipped.gameObject);
                    leftLegEquipped = null;
                    list.changeEquip(null, partType.LeftLeg, -2);
                    return;
                }
                if (leftLegEquipped != null)
                {
                    Destroy(leftLegEquipped.gameObject);
                }
                adding = Instantiate(currentPart, TorsoEquipped.leftLegPivot.transform, false);
                leftLegEquipped = adding.GetComponent<ItemPart>();
                list.changeEquip(currentPart, partType.LeftLeg, currentIndex);
                adding.name = currentPart.name;
                adding = null;
                break;

            case partType.RightLeg:
                if (rightLegEquipped != null && currentPart.name == rightLegEquipped.gameObject.name)
                {
                    Destroy(rightLegEquipped.gameObject);
                    rightLegEquipped = null;
                    list.changeEquip(null, partType.RightLeg, -2);
                    return;
                }
                if (rightLegEquipped != null)
                {
                    Destroy(rightLegEquipped.gameObject);
                }
                adding = Instantiate(currentPart, TorsoEquipped.RightLegPivot.transform, false);
                rightLegEquipped = adding.GetComponent<ItemPart>();
                list.changeEquip(currentPart, partType.RightLeg, currentIndex);
                adding.name = currentPart.name;
                adding = null;
                break;

            default:
                break;
        }
    }

    void addingTorso()
    {
        if (tutorial && tutorial.tutorialStage == 15)
        {
            tutorial.ProgressTutorial();
        }


        if (TorsoEquipped != null)
        {
            Destroy(TorsoEquipped.gameObject);
            TorsoEquipped = null;
            list.changeEquip(null, partType.Torso, -2);
            list.changeEquip(null, partType.Head, -2);

            list.changeEquip(null, partType.LeftArm, -2);
            list.changeEquip(null, partType.RightArm, -2);
            list.changeEquip(null, partType.LeftLeg, -2);
            list.changeEquip(null, partType.RightLeg, -2);
        }
        GameObject adding = Instantiate(currentPart, parentOfTorso.transform, true);
        adding.transform.localPosition = Vector3.zero;
        adding.transform.localScale = currentPart.transform.localScale;
        TorsoEquipped = adding.GetComponent<TorsoPart>();

        list.changeEquip(currentPart, partType.Torso, currentIndex);
        adding.name = currentPart.name;
    }

    public void getPart()
    {
        GameObject adding = null;
        switch (currentType)
        {
            case partType.Torso:
                currentList = list.listOfTorso;
                adding = list.listOfTorso[currentIndex];
                break;
            case partType.Head:
                currentList = list.listofHeads;
                adding = list.listofHeads[currentIndex];
                break;
            case partType.LeftArm:
                currentList = list.listofLeftArms;
                adding = list.listofLeftArms[currentIndex];
                break;
            case partType.RightArm:
                currentList = list.listofRightArms;
                adding = list.listofRightArms[currentIndex];
                break;

            case partType.LeftLeg:
                currentList = list.listofLeftLegs;
                adding = list.listofLeftLegs[currentIndex];
                break;

            case partType.RightLeg:
                currentList = list.listofRightLegs;
                adding = list.listofRightLegs[currentIndex];
                break;

            default:
                break;
        }
        if (adding == null)
        {
            currentPart = null;
            currentOwned = false;
            return;
        }
        currentPart = adding;
        currentOwned = adding.GetComponent<ItemPart>().owned;
    }
}