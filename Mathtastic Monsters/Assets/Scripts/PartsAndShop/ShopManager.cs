﻿using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    internal equipmentList list;


    equipmentManager manager;


    public Text abilityText;
    public GameObject previewParent;
    GameObject preview;

    AbilitiesManager abilities;


    public monsterSteps tutorial;

    public AudioSource purchase;

    HeaderGUI gUI;


    public CombinedShop combinedShop;


    // Use this for initialization
    internal void Begin(CombinedShop shop)
    {
        combinedShop = shop;

        gUI = FindObjectOfType<HeaderGUI>();
      

        if (manager == null)
        {
            manager = list.equip;
        }
        ReadyPart();

        tutorial = FindObjectOfType<monsterSteps>();
    }

    //Purchase your part. No need for money/owned/available check, as this was done above.
    public void buyPart()
    {
        if (combinedShop.currentPart == null)
            return;
        ItemPart part = combinedShop.currentPart.GetComponent<ItemPart>();

        gUI.UINeedsUpdate = true;


        manager.shards -= part.cost;

        purchase.volume = PlayerPrefs.GetFloat("Volume", 0.3f);

        purchase.Play();

        part.owned = true;
        ReadyPart();
    }

    internal void ReadyPart()
    {
        combinedShop.Refresh = true;

        GameObject currentPart = combinedShop.getPart();


        if (currentPart == null) return;
        ItemPart part = currentPart.GetComponent<ItemPart>();

        if (preview != null)
            Destroy(preview);

        preview = Instantiate(currentPart, previewParent.transform, false);


        if (combinedShop.currentType == partType.LeftArm || combinedShop.currentType == partType.RightArm)
        {
            previewParent.transform.eulerAngles = new Vector3(180, 0, 0);
        }
        else
        {
            previewParent.transform.eulerAngles = new Vector3(0, 0, 0);
        }


        if (abilities == null)
            abilities = FindObjectOfType<AbilitiesManager>();

        abilityText.text = abilities.displayPower(part.ability);
    }


}