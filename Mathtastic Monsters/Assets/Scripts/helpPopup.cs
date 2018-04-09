using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class helpPopup : MonoBehaviour
{
    public Image HelpPanel;

    public Sprite[] HelpImages;

    public Sprite FortTen;

    public LevelSelection levelSelection;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    public void SetImage()
    {
        classType type = classType.Addition;

        type = levelSelection.currentSubject;        

        int index = (int)type;

        switch (type)
        {
            case classType.Addition:
                HelpPanel.sprite = HelpImages[0];
                break;
            case classType.Subtraction:
                HelpPanel.sprite = HelpImages[1];
                break;
            case classType.Multiplication:
                HelpPanel.sprite = HelpImages[2];
                break;
            case classType.Division:
                HelpPanel.sprite = HelpImages[3];
                break;
            case classType.Calculi:
                if (levelSelection.currentLevel > 7)
                {
                    HelpPanel.sprite = FortTen;
                }
                else
                {
                    HelpPanel.sprite = HelpImages[4];
                }
                break;
            default:
                return;
        }
        HelpPanel.gameObject.SetActive(true);
    }

    public void DisableImage()
    {
        HelpPanel.gameObject.SetActive(false);
    }
}
