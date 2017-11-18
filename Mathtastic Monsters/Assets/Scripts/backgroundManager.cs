using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class backgroundManager : MonoBehaviour
{
    public Sprite mainScreen;
    public Sprite menuScreen;
    public Sprite subjectScreen;
    public Sprite additionScreen;
    public Sprite subtractionScreen;
    public Sprite multiplicationScreen;
    public Sprite divisionScreen;
    public Sprite mathfortressScreen;
    public Sprite playerhomeScreen;
    public Sprite lillyhomeScreen;

    Image thisScreen;

    Sprite newImage;

    // Use this for initialization
    internal void startBack(playStatus a_state)
    {

        transform.SetSiblingIndex(0);

        thisScreen = GetComponent<Image>();

        GameObject helper = GameObject.Find("Helper");
        if (helper != null)
            helper.transform.SetParent(this.transform, false);

        changeBack(a_state);
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void changeBack(playStatus a_state)
    {
        if (thisScreen == null)
            return;

        newImage = null;

        switch (a_state)
        {
            case playStatus.Start:
                newImage = mainScreen;
                break;
            case playStatus.subjectSelect:
                newImage = subjectScreen;
                break;
            case playStatus.Addition:
                newImage = additionScreen;
                break;
            case playStatus.Subtraction:
                newImage = subtractionScreen;
                break;
            case playStatus.Multiplication:
                newImage = multiplicationScreen;
                break;
            case playStatus.Division:
                newImage = divisionScreen;
                break;
            case playStatus.MathFortress:
                newImage = mathfortressScreen;
                break;
            case playStatus.playing:
                return;
            case playStatus.Won:
                return;
            case playStatus.Lost:
                return;
            case playStatus.MyMonster:
                newImage = menuScreen;
                break;
            case playStatus.MonsterCustomisation:
                newImage = menuScreen;
                break;
            case playStatus.LillyHome:
                newImage = menuScreen;
                break;
            case playStatus.Options:
                newImage = menuScreen;
                break;
            case playStatus.Instructions:
                newImage = menuScreen;
                break;
            case playStatus.Parents:
                newImage = menuScreen;
                break;
            case playStatus.Credits:
                newImage = menuScreen;
                break;
            case playStatus.Login:
                newImage = mainScreen;
                break;
            case playStatus.Splash:
                newImage = mainScreen;
                break;
            default:
                break;
        }

        if (newImage != null)
        {
            thisScreen.sprite = newImage;
            thisScreen.enabled = true;
        }
        else
            thisScreen.enabled = false;
    }
}
