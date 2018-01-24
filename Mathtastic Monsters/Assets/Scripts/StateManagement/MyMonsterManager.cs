using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyMonsterManager : ParentsStateManager
{
    public GameObject mymonsterSelection; //The container that takes you to the monster overview
    public GameObject monstercustomisationSelection; //Above, for monster customisation
    public GameObject lillyhomeSelection; //Above, for Lillys home


    public Text gameInstruction; //Tells player what to do.


    //Links to part and shop managers. Links them to list and tells them to start.
    public ShopManager shop;
    public PartsManager parts;

    // Use this for initialization
    void Start()
    {
        base.Find();

        shop.list = list;
        shop.Begin();
        parts.list = list;
        parts.Begin();


        changeState(playStatus.MyMonster);

        backgrounds.startBack(playStatus.MyMonster);
    }

    //Change the game's state, closing/opening containers and changing text.
    public override void changeState(playStatus newState)
    {
        base.changeState(newState);

        disableObjects();

        gameState = newState;
        switch (gameState)
        {
            case playStatus.MyMonster:
                mymonsterSelection.SetActive(true);
                gameInstruction.text = "Welcome home, " + list.playerName.ToString() + "."; //Player 1 To be changed to monsters given name
                break;
            case playStatus.MonsterCustomisation:
                monstercustomisationSelection.SetActive(true);
                gameInstruction.text = "Customise your monster here!";
                break;
            case playStatus.LillyHome:
                lillyhomeSelection.SetActive(true);
                gameInstruction.text = "Hello, " + list.playerName.ToString() + "! trade me your shards, so I may grant you the power to defeat Lord Calculi!";
                break;
            case playStatus.subjectSelect:
                SceneManager.LoadScene(1);
                break;

            default:
                break;
        }
    }


    //Disables all objects by default so it doesn't have to be done manually.
    void disableObjects()
    {
        mymonsterSelection.SetActive(false);
        monstercustomisationSelection.SetActive(false);
        lillyhomeSelection.SetActive(false);
    }

    public void MonsterBack()
    {
        if(gameState==playStatus.MyMonster)
        {
            changeState(playStatus.subjectSelect);
        }
        else
        {
            changeState(playStatus.MyMonster);
        }

    }
}
