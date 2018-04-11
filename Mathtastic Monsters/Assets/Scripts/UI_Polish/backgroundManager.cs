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


    public GameObject AddPrefab;
    public GameObject subPrefab;
    public GameObject mulPrefab;
    public GameObject divPrefab;
    public GameObject ForPrefab;

    internal ScrollingParent currentScroll;


    //Initiate scene by setting background and setting up objects
    internal void startBack(playStatus a_state)
    {

        transform.SetSiblingIndex(0);

        thisScreen = GetComponent<Image>();

        GameObject helper = GameObject.Find("Helper");
        if (helper != null)
            helper.transform.SetParent(this.transform, false);

        changeBack(a_state);
    }

    //We're changing states, which means changing the background to suit that state.
    public void changeBack(playStatus a_state)
    {
        if (thisScreen == null)
            return;

        newImage = null;

        GameObject scroller = null;

        switch (a_state)
        {
            case playStatus.Start:
                newImage = mainScreen;
                break;
            case playStatus.subjectSelect:
                newImage = subjectScreen;
                break;
            case playStatus.Addition:
                scroller = Instantiate(AddPrefab, transform, false);
                //newImage = additionScreen;
                break;
            case playStatus.Subtraction:
                scroller = Instantiate(subPrefab, transform, false);

                //newImage = subtractionScreen;
                break;
            case playStatus.Multiplication:
                scroller = Instantiate(mulPrefab, transform, false);

                //newImage = multiplicationScreen;
                break;
            case playStatus.Division:
                scroller = Instantiate(divPrefab, transform, false);

                //newImage = divisionScreen;
                break;
            case playStatus.MathFortress:
                scroller = Instantiate(ForPrefab, transform, false);

                //newImage = mathfortressScreen;
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

        if(scroller)
        {
            if (currentScroll)
                Destroy(currentScroll.gameObject);

            currentScroll = scroller.GetComponent<ScrollingParent>();

            thisScreen.enabled = false;
            return;
        }


        if (currentScroll)
        {
            Destroy(currentScroll.gameObject);
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
