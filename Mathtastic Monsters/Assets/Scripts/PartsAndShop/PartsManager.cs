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


    public GameObject parentOfTorso;

    public TorsoPart TorsoEquipped;

    public ItemPart headEquipped;

    public ArmPart leftArmEquipped;
    public ArmPart rightArmEquipped;

    public LegPart leftLegEquipped;
    public LegPart rightLegEquipped;


    public Text displayCurrent; //Display the name of the currentPart.
    public Button equipItemButton; //Turns off if we don't own this item.
    
    public AbilitiesManager abilities;
    bool changed = false;

    monsterSteps tutorial;

    bool built = false;


    CombinedShop combinedShop;



    public Image[] previewIcons;

    public Sprite[] iconsList;


    public GameObject AbilityDescription;


    int[] abilityList = new int[6];

    public Text Name;
    public Image DescriptionImage;
    public Text UsesText;
    public Text DescriptionText;


    // Use this for initialization
    internal void Begin(CombinedShop shop)
    {
        combinedShop = shop;

        abilities = FindObjectOfType<AbilitiesManager>();

        list.BuildCharacter(parentOfTorso, this);

        tutorial = FindObjectOfType<monsterSteps>();

        changed = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!built && TorsoEquipped != null)
        {
            TorsoEquipped.Animate(Animations.Idle);
            built = true;
        }


        if (changed)
        {
            settingAbilityIcons();

            changed = false;
            combinedShop.Refresh = true;
        }
    }


    void settingAbilityIcons()
    {
        abilities.setAbilities(list);

        for (int i = 0; i < previewIcons.Length; i++)
        {
            previewIcons[i].enabled = false;
        }


        int j = 0;
        for (int i = 1; i < abilities.abilities.Length && j < previewIcons.Length; i++)
        {
            if (abilities.abilities[i] > 0)
            {
                abilityList[j] = i;

                previewIcons[j].sprite = iconsList[(i - 1)];
                previewIcons[j].enabled = true;
                j++;
            }
        }
    }


    public void addLimb()
    {
        combinedShop.Refresh = true;

        changed = true;

        GameObject adding;

        GameObject currentPart = combinedShop.currentPart; //The object, found at the index inside the list.

        int currentIndex = combinedShop.currentIndex;

        partType currentType = combinedShop.currentType;



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

        if (TorsoEquipped.bodyType == BodyType.FourArm)
        {
            Destroy(TorsoEquipped.gameObject);
            list.BuildCharacter(parentOfTorso, this);
            return;
        }

    }

    void addingTorso()
    {
        GameObject currentPart = combinedShop.currentPart; //The object, found at the index inside the list.

        int currentIndex = combinedShop.currentIndex;

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


    public void ChecKability(int i)
    {
        AbilityDescription.SetActive(true);

        abilityTypes ability = (abilityTypes)abilityList[i];

        Name.text = ability.ToString();

        DescriptionImage.sprite = iconsList[(abilityList[i] - 1)];

        UsesText.text = "Parts = " + abilities.abilities[abilityList[i]];

        DescriptionText.text = abilities.displayPower(ability);

    }

    public void CloseAgain()
    {
        AbilityDescription.SetActive(false);
    }
}