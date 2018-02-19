using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MyMonsterManager : ParentsStateManager
{
    public GameObject lillyhomeSelection; //Above, for Lillys home


    public Text gameInstruction; //Tells player what to do.


    //Links to part and shop managers. Links them to list and tells them to start.
    public CombinedShop combinedShop;


    // Use this for initialization
    void Start()
    {
        base.Find();

        combinedShop.Begin(list);

        changeState(playStatus.LillyHome);

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
            case playStatus.LillyHome:
                lillyhomeSelection.SetActive(true);

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
        lillyhomeSelection.SetActive(false);
    }

    public void MonsterBack()
    {
        changeState(playStatus.subjectSelect);

    }
}
