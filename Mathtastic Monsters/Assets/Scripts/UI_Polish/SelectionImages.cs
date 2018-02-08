using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionImages : MonoBehaviour
{
    public Sprite DefaultSprite;

    public Sprite AdditionSprite;

    public Sprite SubtractionSprite;

    public Sprite MultiplicationSprite;

    public Sprite DivisionSprite;

    public Sprite CalculusSprite;

    public Image[] Buttons;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    internal void SetSprite(classType subject)
    {

        Sprite newSprite = null;

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
            default:
                newSprite = DefaultSprite;
                break;
        }
        if (newSprite == null)
        {
            newSprite = DefaultSprite;
        }

        foreach (Image item in Buttons)
        {
            item.sprite = newSprite;
        }
    }

}
