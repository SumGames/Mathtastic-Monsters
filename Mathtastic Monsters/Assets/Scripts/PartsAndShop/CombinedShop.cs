using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombinedShop : MonoBehaviour
{

    internal partType currentType; //Type of object we want.
    List<GameObject> currentList; //The list our object is in.
    internal int currentIndex; //Where in array we're looking at.
    internal GameObject currentPart; //The object, found at the index inside the list.


    public Text displayCurrent; //Display the name of the currentPart.
    public Button CurrentButton; //Turns off if we don't own this item.

    public Button BuyOrEquip;



    public equipmentList list;
    equipmentManager manager;



    public PartsManager parts;
    public ShopManager shop;


    int stars;

    internal bool Refresh;

    public SelectPartTypes selectPart;

    public monsterSteps monsterSteps;


    // Use this for initialization
    internal void Begin (equipmentList a_list)
    {

        list = a_list;
        manager = list.equip;

        shop.list = list;
        parts.list = list;


        shop.Begin(this);
        parts.Begin(this);

        currentType = partType.Torso;

        stars = manager.GetTotalStars();

        getPart();

        Refresh = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!Refresh)
            return;

        Refresh = false;

        if (monsterSteps && !monsterSteps.EnableEquip)
        {
            BuyOrEquip.interactable = false;
            displayCurrent.text = "";
            return;
        }


        if (currentPart == null)
        {
            BuyOrEquip.interactable = false;
            displayCurrent.text = "";
            return;
        }

        ItemPart part = currentPart.GetComponent<ItemPart>();


        if (part.owned)
        {
            CurrentButton.interactable = false;
            displayCurrent.text = currentPart.gameObject.name;
        }
        else
        {
            CurrentButton.interactable = true;
            displayCurrent.text = currentPart.gameObject.name;
        }

        if (part.owned)
        {
            BuyOrEquip.interactable = true;
            selectPart.SelectEquipButton(true);
            return;
        }       
        else if (stars < part.starRequired)
        {
            displayCurrent.text = "Not enough Stars.\n Needs " + part.starRequired + " Stars";
            BuyOrEquip.interactable = false;
            selectPart.SelectEquipButton(false);
        }

        else if (manager.shards < part.cost)
        {
            displayCurrent.text = "Can't afford.\n Costs " + part.cost + " Shards";
            BuyOrEquip.interactable = false;
            selectPart.SelectEquipButton(false);
        }
        else
        {
            displayCurrent.text = "Buy " + currentPart.gameObject.name + "for " + part.cost + "?";

            BuyOrEquip.interactable = true;
            selectPart.SelectEquipButton(false);
        }

    }

    public void setType(int type)
    {
        currentType = (partType)type;
        currentIndex = 0;
        getPart();

        shop.ReadyPart();

        Refresh = true;

    }

    public GameObject getPart()
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
            return null;
        }
        currentPart = adding;

        return adding;
    }


    //Change the index and set the part, taking care to not go out of bounds.
    public void changeIndex(bool plus)
    {
        Refresh = true;
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
        shop.ReadyPart();
    }

    public void EquipOrBuy()
    {
        ItemPart part = currentPart.GetComponent<ItemPart>();

        if (part.owned)
        {
            parts.addLimb();
        }
        else
        {
            shop.buyPart();
        }
        Refresh = true;
    }

    public void AddCoins()
    {
        manager.shards += 100;

        Refresh = true;

        FindObjectOfType<HeaderGUI>().UINeedsUpdate = true;

    }
}
