using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPartTypes : MonoBehaviour
{
    public Sprite[] DefaultSprite;
    public Sprite[] SelectedSprite;

    public Image[] Buttons;


    public Image BuyEquipButton;
    public Sprite BuySprite;
    public Sprite EquipSprite;

	// Use this for initialization
	void Start ()
    {
        SelectPartType(1);		
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    public void SelectPartType(int index)
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].sprite = DefaultSprite[i];
        }
        Buttons[index].sprite = SelectedSprite[index];
    }

    public void SelectEquipButton(bool Equip)
    {
        if (Equip)
            BuyEquipButton.sprite = EquipSprite;
        else
            BuyEquipButton.sprite = BuySprite;

    }
}
