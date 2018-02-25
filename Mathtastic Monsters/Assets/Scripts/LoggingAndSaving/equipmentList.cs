using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class equipmentList : MonoBehaviour
{
    public List<GameObject> listOfTorso;
    public List<GameObject> listofHeads;

    public List<GameObject> listofLeftArms;
    public List<GameObject> listofRightArms;

    public List<GameObject> listofLeftLegs;
    public List<GameObject> listofRightLegs;

    public equipmentManager equip; //What will be saving.

    public string playerName;


    //The set of Game objects that will be created when the Monster is needed.
    public GameObject currentTorsoPrefab;
    internal GameObject currentHeadPrefab;

    internal GameObject currentLeftArmPrefab;
    internal GameObject currentRightArmPrefab;

    internal GameObject currentLeftLegPrefab;
    internal GameObject currentRightLegPrefab;

    internal bool skipping;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    //If we hit close button.
    void OnApplicationQuit()
    {
        Save();
    }

    //If we suspend the app, incase we never come back.
    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
    }


    //If our profile wasn't special, save it as .gd.
    internal void Save()
    {
        if (playerName == "Guest" || playerName == "")
            return;

        //Setting the equipmentManager's availability flags.
        SetManagerAvailabilityUsingList();

        string fileName = Application.persistentDataPath + "/" + playerName + ".gd";

        Debug.Log(fileName);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(fileName);
        bf.Serialize(file, equip);
        file.Close();
    }


    //Deserialise and load the file.
    bool Load()
    {
        string fileName = Application.persistentDataPath + "/" + playerName + ".gd";
        if (File.Exists(fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileName, FileMode.Open);
            equip = (equipmentManager)bf.Deserialize(file);
            file.Close();
            return true;
        }
        return false;
    }

    //Take variables from this list and add them to equipment Manager
    public void SetManagerAvailabilityUsingList()
    {
        for (int i = 0; i < listOfTorso.Capacity && i < equip.torsoAvailability.Length; i++)
        {
            ItemPart item = listOfTorso[i].GetComponent<ItemPart>();
            equip.torsoAvailability[i] = item.owned;
        }

        for (int i = 0; i < listofHeads.Capacity && i < equip.headAvailability.Length; i++)
        {
            ItemPart item = listofHeads[i].GetComponent<ItemPart>();
            equip.headAvailability[i] = item.owned;
        }


        for (int i = 0; i < listofLeftArms.Capacity && i < equip.leftArmAvailability.Length; i++)
        {
            ItemPart item = listofLeftArms[i].GetComponent<ItemPart>();
            equip.leftArmAvailability[i] = item.owned;
        }
        for (int i = 0; i < listofRightArms.Capacity && i < equip.rightArmAvailability.Length; i++)
        {
            ItemPart item = listofRightArms[i].GetComponent<ItemPart>();
            equip.rightArmAvailability[i] = item.owned;
        }

        for (int i = 0; i < listofLeftLegs.Capacity && i < equip.leftLegAvailability.Length; i++)
        {
            ItemPart item = listofLeftLegs[i].GetComponent<ItemPart>();
            equip.leftLegAvailability[i] = item.owned;
        }
        for (int i = 0; i < listofRightLegs.Capacity && i < equip.rightLegAvailability.Length; i++)
        {
            ItemPart item = listofRightLegs[i].GetComponent<ItemPart>();
            equip.rightLegAvailability[i] = item.owned;
        }
    }

    //Ask the manager's list if the items on the list are owned.
    public void SetAvailabilityUsingManager()
    {
        for (int i = 0; i < listOfTorso.Capacity && i < equip.torsoAvailability.Length; i++)
        {
            ItemPart item = listOfTorso[i].GetComponent<ItemPart>();
            item.owned = equip.torsoAvailability[i];
        }
        for (int i = 0; i < listofHeads.Capacity && i < equip.headAvailability.Length; i++)
        {
            ItemPart item = listofHeads[i].GetComponent<ItemPart>();
            item.owned = equip.headAvailability[i];
        }

        for (int i = 0; i < listofLeftArms.Capacity && i < equip.leftArmAvailability.Length; i++)
        {
            ItemPart item = listofLeftArms[i].GetComponent<ItemPart>();
            item.owned = equip.leftArmAvailability[i];

            item = listofRightArms[i].GetComponent<ItemPart>();
            item.owned = equip.rightArmAvailability[i];
        }

        for (int i = 0; i < listofLeftLegs.Capacity && i < equip.leftLegAvailability.Length; i++)
        {
            ItemPart item = listofLeftLegs[i].GetComponent<ItemPart>();
            item.owned = equip.leftLegAvailability[i];

            item = listofRightLegs[i].GetComponent<ItemPart>();
            item.owned = equip.rightLegAvailability[i];
        }
    }

    //Check the list of items and use the manager's index to get our monster ready for building.
    void SetEquippedUsingIndex(int[] equipped)
    {
        if (equip.equippedParts[(int)partType.Torso] >= 0)
        {
            currentTorsoPrefab = listOfTorso[equip.equippedParts[(int)partType.Torso]];
        }
        else
        {
            currentTorsoPrefab = null;
        }

        if (equip.equippedParts[(int)partType.Head] >= 0)
            currentHeadPrefab = listofHeads[equip.equippedParts[(int)partType.Head]];
        else
            currentHeadPrefab = null;


        if (equip.equippedParts[(int)partType.LeftArm] >= 0)
            currentLeftArmPrefab = listofLeftArms[equip.equippedParts[(int)partType.LeftArm]];
        else
            currentLeftArmPrefab = null;

        if (equip.equippedParts[(int)partType.RightArm] >= 0)
            currentRightArmPrefab = listofRightArms[equip.equippedParts[(int)partType.RightArm]];
        else
            currentRightArmPrefab = null;


        if (equip.equippedParts[(int)partType.LeftLeg] >= 0)
            currentLeftLegPrefab = listofLeftLegs[equip.equippedParts[(int)partType.LeftLeg]];
        else
            currentLeftLegPrefab = null;

        if (equip.equippedParts[(int)partType.RightLeg] >= 0)
            currentRightLegPrefab = listofRightLegs[equip.equippedParts[(int)partType.RightLeg]];
        else
            currentRightLegPrefab = null;


    }

    //Change our index and the part we'll spawn. Called when we make a new monster.
    public void ChangeEquip(GameObject a_setting, partType a_type, int index)
    {
        equip.setEquipped(a_type, index);

        switch (a_type)
        {
            case partType.Torso:
                currentTorsoPrefab = a_setting;
                break;
            case partType.Head:
                currentHeadPrefab = a_setting;
                break;

            case partType.LeftArm:
                currentLeftArmPrefab = a_setting;
                break;
            case partType.RightArm:
                currentRightArmPrefab = a_setting;
                break;

            case partType.LeftLeg:
                currentLeftLegPrefab = a_setting;
                break;
            case partType.RightLeg:
                currentRightLegPrefab = a_setting;
                break;
            default:
                break;
        }
    }

    //Build the arrays for the equipmentlists, and set everything to false/0/null.
    void NewGame()
    {
        equip.StarsAcquired = new int[75];

        PlayerPrefs.SetInt("Transitions", 1);

        equip.shards = 0;
        equip.tutorialComplete = false;

        equip.completedLevels = new int[5];

        equip.equippedParts = new int[6];

        for (int i = 0; i < 5; i++)
        {
            equip.completedLevels[i] = 0;

            if (i < 4)
                equip.equippedParts[i] = -2;
        }

        equip.torsoAvailability = new bool[listOfTorso.Capacity];
        equip.headAvailability = new bool[listofHeads.Capacity];
        equip.leftArmAvailability = new bool[listofLeftArms.Capacity];
        equip.rightArmAvailability = new bool[listofRightArms.Capacity];

        equip.leftLegAvailability = new bool[listofLeftLegs.Capacity];
        equip.rightLegAvailability = new bool[listofRightLegs.Capacity];

        for (int i = 0; i < equip.torsoAvailability.Length; i++)
        {
            ItemPart item = listOfTorso[i].GetComponent<ItemPart>();
            item.owned = false;
        }

        foreach (GameObject item in listOfTorso)
        {
            ItemPart mod = item.GetComponent<ItemPart>();
            mod.owned = false;
        }
        foreach (GameObject item in listofHeads)
        {
            ItemPart mod = item.GetComponent<ItemPart>();
            mod.owned = false;
        }

        foreach (GameObject item in listofLeftArms)
        {
            ItemPart mod = item.GetComponent<ItemPart>();
            mod.owned = false;
        }
        foreach (GameObject item in listofRightArms)
        {
            ItemPart mod = item.GetComponent<ItemPart>();
            mod.owned = false;
        }

        foreach (GameObject item in listofLeftLegs)
        {
            ItemPart mod = item.GetComponent<ItemPart>();
            mod.owned = false;
        }

        foreach (GameObject item in listofRightLegs)
        {
            ItemPart mod = item.GetComponent<ItemPart>();
            mod.owned = false;
        }

        EquipDefault();

        Save();
    }

    void EquipDefault()
    {
        listOfTorso[0].GetComponent<ItemPart>().owned = true;
        ChangeEquip(listOfTorso[0], partType.Torso, 0);

        listofHeads[0].GetComponent<ItemPart>().owned = true;
        ChangeEquip(listofHeads[0], partType.Head, 0);

        listofLeftArms[0].GetComponent<ItemPart>().owned = true;
        ChangeEquip(listofLeftArms[0], partType.LeftArm, 0);

        listofRightArms[0].GetComponent<ItemPart>().owned = true;
        ChangeEquip(listofRightArms[0], partType.RightArm, 0);

        listofLeftLegs[0].GetComponent<ItemPart>().owned = true;
        ChangeEquip(listofLeftLegs[0], partType.LeftLeg, 0);

        listofRightLegs[0].GetComponent<ItemPart>().owned = true;
        ChangeEquip(listofRightLegs[0], partType.RightLeg, 0);
    }

    internal void RemoveHead()
    {
        listofHeads[0].GetComponent<ItemPart>().owned = false;
        ChangeEquip(null, partType.Head, -1);

    }

    //Set up our profile based upon the conditions we gave.
    public void startGame(string nameGiven, bool a_begin)
    {
        playerName = nameGiven;

        if (a_begin)
        {
            NewGame();
        }
        else
        {
            if (Load())
            {
                SetAvailabilityUsingManager();
                SetEquippedUsingIndex(equip.equippedParts);
            }
            else
                NewGame();
        }


        if (String.Equals(nameGiven, "Lilly", StringComparison.OrdinalIgnoreCase))
        {
            ByPassRestrictions();
            return;
        }
    }

    public GameObject BuildCharacter(GameObject characterContainer, PartsManager caller = null)
    {
        SetEquippedUsingIndex(equip.equippedParts);

        if (currentTorsoPrefab == null)
            return null;

        GameObject ad = Instantiate(currentTorsoPrefab, characterContainer.transform, false);
        ad.transform.localPosition = Vector3.zero;
        if (caller != null)
        {
            caller.TorsoEquipped = ad.GetComponent<TorsoPart>();
        }

        TorsoPart parent = ad.GetComponent<TorsoPart>();

        if (currentHeadPrefab != null)
        {
            GameObject adding = Instantiate(currentHeadPrefab, parent.neckForHead.transform, false);
            adding.transform.localPosition = Vector3.zero;
            if (caller != null)
            {
                caller.headEquipped = adding.GetComponent<ItemPart>();
            }
        }

        if (currentLeftArmPrefab != null)
        {
            GameObject adding = Instantiate(currentLeftArmPrefab, parent.LeftArmUpper.transform, false);
            ArmPart armPart = adding.GetComponent<ArmPart>();
            armPart.EquipArm(parent, parent.LeftArmUpper, parent.LeftArmFore, parent.LeftArmHand);
            if (caller != null)
            {
                caller.leftArmEquipped = armPart;
            }
        }
        if (currentRightArmPrefab != null)
        {
            GameObject adding = Instantiate(currentRightArmPrefab, parent.RightArmUpper.transform, false);
            ArmPart armPart = adding.GetComponent<ArmPart>();

            armPart.EquipArm(parent, parent.RightArmUpper, parent.RightArmFore, parent.RightArmHand);
            if (caller != null)
            {
                caller.rightArmEquipped = armPart;
            }
        }


        if (currentLeftLegPrefab != null)
        {
            GameObject adding = Instantiate(currentLeftLegPrefab, parent.LeftUpperThigh.transform, false);
            LegPart legPart = adding.GetComponent<LegPart>();

            legPart.EquipLeg(parent.LeftUpperThigh, parent.Leftshin, parent.LefttAnkle, parent.LeftFoot);
            if (caller != null)
            {
                caller.leftLegEquipped = legPart;
            }
        }
        if (currentRightLegPrefab != null)
        {
            GameObject adding = Instantiate(currentRightLegPrefab, parent.RightUpperThigh.transform, false);
            LegPart legPart = adding.GetComponent<LegPart>();

            legPart.EquipLeg(parent.RightUpperThigh, parent.Rightshin, parent.RightAnkle, parent.RightFoot);
            if (caller != null)
            {
                caller.rightLegEquipped = legPart;
            }
        }

        if (parent.bodyType == BodyType.FourArm)
        {
            Debug.Log("Four");

            if (currentRightArmPrefab != null)
            {
                GameObject adding = Instantiate(currentRightArmPrefab, parent.LowerRightArmUpper.transform, false);
                ArmPart armPart = adding.GetComponent<ArmPart>();

                armPart.EquipArm(parent, parent.LowerRightArmUpper, parent.LowerRightArmFore, parent.LowerRightArmHand);
            }

            if (currentLeftArmPrefab != null)
            {
                GameObject adding = Instantiate(currentLeftArmPrefab, parent.LowerLeftArmUpper.transform, false);
                ArmPart armPart = adding.GetComponent<ArmPart>();

                armPart.EquipArm(parent, parent.LowerLeftArmUpper, parent.LowerLeftArmFore, parent.LowerLeftArmHand);
            }

        }
        return ad;
    }

    internal void ByPassRestrictions()
    {
        NewGame();

        for (int i = 0; i < 5; i++)
        {
            equip.completedLevels[i] = 16;
        }
        equip.shards = 999999;

        for (int i = 0; i < equip.torsoAvailability.Length; i++)
        {
            ItemPart item = listOfTorso[i].GetComponent<ItemPart>();
            item.owned = true;
        }
        foreach (GameObject item in listofHeads)
        {
            ItemPart mod = item.GetComponent<ItemPart>();
            mod.owned = true;
        }

        foreach (GameObject item in listofLeftArms)
        {
            ItemPart mod = item.GetComponent<ItemPart>();
            mod.owned = true;
        }
        foreach (GameObject item in listofRightArms)
        {
            ItemPart mod = item.GetComponent<ItemPart>();
            mod.owned = true;
        }

        foreach (GameObject item in listofLeftLegs)
        {
            ItemPart mod = item.GetComponent<ItemPart>();
            mod.owned = true;
        }

        foreach (GameObject item in listofRightLegs)
        {
            ItemPart mod = item.GetComponent<ItemPart>();
            mod.owned = true;
        }
    }

    public string getShards()
    {
        return equip.shards.ToString();
    }
}