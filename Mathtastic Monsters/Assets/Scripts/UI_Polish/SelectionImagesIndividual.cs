using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionImagesIndividual : MonoBehaviour
{
    public Sprite DefaultSprite;

    public Sprite[] AdditionSprite;

    public Sprite[] SubtractionSprite;

    public Sprite[] MultiplicationSprite;

    public Sprite[] DivisionSprite;

    public Sprite[] CalculusSprite;

    public Image[] Buttons;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void SetSprite(classType subject)
    {

        Sprite[] newSprite = null;

        switch (subject)
        {
            case classType.Addition:
                newSprite = AdditionSprite;
                break;
            case classType.Subtraction:
                newSprite = SubtractionSprite;
                break;
            case classType.Multiplication:
                newSprite = MultiplicationSprite;
                break;
            case classType.Division:
                newSprite = DivisionSprite;
                break;
            case classType.Calculi:
                newSprite = CalculusSprite;
                break;
        }
        if (newSprite == null)
        {
            foreach (Image item in Buttons)
            {
                item.sprite = DefaultSprite;
            }
            return;
        }

        for (int i = 0; i < Buttons.Length; i++)
        {
            if (i >= newSprite.Length)
            {
                Buttons[i].sprite = DefaultSprite;
                continue;
            }
            if (newSprite[i] == null)
            {
                Buttons[i].sprite = DefaultSprite;
                continue;
            }
            Buttons[i].sprite = newSprite[i];
        }

    }

}
