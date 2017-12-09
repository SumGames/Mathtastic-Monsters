using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    internal equipmentList list;

    partType currentType; //Type of object we want.
    List<GameObject> currentList; //The list our object is in.
    int currentIndex; //Where in array we're looking at.
    public GameObject currentPart; //The object, found at the index inside the list.



    public Text displayCurrent; //Display the name of the currentPart.
    public Button equipItemButton; //Turns off if we don't own this item.

    public Text Currency;

    bool available;
    string availableText;

    equipmentManager manager;


    public Text abilityText;
    public GameObject previewParent;
    GameObject preview;

    AbilitiesManager abilities;


    public monsterSteps tutorial;

    public AudioSource[] sounds;
    public AudioSource purchase;


    int stars;


    // Use this for initialization
    internal void Begin()
    {
        sounds = GetComponents<AudioSource>();
        purchase = sounds[0];
      

        if (manager == null)
        {
            manager = list.equip;
        }
        currentType = partType.Torso;
        readyPart();

        tutorial = FindObjectOfType<monsterSteps>();
        stars = list.equip.GetTotalStars();
    }

    // Update is called once per frame
    void Update()
    {
        Currency.text = list.getShards();

        if (currentPart == null)
        {
            displayCurrent.text = "";
            return;
        }

        ItemPart part = currentPart.GetComponent<ItemPart>();

        if (tutorial != null)
        {
            if (tutorial.tutorialStage < 9)
            {
                equipItemButton.interactable = false;
                displayCurrent.text = "Not ready";
                return;
            }
        }

        if (part.owned)
        {
            equipItemButton.interactable = false;
            displayCurrent.text = "Already owned";
        }
        else if (!available)
        {
            equipItemButton.interactable = false;
            displayCurrent.text = availableText;
        }
        else if (stars < part.starRequired)
        {
            displayCurrent.text = "Not enough Stars. \n Needs " + part.starRequired;
            equipItemButton.interactable = false;
        }

        else if (manager.shards < part.cost)
        {
            displayCurrent.text = "Can't afford. \nCosts " + part.cost;
            equipItemButton.interactable = false;
        }
        else
        {
            displayCurrent.text = currentPart.gameObject.name;
            equipItemButton.interactable = true;
        }
    }

    //Change from one list of objects to another.
    public void setType(int type)
    {
        currentType = (partType)type;
        currentIndex = 0;
        readyPart();
    }

    //Change the index and set the part, taking care to not go out of bounds.
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
        }
        readyPart();
    }


    //Purchase your part. No need for money/owned/available check, as this was done above.
    public void buyPart()
    {
        if (currentPart == null)
            return;
        ItemPart part = currentPart.GetComponent<ItemPart>();


        manager.shards -= part.cost;
        purchase.Play();

        part.owned = true;
        readyPart();
    }

    //Use your index, and list you're checking to find the part you want.
    public GameObject getPart()
    {
        switch (currentType)
        {
            case partType.Torso:
                currentList = list.listOfTorso;
                return list.listOfTorso[currentIndex];
            case partType.Head:
                currentList = list.listofHeads;
                return list.listofHeads[currentIndex];

            case partType.LeftArm:
                currentList = list.listofLeftArms;
                return list.listofLeftArms[currentIndex];
            case partType.RightArm:
                currentList = list.listofRightArms;
                return list.listofRightArms[currentIndex];

            case partType.LeftLeg:
                currentList = list.listofLeftLegs;
                return list.listofLeftLegs[currentIndex];

            case partType.RightLeg:
                currentList = list.listofRightLegs;
                return list.listofRightLegs[currentIndex];
            default:
                break;
        }
        currentList = null;
        return null;
    }


    void readyPart()
    {
        currentPart = getPart();
        if (currentPart == null) return;
        ItemPart part = currentPart.GetComponent<ItemPart>();

        if (preview != null)
            Destroy(preview);

        preview = Instantiate(currentPart, previewParent.transform, false);
        preview.transform.localScale = preview.GetComponent<ItemPart>().Scale;

        if (abilities == null)
            abilities = FindObjectOfType<AbilitiesManager>();

        abilityText.text = abilities.displayPower(part.ability);

        available = part.checkAvailable(manager);
        availableText = part.setAvailableText();
    }

    public void getMoney()
    {
        list.equip.shards += 100;
    }
    public void resetOwned()
    {
        list.equip.shards = 0;
        foreach (GameObject body in list.listOfTorso)
        {
            body.GetComponent<TorsoPart>().owned = false;
        }

        foreach (GameObject limb in list.listofHeads)
        {
            limb.GetComponent<ItemPart>().owned = false;
        }

        foreach (GameObject limb in list.listofLeftLegs)
        {
            limb.GetComponent<ItemPart>().owned = false;
        }

        foreach (GameObject limb in list.listofRightLegs)
        {
            limb.GetComponent<ItemPart>().owned = false;
        }

        foreach (GameObject limb in list.listofLeftArms)
        {
            limb.GetComponent<ItemPart>().owned = false;
        }

        foreach (GameObject limb in list.listofRightArms)
        {
            limb.GetComponent<ItemPart>().owned = false;
        }
    }
}