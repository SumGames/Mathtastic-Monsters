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

    public ArmPart leftArmEquipped;
    public ArmPart rightArmEquipped;

    public LegPart leftLegEquipped;
    public LegPart rightLegEquipped;


    public Text displayCurrent; //Display the name of the currentPart.
    public Button equipItemButton; //Turns off if we don't own this item.


    public Text previewAbilities;
    AbilitiesManager abilities;
    bool changed = false;

    monsterSteps tutorial;

    bool built = false;


    // Use this for initialization
    internal void Begin()
    {
        abilities = FindObjectOfType<AbilitiesManager>();

        list.BuildCharacter(parentOfTorso, this);
        

        currentType = partType.Torso;
        getPart();

        tutorial = FindObjectOfType<monsterSteps>();        
    }

    void Start()
    {
        previewAbilities.text = abilities.setAbilities();
    }

    // Update is called once per frame
    void Update()
    {
        if(!built && TorsoEquipped!=null)
        {
            TorsoEquipped.Animate(Animations.Idle);
            built = true;
        }

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
            if (currentIndex >= (currentList.Count - 1) || currentList.Count == 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }

            if (currentList[currentIndex] == null)
                return;

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

                    list.ChangeEquip(null, partType.Head, -2);
                    return;
                }
                if (headEquipped != null)
                {
                    Destroy(headEquipped.gameObject);
                }

                adding = Instantiate(currentPart, TorsoEquipped.neckForHead.transform, false);
                headEquipped = adding.GetComponent<ItemPart>();
                list.ChangeEquip(currentPart, partType.Head, currentIndex);
                adding.name = currentPart.name;
                break;

            case partType.LeftArm:
                if (leftArmEquipped != null && currentPart.name == leftArmEquipped.gameObject.name)
                {
                    leftArmEquipped.deleteArm();
                    leftArmEquipped = null;
                    list.ChangeEquip(null, partType.LeftArm, -2);
                    return;
                }
                if (leftArmEquipped != null)
                {
                    leftArmEquipped.deleteArm();
                }
                adding = Instantiate(currentPart, TorsoEquipped.LeftArmUpper.transform, false);
                leftArmEquipped = adding.GetComponent<ArmPart>();
                leftArmEquipped.EquipArm(TorsoEquipped, TorsoEquipped.LeftArmUpper, TorsoEquipped.LeftArmFore, TorsoEquipped.LeftArmHand);

                adding.name = currentPart.name;
                list.ChangeEquip(currentPart, partType.LeftArm, currentIndex);
                adding = null;
                break;

            case partType.RightArm:
                if (rightArmEquipped != null && currentPart.name == rightArmEquipped.gameObject.name)
                {
                    rightArmEquipped.deleteArm();
                    rightArmEquipped = null;
                    list.ChangeEquip(null, partType.RightArm, -2);
                    return;
                }
                if (rightArmEquipped != null)
                {
                    rightArmEquipped.deleteArm();
                }
                adding = Instantiate(currentPart, TorsoEquipped.RightArmUpper.transform, false);

                rightArmEquipped = adding.GetComponent<ArmPart>();
                rightArmEquipped.EquipArm(TorsoEquipped, TorsoEquipped.RightArmUpper, TorsoEquipped.RightArmFore, TorsoEquipped.RightArmHand);

                adding.name = currentPart.name;
                list.ChangeEquip(currentPart, partType.RightArm, currentIndex);
                adding = null;
                break;

            case partType.LeftLeg:
                if (leftLegEquipped != null && currentPart.name == leftLegEquipped.gameObject.name)
                {
                    leftLegEquipped.DeleteLeg();
                    leftLegEquipped = null;
                    list.ChangeEquip(null, partType.LeftLeg, -2);
                    return;
                }
                if (leftLegEquipped != null)
                {
                    leftLegEquipped.DeleteLeg();
                }
                adding = Instantiate(currentPart, TorsoEquipped.RightUpperThigh.transform, false);
                leftLegEquipped = adding.GetComponent<LegPart>();
                leftLegEquipped.EquipLeg(TorsoEquipped.LeftUpperThigh, TorsoEquipped.Leftshin, TorsoEquipped.LefttAnkle, TorsoEquipped.LeftFoot);


                list.ChangeEquip(currentPart, partType.LeftLeg, currentIndex);
                adding.name = currentPart.name;
                adding = null;
                break;

            case partType.RightLeg:
                if (rightLegEquipped != null && currentPart.name == rightLegEquipped.gameObject.name)
                {
                    rightLegEquipped.DeleteLeg();
                    rightLegEquipped = null;
                    list.ChangeEquip(null, partType.RightLeg, -2);
                    return;
                }
                if (rightLegEquipped != null)
                {
                    rightLegEquipped.DeleteLeg();
                }
                adding = Instantiate(currentPart, TorsoEquipped.RightUpperThigh.transform, false);
                rightLegEquipped = adding.GetComponent<LegPart>();
                rightLegEquipped.EquipLeg(TorsoEquipped.RightUpperThigh, TorsoEquipped.Rightshin, TorsoEquipped.RightAnkle, TorsoEquipped.RightFoot);


                list.ChangeEquip(currentPart, partType.RightLeg, currentIndex);
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
        }

        list.ChangeEquip(currentPart, partType.Torso, currentIndex);

        list.BuildCharacter(parentOfTorso, this);

        built = false;

    }

    public void ModelTrash()
    {
        Destroy(TorsoEquipped.gameObject);
        list.ChangeEquip(null, partType.Head, -2);

        list.ChangeEquip(null, partType.LeftArm, -2);
        list.ChangeEquip(null, partType.RightArm, -2);
        list.ChangeEquip(null, partType.LeftLeg, -2);
        list.ChangeEquip(null, partType.RightLeg, -2);


        list.BuildCharacter(parentOfTorso, this);
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